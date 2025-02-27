using System;
using UnityEngine;

namespace GameClasses.SpriteAnimatorLib.Template {

    [CreateAssetMenu(fileName = "So_Animator_", menuName = "GameTK/SpriteAnimator")]
    public class SpriteAnimatorSO : ScriptableObject {

        public int typeGroup;
        public int typeID;

        public SpriteAnimatorStateTM[] states; // first state is the default state
        public SpriteAnimatorParameterTM[] parameters;

    }

}