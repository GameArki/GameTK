using System;
using UnityEngine;
using NJM.Modules_Sound;

namespace NJM {

    [CreateAssetMenu(fileName = "So_Sound_", menuName = "NJM/SoundSO")]
    public class SoundModuleSO : ScriptableObject {

        public int typeID;
        public SoundLayerType layer;
        public AudioClip clip;
        public float volumePercent = 1;
        public bool isEffectByDistance;
        public bool isLoop; // destroy when belong destroy
        public bool isFollowBelong;

    }

}