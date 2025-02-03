using System;
using UnityEngine;
using NJM.Modules_Sound;

namespace NJM {

    [Serializable]
    public struct SoundModuleTM {

        public int typeID;
        public SoundLayerType layer;
        public AudioClip clip;
        public float volumePercent;
        public bool isEffectByDistance;
        public bool isLoop; // destroy when belong destroy
        public bool isFollowBelong;

    }

}