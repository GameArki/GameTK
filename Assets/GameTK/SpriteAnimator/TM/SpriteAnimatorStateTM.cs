using System;
using UnityEngine;

namespace GameClasses.SpriteAnimatorLib.Template {

    [Serializable]
    public struct SpriteAnimatorStateTM {

        public int stateID;
        public string stateName;

        public bool isLoop;

        public float speed;

        public Sprite[] sprites;
        public SpriteAnimatorTransitionTM[] transitions;

    }

}