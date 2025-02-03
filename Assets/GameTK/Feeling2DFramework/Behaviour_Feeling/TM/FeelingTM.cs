using System;
using System.Reflection;
using UnityEngine;
using TriInspector;
using GameFunctions;

namespace NJM.Template {

    [Serializable]
    public struct FeelingTM {

        // - Suffer Mesh Shake
        public bool hasSufferMeshShake;
        [ShowIf(nameof(hasSufferMeshShake))] public float sufferMeshShakeDuration;
        [ShowIf(nameof(hasSufferMeshShake))] public Vector2 sufferMeshShakeStrength;

        // - Camera
        public bool hasCameraShake;
        [ShowIf(nameof(hasCameraShake))] public float cameraShakeDuration;
        [ShowIf(nameof(hasCameraShake))] public float cameraShakeFrequency;
        [ShowIf(nameof(hasCameraShake))] public Vector2 cameraShakeAmplitude;

        // - Camera ZoomIn
        public bool hasCameraZoomIn;
        [ShowIf(nameof(hasCameraZoomIn))] public GFEasingEnum cameraZoomInEasingType;
        [ShowIf(nameof(hasCameraZoomIn))] public float cameraZoomInFrameCount;
        [ShowIf(nameof(hasCameraZoomIn))] public float cameraZoomInMultiplier;
        [ShowIf(nameof(hasCameraZoomIn))] public bool isCameraZoomInAutoRestore;
        [ShowIf(nameof(isCameraZoomInAutoRestore))] public GFEasingEnum cameraZoomInAutoRestoreEasingType;
        [ShowIf(nameof(isCameraZoomInAutoRestore))] public float cameraZoomInAutoRestoreDelayFrameCount;
        [ShowIf(nameof(isCameraZoomInAutoRestore))] public float cameraZoomInAutoRestoreFrameCount;

        // - PP
        public bool isPPFilmBorderFadeIn;
        public bool isPPFilmBorderFadeOut;

        // - GhostTrail
        public bool hasGhostTrail;
        [ShowIf(nameof(hasGhostTrail))] public float ghostTrailDuration;

        // - VFX
        public bool hasVFX;
        [ShowIf(nameof(hasVFX))] public VFXModuleSM[] vfxs;

        // - SFX
        public bool hasSFX;
        [ShowIf(nameof(hasSFX))] public SoundModuleSO[] sfxs;

        // - Rumble
        public bool hasRumble;
        [ShowIf(nameof(hasRumble))] public float leftRumbleDuration;
        [ShowIf(nameof(hasRumble))] public float leftRumbleStrength;
        [ShowIf(nameof(hasRumble))] public float rightRumbleDuration;
        [ShowIf(nameof(hasRumble))] public float rightRumbleStrength;

    }

}