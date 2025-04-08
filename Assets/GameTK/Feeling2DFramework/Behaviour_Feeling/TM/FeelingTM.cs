using System;
using System.Reflection;
using UnityEngine;
using TriInspector;
using GameFunctions;
using GameTK.Modules_Sound;

namespace GameTK {

    [Serializable]
    public struct FeelingTM {

        [Title("PP Shake")]
        public bool hasPPShake;
        [ShowIf(nameof(hasPPShake))] public float ppShakeDuration;
        [ShowIf(nameof(hasPPShake))] public float ppShakeFrequency;
        [ShowIf(nameof(hasPPShake))] public Vector2 ppShakeAmplitude;

        [Title("Camera Shake")]
        public bool hasCameraShake;
        [ShowIf(nameof(hasCameraShake))] public float cameraShakeDuration;
        [ShowIf(nameof(hasCameraShake))] public float cameraShakeFrequency;
        [ShowIf(nameof(hasCameraShake))] public Vector2 cameraShakeAmplitude;

        [Title("Camera ZoomIn")]
        public bool hasCameraZoomIn;
        [ShowIf(nameof(hasCameraZoomIn))] public GFEasingEnum cameraZoomInEasingType;
        [ShowIf(nameof(hasCameraZoomIn))] public float cameraZoomInFrameCount;
        [ShowIf(nameof(hasCameraZoomIn))] public float cameraZoomInMultiplier;
        [ShowIf(nameof(hasCameraZoomIn))] public bool isCameraZoomInAutoRestore;
        [ShowIf(nameof(isCameraZoomInAutoRestore))] public GFEasingEnum cameraZoomInAutoRestoreEasingType;
        [ShowIf(nameof(isCameraZoomInAutoRestore))] public float cameraZoomInAutoRestoreDelayFrameCount;
        [ShowIf(nameof(isCameraZoomInAutoRestore))] public float cameraZoomInAutoRestoreFrameCount;

        // - PP
        [Title("电影黑边 FadeIn")]
        public bool isPPFilmBorderFadeIn;
        [ShowIf(nameof(isPPFilmBorderFadeIn))] public float filmBorderFadeInScreenEdgePercent;
        [ShowIf(nameof(isPPFilmBorderFadeIn))] public GFEasingEnum filmBorderFadeInEasingType;
        [ShowIf(nameof(isPPFilmBorderFadeIn))] public float filmBorderFadeInDuration;

        [Title("电影黑边 FadeOut")]
        public bool isPPFilmBorderFadeOut;
        [ShowIf(nameof(isPPFilmBorderFadeOut))] public float filmBorderFadeOutScreenEdgePercent;
        [ShowIf(nameof(isPPFilmBorderFadeOut))] public GFEasingEnum filmBorderFadeOutEasingType;
        [ShowIf(nameof(isPPFilmBorderFadeOut))] public float filmBorderFadeOutDuration;

        [Title("残影")]
        public bool hasGhostTrail;
        [ShowIf(nameof(hasGhostTrail))] public float ghostTrailDuration;

        [Title("VFX")]
        public bool hasVFX;
        [ShowIf(nameof(hasVFX))] public VFXModuleSM[] vfxs;

        [Title("Sound")]
        public bool hasSound;
        [ShowIf(nameof(hasSound))] public SoundModuleTM[] sounds;
        public bool isStopAllBGM;
        [ShowIf(nameof(isStopAllBGM))] public float stopAllBGMDuration;
        public bool isStopAllBGMByLayer;
        [ShowIf(nameof(isStopAllBGMByLayer))] public SoundLayerType stopAllBGMLayer;
        [ShowIf(nameof(isStopAllBGMByLayer))] public float stopAllBGMLayerDuration;

        [Title("Rumble")]
        public bool hasRumble;
        [ShowIf(nameof(hasRumble))] public RumbleTM[] rumbles;

    }

}