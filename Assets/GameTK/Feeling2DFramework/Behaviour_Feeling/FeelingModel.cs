using System;
using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace NJM {

    [Serializable]
    public struct FeelingModel {

        // - Suffer Mesh Shake
        public bool hasSufferMeshShake;
        public float sufferMeshShakeDuration;
        public Vector2 sufferMeshShakeStrength;

        // - Camera: Effect Shake
        public bool hasCameraShake;
        public float cameraShakeDuration;
        public float cameraShakeFrequency;
        public Vector2 cameraShakeAmplitude;

        // - Camera: Effect ZoomIn
        public bool hasCameraZoomIn;
        public GFEasingEnum cameraZoomInEasingType;
        public float cameraZoomInDurationFrameCount;
        public float cameraZoomInMultiplier;
        public bool isCameraZoomInAutoRestore;
        public GFEasingEnum cameraZoomInAutoRestoreEasingType;
        public float cameraZoomInAutoRestoreDelayFrameCount;
        public float cameraZoomInAutoRestoreDurationFrameCount;

        // - PP
        public bool isPPFilmBorderFadeIn;
        public bool isPPFilmBorderFadeOut;

        // - GhostTrail
        public bool hasGhostTrail;
        public float ghostTrailDuration;

        // - VFX
        public bool hasVFX;
        public VFXModuleSM[] vfxs;

        // - SFX
        public bool hasSFX;
        public SoundModuleSO[] sfxs;

        // - Rumble
        public bool hasRumble;
        public float leftRumbleDuration;
        public float leftRumbleStrength;
        public float rightRumbleDuration;
        public float rightRumbleStrength;

        public void FromTM(in Template.FeelingTM tm) {

            hasSufferMeshShake = tm.hasSufferMeshShake;
            sufferMeshShakeDuration = tm.sufferMeshShakeDuration;
            sufferMeshShakeStrength = tm.sufferMeshShakeStrength;

            hasCameraShake = tm.hasCameraShake;
            cameraShakeDuration = tm.cameraShakeDuration;
            cameraShakeFrequency = tm.cameraShakeFrequency;
            cameraShakeAmplitude = tm.cameraShakeAmplitude;

            hasCameraZoomIn = tm.hasCameraZoomIn;
            cameraZoomInEasingType = tm.cameraZoomInEasingType;
            cameraZoomInDurationFrameCount = tm.cameraZoomInFrameCount;
            cameraZoomInMultiplier = tm.cameraZoomInMultiplier;
            isCameraZoomInAutoRestore = tm.isCameraZoomInAutoRestore;
            cameraZoomInAutoRestoreEasingType = tm.cameraZoomInAutoRestoreEasingType;
            cameraZoomInAutoRestoreDelayFrameCount = tm.cameraZoomInAutoRestoreDelayFrameCount;
            cameraZoomInAutoRestoreDurationFrameCount = tm.cameraZoomInAutoRestoreFrameCount;

            isPPFilmBorderFadeIn = tm.isPPFilmBorderFadeIn;
            isPPFilmBorderFadeOut = tm.isPPFilmBorderFadeOut;

            hasGhostTrail = tm.hasGhostTrail;
            ghostTrailDuration = tm.ghostTrailDuration;

            hasVFX = tm.hasVFX;
            vfxs = tm.vfxs;

            hasSFX = tm.hasSFX;
            sfxs = tm.sfxs;

            hasRumble = tm.hasRumble;
            leftRumbleDuration = tm.leftRumbleDuration;
            leftRumbleStrength = tm.leftRumbleStrength;
            rightRumbleDuration = tm.rightRumbleDuration;
            rightRumbleStrength = tm.rightRumbleStrength;

        }

    }

}