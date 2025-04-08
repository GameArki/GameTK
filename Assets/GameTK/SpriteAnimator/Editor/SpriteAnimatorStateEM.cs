#if UNITY_EDITOR
using System;
using UnityEngine;

namespace GameTK.Library_SpriteAnimator.Editor {

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