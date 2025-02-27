using System;

namespace GameClasses.SpriteAnimatorLib.Template {

    [Serializable]
    public struct SpriteAnimatorTransitionTM {

        public int toStateID;

        public bool isAutoExit;

        public SpriteAnimatorParameterConditionTM[] conditions;

    }

}