using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using GameFunctions;
using GameTK.Framework_Feeling2D;
using GameTK.Modules_Sound;

namespace GameTK {

    // Step:
    // 0. Binding MonoBehaviour
    // 1. Ctor
    // 2. Inject
    // 3. Init
    // 4. Tick (every frame)
    // 5. Call API when needed
    public class Feeling2DFramework : MonoBehaviour {

        [SerializeField] Camera mainCamera;
        [SerializeField] Camera[] otherCameras;
        [SerializeField] Volume globalVolume;
        [SerializeField] float rumbleIntensity;

        Feeling2DFrameworkContext ctx;

        Feeling2DFrameworkEvents events;
        public Feeling2DFrameworkEvents Events => events;

        #region Lifecycle
        public void Ctor() {
            ctx = new Feeling2DFrameworkContext();
            events = new Feeling2DFrameworkEvents();

            ctx.cameraModule = new CameraModule();
            ctx.vfxModule = new VFXModule();
            ctx.soundModule = new SoundModule();
            ctx.rendererModule = new RendererModule();
            ctx.rumbleModule = new RumbleModule();

            ctx.vfxModule.Events.OnGetBelongPosHandle = (uniqueSig) => {
                return events.OnGetBelongPos(uniqueSig);
            };

            ctx.soundModule.Events.OnGetBelongPosHandle = (uniqueSig) => {
                return events.OnGetBelongPos(uniqueSig);
            };
        }

        public void Inject() {

            ctx.cameraModule.Inject(mainCamera, otherCameras);

            ctx.vfxModule.Inject();

            ctx.soundModule.Inject();

            ctx.rendererModule.Inject(globalVolume);

            ctx.rumbleModule.Inject(rumbleIntensity);

        }

        public IEnumerator InitIE(int enum_logLevel) {
            FFWLog.logLevel = enum_logLevel;
            ctx.cameraModule.Init();
            yield return ctx.soundModule.InitIE();
            yield return ctx.vfxModule.InitIE();
            ctx.rendererModule.Init();
            ctx.rumbleModule.Init();
        }

        public void Release() {
            ctx.soundModule.Release();
            ctx.vfxModule.Release();
        }

        public void Tick(SoundModuleUpdateArgs soundModuleUpdateArgs, Vector2 cameraTargetPos, Gamepad gamepad, float dt) {
            ctx.soundModule.Tick(soundModuleUpdateArgs, dt);
            ctx.cameraModule.Tick(cameraTargetPos, dt);
            ctx.rendererModule.Tick(dt);
            ctx.vfxModule.Tick(dt);
            ctx.rumbleModule.Tick(gamepad, dt);
        }

        public void TearDown() {
            ctx.soundModule.TearDown_ButBGM();
            ctx.vfxModule.TearDown();
            ctx.rumbleModule.StopAll();
            ctx.rendererModule.StopAllEffects();
            ctx.cameraModule.StopAllEffects();
        }
        #endregion

        #region API: Framework
        public void Feeling_ActWithoutBelong(Vector2 pos, FeelingSO feelingSO) {
            Feeling_Act(UniqueSignature.Zero, pos, feelingSO.tm);
        }

        public void Feeling_Act(UniqueSignature belongUniqueSig, Vector2 pos, FeelingSO feelingSO) {
            if (feelingSO == null) {
                FFWLog.LogError("FeelingSO is null");
                return;
            }
            Feeling_Act(belongUniqueSig, pos, feelingSO.tm);
        }

