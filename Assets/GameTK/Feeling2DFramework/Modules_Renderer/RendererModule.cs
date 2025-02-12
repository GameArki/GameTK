using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using GameRenderer;
using GameFunctions;
using NJM.Modules_Renderer;

namespace NJM {

    public class RendererModule {

        RendererModuleContext ctx;

        Volume volume_global;
        GhostTrailEntity ghostTrailEntity;

        public RendererModule() {
            ctx = new RendererModuleContext();
        }

        public void Inject(Volume globalVolume) {
            volume_global = globalVolume;
            ghostTrailEntity = new GameObject("GhostTrail").AddComponent<GhostTrailEntity>();
            ghostTrailEntity.Ctor();
        }

        public void Init() {

        }

        public void StopAllEffects() {
            ShakeScreen_Stop();
            FilmBorder_Stop();
        }

        public void Tick(float dt) {
            ShakeScreen_Tick(dt);
            FilmBorder_Tick(dt);
        }

        #region PP: CRT
        public void CRT_Toggle(bool isEnable) {
            {
                bool has = volume_global.profile.TryGet<PPScanLineVolume>(out var scanLineVolume);
                if (has) {
                    scanLineVolume.active = isEnable;
                    scanLineVolume.isEnable.value = isEnable;
                }
            }
            {
                bool has = volume_global.profile.TryGet<ChromaticAberration>(out var caVolume);
                if (has) {
                    caVolume.active = isEnable;
                }
            }
            {
                bool has = volume_global.profile.TryGet<PaniniProjection>(out var paniniVolume);
                if (has) {
                    paniniVolume.active = isEnable;
                }
            }
        }
        #endregion

        #region PP: ShakeScreen
        public void ShakeScreen_Begin(Vector2 amplitude, float frequency, float duration) {
            bool has = volume_global.profile.TryGet<PPShakeScreenVolume>(out var shakeVolume);
            if (has) {
                shakeVolume.active = true;
                shakeVolume.isEnable.value = true;
                shakeVolume.amplitude.value = amplitude;
                shakeVolume.frequency.value = frequency;
                shakeVolume.duration.value = duration;
                shakeVolume.timer.value = duration;
            }
        }

        public void ShakeScreen_Stop() {
            bool has = volume_global.profile.TryGet<PPShakeScreenVolume>(out var shakeVolume);
            if (has) {
                shakeVolume.isEnable.value = false;
            }
        }

        void ShakeScreen_Tick(float dt) {
            bool has = volume_global.profile.TryGet<PPShakeScreenVolume>(out var volume);
            if (!has) {
                return;
            }
            if (volume.timer.value > 0) {
                volume.timer.value -= dt;
            } else {
                volume.timer.value = 0;
                volume.isEnable.value = false;
            }
        }
        #endregion

        #region PP: FilmBorder
        public void FilmBorder_FadeIn(float screenEdgePercent, GFEasingEnum easingEnum, float duration) {
            FilmBorder_Begin(true, easingEnum, 0, screenEdgePercent, duration);
        }

        public void FilmBorder_FadeOut(float screenEdgePercent, GFEasingEnum easingEnum, float duration) {
            FilmBorder_Begin(false, easingEnum, screenEdgePercent, 0, duration);
        }

        public void FilmBorder_Begin(bool isEnable, GFEasingEnum easingEnum, float startValue, float endValue, float duration) {
            bool has = volume_global.profile.TryGet<PPFilmBorderVolume>(out var filmBorderVolume);
            if (!has) {
                Debug.LogError("FilmBorder_Begin: filmBorderVolume not found");
                return;
            }
            if (isEnable) {
                filmBorderVolume.active = isEnable;
                filmBorderVolume.isEnable.value = isEnable;
            }

            var model = ctx.pp_filmBorderModel;
            model.easingType = easingEnum;
            model.duration = duration;
            model.timer = duration;
            model.startValue = startValue;
            model.endValue = endValue;
            model.isToEnable = isEnable;

            filmBorderVolume.borderOffset.value = new Vector2(model.startValue, model.startValue);
        }

        void FilmBorder_Tick(float dt) {
            bool has = volume_global.profile.TryGet<PPFilmBorderVolume>(out var filmBorderVolume);
            if (!has) {
                return;
            }

            var model = ctx.pp_filmBorderModel;
            model.timer -= dt;
            float value = GFEasing.Ease1D(model.easingType, model.duration - model.timer, model.duration, model.startValue, model.endValue);
            filmBorderVolume.borderOffset.value = new Vector2(value, value);

            if (model.timer <= 0 && !model.isToEnable) {
                filmBorderVolume.isEnable.value = false;
            }

        }

        void FilmBorder_Stop() {
            bool has = volume_global.profile.TryGet<PPFilmBorderVolume>(out var filmBorderVolume);
            if (has) {
                filmBorderVolume.isEnable.value = false;
            }
        }
        #endregion

        #region GhostTrail
        public void GhostTrail_SetFrame(Vector2 pos, Quaternion rot, Vector3 scale, Sprite sprite, float maintainSec, float alpha) {
            ghostTrailEntity.SetFrame(pos, rot, scale, sprite, maintainSec, alpha);
        }
        #endregion

    }

}