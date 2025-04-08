using System;

namespace GameTK.Modules_Sound {

    public enum SoundLayerType {
        BGM_Core,
        BGM_Env,
        BGM_Misc,

        SFX_Core,
        SFX_Env,
        SFX_Misc,
    }

    public static class SoundLayerTypeExtension {

        public static bool IsBGM(this SoundLayerType layer) {
            return layer == SoundLayerType.BGM_Core || layer == SoundLayerType.BGM_Env || layer == SoundLayerType.BGM_Misc;
        }

    }
}