        public void Feeling_Act(UniqueSignature belongUniqueSig, Vector2 pos, in FeelingTM tm) {

            if (tm.hasVFX) {
                foreach (var vfx in tm.vfxs) {
                    ctx.vfxModule.Spawn(vfx, belongUniqueSig, pos);
                }
            }

            if (tm.hasCameraShake) {
                ctx.cameraModule.Effect_Shake(tm.cameraShakeAmplitude, tm.cameraShakeFrequency, tm.cameraShakeDuration);
            }

            if (tm.hasPPShake) {
                ctx.rendererModule.ShakeScreen_Begin(tm.ppShakeAmplitude, tm.ppShakeFrequency, tm.ppShakeDuration);
            }

            if (tm.hasCameraZoomIn) {
                const float logicFPS = 1 / 24f;
                ctx.cameraModule.Effect_ZoomIn(tm.cameraZoomInEasingType, tm.cameraZoomInMultiplier,
                                               tm.cameraZoomInFrameCount * logicFPS,
                                               tm.isCameraZoomInAutoRestore,
                                               tm.cameraZoomInAutoRestoreEasingType,
                                               tm.cameraZoomInAutoRestoreFrameCount * logicFPS,
                                               tm.cameraZoomInAutoRestoreDelayFrameCount * logicFPS);
            }

            if (tm.hasSound) {
                foreach (var soundTM in tm.sounds) {
                    ctx.soundModule.Play(soundTM, belongUniqueSig, pos);
                }
            }

            if (tm.isStopAllBGM) {
                ctx.soundModule.BGM_StopAll(tm.stopAllBGMDuration);
            }

            if (tm.isStopAllBGMByLayer) {
                ctx.soundModule.BGM_StopAllByLayer(tm.stopAllBGMLayer, tm.stopAllBGMLayerDuration);
            }

            if (tm.isPPFilmBorderFadeIn) {
                ctx.rendererModule.FilmBorder_FadeIn(tm.filmBorderFadeInScreenEdgePercent, tm.filmBorderFadeInEasingType, tm.filmBorderFadeInDuration);
            }

            if (tm.isPPFilmBorderFadeOut) {
                ctx.rendererModule.FilmBorder_FadeOut(tm.filmBorderFadeOutScreenEdgePercent, tm.filmBorderFadeOutEasingType, tm.filmBorderFadeOutDuration);
            }

            if (tm.hasRumble) {
                foreach (var rumble in tm.rumbles) {
                    ctx.rumbleModule.Rumble(rumble);
                }
            }
        }

        public void Feeling_Stop(UniqueSignature belongUniqueSig, FeelingSO feelingSO) {
            Feeling_Stop(belongUniqueSig, feelingSO.tm);
        }

        public void Feeling_Stop(UniqueSignature belong, in FeelingTM tm) {
            if (tm.hasSound) {
                foreach (var sfx in tm.sounds) {
                    ctx.soundModule.Sound_Stop(belong, sfx.typeGroup, sfx.typeID);
                }
            }

            if (tm.hasVFX) {
                foreach (var vfx in tm.vfxs) {
                    ctx.vfxModule.UnspawnBelongAndTypeID(belong, vfx.typeGroup, vfx.typeID);
                }
            }
        }

        public void Feeling_DestroyBelong(UniqueSignature belong) {
            Sound_DestroyBelong(belong);
            ctx.vfxModule.UnspawnBelong(belong);
        }
        #endregion

        #region API: Camera
        public Camera Camera_GetMain() {
            return mainCamera;
        }

        public void Camera_FollowChangeTarget(Vector2 targetPos) {
            ctx.cameraModule.Follow_ChangeTarget(targetPos);
        }

        public void Camera_FastForward(float dt) {
            ctx.cameraModule.FastForward(dt);
        }

        public void Camera_Confiner_Enable(bool isEnable) {
            ctx.cameraModule.Confiner_Enable(isEnable);
        }

        public void Camera_Confiner_Set(Vector2 leftDown, Vector2 rightUp) {
            ctx.cameraModule.Confiner_Set(leftDown, rightUp);
        }

        public void Camera_Follow_Enable(bool isEnable) {
            ctx.cameraModule.Follow_Enable(isEnable);
        }

        public void Camera_Follow_Change(Vector2 centerPos, Vector3 vector3, float xDamping, float yDamping) {
            ctx.cameraModule.Follow_Init(centerPos, vector3, xDamping, yDamping);
        }

        public void Camera_Follow_DeadZone_Set(Vector2 size) {
            ctx.cameraModule.Follow_DeadZone_Set(size);
        }

        public void Camera_Orthographic_SetBaseSize(float orthographicSize, float ratioDisplay) {
            ctx.cameraModule.Orthographic_SetBaseSize(orthographicSize, ratioDisplay);
        }

