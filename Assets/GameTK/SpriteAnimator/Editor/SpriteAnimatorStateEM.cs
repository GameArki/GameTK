#if UNITY_EDITOR
using System;
using UnityEngine;
using GameClasses.SpriteAnimatorLib.Template;

namespace GameClasses.SpriteAnimatorLib.Editor {

    [ExecuteInEditMode]
    public class SpriteAnimatorStateEM : MonoBehaviour {

        public SpriteAnimatorStateTM tm;

        void Update() {
            string n = tm.stateName + "_" + tm.stateID;
            if (name != n) {
                name = n;
            }
        }

    }

}
#endif