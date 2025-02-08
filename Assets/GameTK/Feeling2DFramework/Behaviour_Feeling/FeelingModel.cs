using System;
using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace NJM {

    [Serializable]
    public struct FeelingModel {

        // - PP: Effect Shake
        public bool hasPPShake;
        public float ppShakeDuration;
        public float ppShakeFrequency;
        public Vector2 ppShakeAmplitude;

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
        public float filmBorderFadeInScreenEdgePercent;
        public GFEasingEnum filmBorderFadeInEasingType;
        public float filmBorderFadeInDuration;

        public bool isPPFilmBorderFadeOut;
        public float filmBorderFadeOutScreenEdgePercent;
        public GFEasingEnum filmBorderFadeOutEasingType;
        public float filmBorderFadeOutDuration;

        // - GhostTrail
        public bool hasGhostTrail;
        public float ghostTrailDuration;

        // - VFX
        public bool hasVFX;
        public VFXModuleSM[] vfxs;

        // - SFX
        public bool hasSound;
        public SoundModuleTM[] sounds;

        // - Rumble
        public bool hasRumble;
        public RumbleTM[] rumbles;

        public void FromTM(in Template.FeelingTM tm) {

            hasPPShake = tm.hasPPShake;
            ppShakeDuration = tm.ppShakeDuration;
            ppShakeFrequency = tm.ppShakeFrequency;
            ppShakeAmplitude = tm.ppShakeAmplitude;

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
            filmBorderFadeInScreenEdgePercent = tm.filmBorderFadeInScreenEdgePercent;
            filmBorderFadeInEasingType = tm.filmBorderFadeInEasingType;
            filmBorderFadeInDuration = tm.filmBorderFadeInDuration;

            isPPFilmBorderFadeOut = tm.isPPFilmBorderFadeOut;
            filmBorderFadeOutScreenEdgePercent = tm.filmBorderFadeOutScreenEdgePercent;
            filmBorderFadeOutEasingType = tm.filmBorderFadeOutEasingType;
            filmBorderFadeOutDuration = tm.filmBorderFadeOutDuration;

            hasGhostTrail = tm.hasGhostTrail;
            ghostTrailDuration = tm.ghostTrailDuration;

            hasVFX = tm.hasVFX;
            vfxs = tm.vfxs;

            hasSound = tm.hasSound;
            sounds = tm.sounds;

            hasRumble = tm.hasRumble;
            rumbles = tm.rumbles;

        }

    }

}