#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using TriInspector;
using GameClasses.SpriteAnimatorLib.Template;

namespace GameClasses.SpriteAnimatorLib.Editor {

    public class SpriteAnimatorEM : MonoBehaviour {

        public SpriteAnimatorSO so;

        public SpriteAnimatorParameterTM[] parameterTMs;

        [Button]
        public void Bake() {
            SpriteAnimatorStateEM[] stateEMs = GetComponentsInChildren<SpriteAnimatorStateEM>();
            SpriteAnimatorStateTM[] stateTMs = new SpriteAnimatorStateTM[stateEMs.Length];
            for (int i = 0; i < stateEMs.Length; i++) {
                stateTMs[i] = stateEMs[i].tm;
            }
            so.states = stateTMs;
            so.parameters = parameterTMs;
            EditorUtility.SetDirty(so);
        }

    }

}
#endif