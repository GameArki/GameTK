using System;
using GameClasses.SpriteAnimatorLib.Template;

namespace GameClasses.SpriteAnimatorLib {

    public struct SpriteAnimatorParameterConditionModel {

        public int parameterID;

        public SpriteAnimatorEqualLogicType equalLogicType;
        public float conditionValue;

        public void FromTM(SpriteAnimatorParameterConditionTM tm) {
            parameterID = tm.parameterID;
            equalLogicType = tm.equalLogicType;
            conditionValue = tm.conditionValue;
        }

    }

}