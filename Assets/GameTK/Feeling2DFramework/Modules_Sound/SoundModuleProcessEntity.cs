using System;
using UnityEngine;

namespace NJM.Modules_Sound {

    public class SoundModuleProcessEntity {

        public float bgmVolume;
        public float sfxVolume;
        public Vector2 listenerPos;
        public float thresholdDistance;

        public SoundModuleProcessEntity() {
            bgmVolume = 1;
            sfxVolume = 1;
            listenerPos = Vector2.zero;
            thresholdDistance = 40;
        }

    }

}