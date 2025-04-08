using System;
using UnityEngine;
using GameTK.Modules_Sound;

namespace GameTK {

    [Serializable]
    public struct SoundModuleTM {

        public int typeGroup;
        public int typeID;
        public SoundLayerType layer;
        public AudioClip clip;
        public float volumePercent;
        public bool isEffectByDistance;
        public bool isLoop; // destroy when belong destroy
        public bool isFollowBelong;

    }

}