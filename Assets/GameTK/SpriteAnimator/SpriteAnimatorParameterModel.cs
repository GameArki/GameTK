using System;
using GameClasses.SpriteAnimatorLib.Template;

namespace GameClasses.SpriteAnimatorLib {

    public struct SpriteAnimatorParameterModel {

        public int parameterID;
        public string parameterName;
        public float value;

        public void FromTM(SpriteAnimatorParameterTM tm) {
            parameterID = tm.parameterID;
            parameterName = tm.parameterName;
            value = tm.parameterValue;
        }

    }

}