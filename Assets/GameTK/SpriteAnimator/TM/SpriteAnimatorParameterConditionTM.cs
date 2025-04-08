using System;

namespace GameTK.Library_SpriteAnimator {

    [Serializable]
    public struct SpriteAnimatorParameterConditionTM {

        public int parameterID;

        public SpriteAnimatorEqualLogicType equalLogicType;
        public float conditionValue;

    }

}