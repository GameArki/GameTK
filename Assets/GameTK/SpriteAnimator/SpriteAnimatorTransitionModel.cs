using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTK.Library_SpriteAnimator {

    [Serializable]
    public class SpriteAnimatorTransitionModel {

        public int toStateID;

        public bool isAutoExit;

        public List<SpriteAnimatorParameterConditionModel> conditions;

        public SpriteAnimatorTransitionModel() {
            conditions = new List<SpriteAnimatorParameterConditionModel>();
        }

        public void FromTM(SpriteAnimatorTransitionTM tm) {
            toStateID = tm.toStateID;
            isAutoExit = tm.isAutoExit;
            conditions.Clear();
            foreach (SpriteAnimatorParameterConditionTM conditionTM in tm.conditions) {
                SpriteAnimatorParameterConditionModel conditionModel = new SpriteAnimatorParameterConditionModel();
                conditionModel.FromTM(conditionTM);
                conditions.Add(conditionModel);
            }
        }

    }

}