        public Vector3 Camera_WorldToScreenPoint(Vector3 worldPos) {
            return ctx.cameraModule.WorldToScreenPoint(worldPos);
        }

        public void Camera_Orthographic_SetMultiplier(float mul) {
            ctx.cameraModule.Orthographic_SetMultiplier(mul);
        }

        public void Camera_Effect_ZoomIn(GFEasingEnum easingType, float multiplier, float duration, bool isAutoRestore, GFEasingEnum autoRestoreEasingType, float autoRestoreDuration, float autoRestoreDelay) {
            ctx.cameraModule.Effect_ZoomIn(easingType, multiplier, duration, isAutoRestore, autoRestoreEasingType, autoRestoreDuration, autoRestoreDelay);
        }
        #endregion

        #region  API: GhostTrail
        public void GhostTrail_SetFrame(Vector2 centerPos, Quaternion rot, Vector3 scale, Sprite sprite, float ghostTrailDuration, float alpha) {
            ctx.rendererModule.GhostTrail_SetFrame(centerPos, rot, scale, sprite, ghostTrailDuration, alpha);
        }
        #endregion

        #region API: Sound
        public void Sound_Layer_SetLimited(SoundLayerType layerType, int limitedCount) {
            ctx.soundModule.Layer_SetLimited(layerType, limitedCount);
        }

        public void Sound_Play(SoundModuleSO soundModuleSO, UniqueSignature belong, Vector2 happenPos) {
            if (soundModuleSO == null) {
                FFWLog.LogError("SoundModuleSO is null");
                return;
            }
            ctx.soundModule.Play(soundModuleSO.tm, belong, happenPos);
        }

        public void Sound_Play(int typeGroup, int typeID, UniqueSignature belong, Vector2 happenPos) {
            ctx.soundModule.Play(typeGroup, typeID, belong, happenPos);
        }

        public void Sound_Pause(SoundModuleSO soundModuleSO, UniqueSignature belong) {
            Sound_Pause(soundModuleSO.tm.typeGroup, soundModuleSO.tm.typeID, belong);
        }

        public void Sound_Pause(int typeGroup, int typeID, UniqueSignature belong) {
            ctx.soundModule.Pause(typeGroup, typeID, belong);
        }

        public void Sound_StopAllBGM(float fadeOutDuration) {
            ctx.soundModule.BGM_StopAll(fadeOutDuration);
        }

        public void Sound_PauseAllBGM() {
            ctx.soundModule.BGM_PauseAll();
        }

        public void Sound_ResumeAllBGM() {
            ctx.soundModule.BGM_ResumeAll();
        }

        public void Sound_DestroyBelong(UniqueSignature belong) {
            ctx.soundModule.DestroyBelong(belong);
        }

        public void Sound_BGM_Volume_Set(float volume) {
            ctx.soundModule.BGM_Volume_Set(volume);
        }

        public void Sound_SFX_Volume_Set(float volume) {
            ctx.soundModule.SFX_Volume_Set(volume);
        }
        #endregion

        #region API: VFX
        public void VFX_DestroyBelong(UniqueSignature belong) {
            ctx.vfxModule.UnspawnBelong(belong);
        }
        #endregion

        #region API: PP
        public void PP_ShakeScreen_Begin(Vector2 amplitude, float frequency, float duration) {
            ctx.rendererModule.ShakeScreen_Begin(amplitude, frequency, duration);
        }

        public void PP_ShakeScreen_Stop() {
            ctx.rendererModule.ShakeScreen_Stop();
        }

        public void PP_CRT_Toggle(bool isEnable) {
            ctx.rendererModule.CRT_Toggle(isEnable);
        }
        #endregion

        #region API: Rumble
        public void Rumble_Start(RumbleTM rumbleTM) {
            ctx.rumbleModule.Rumble(rumbleTM);
        }

        public void Rumble_StopAll() {
            ctx.rumbleModule.StopAll();
        }

        public void Rumble_SetIntensity(float rumbleIntensity) {
            ctx.rumbleModule.SetIntensity(rumbleIntensity);
        }

        public void Rumble_Enable(bool isEnable) {
            ctx.rumbleModule.Enable(isEnable);
        }
        #endregion

    }

}