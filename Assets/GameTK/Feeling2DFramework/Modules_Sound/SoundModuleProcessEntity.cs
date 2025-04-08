using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTK.Modules_Sound {

    public class SoundModuleProcessEntity {

        public float bgmVolume;
        public float sfxVolume;
        public Vector2 listenerPos;
        public float thresholdDistance;

        public Dictionary<SoundLayerType, int> setting_layerLimited;

        public SoundModuleProcessEntity() {
            bgmVolume = 1;
            sfxVolume = 1;
            listenerPos = Vector2.zero;
            thresholdDistance = 40;
            setting_layerLimited = new Dictionary<SoundLayerType, int>();
        }

    }

}