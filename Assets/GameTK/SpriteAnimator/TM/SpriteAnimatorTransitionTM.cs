using System;

namespace GameTK.Library_SpriteAnimator {

    [Serializable]
    public struct SpriteAnimatorTransitionTM {

        public int toStateID;

        public bool isAutoExit;

        public SpriteAnimatorParameterConditionTM[] conditions;

    }

}