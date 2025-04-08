using System;
using System.Collections;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;

namespace GameTK {

    [Serializable]
    public struct RumbleModuleTaskModel {

        public RumbleMotorType RumbleMotorType;
        public float delay;
        public float startFreq;
        public float endFreq;
        public float duration;
        public GFEasingEnum easingType;

    }

}