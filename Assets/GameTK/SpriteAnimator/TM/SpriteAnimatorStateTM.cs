using System;
using UnityEngine;

namespace GameTK.Library_SpriteAnimator {

    [Serializable]
    public struct SpriteAnimatorStateTM {

        public int stateID;
        public string stateName;

        public bool isLoop;

        public float fps;

        public Sprite[] sprites;
        public SpriteAnimatorTransitionTM[] transitions;

    }

}