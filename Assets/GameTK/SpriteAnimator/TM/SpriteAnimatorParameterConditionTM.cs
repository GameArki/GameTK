using System;

namespace GameClasses.SpriteAnimatorLib.Template {

    [Serializable]
    public struct SpriteAnimatorParameterConditionTM {

        public int parameterID;

        public SpriteAnimatorEqualLogicType equalLogicType;
        public float conditionValue;

    }

}