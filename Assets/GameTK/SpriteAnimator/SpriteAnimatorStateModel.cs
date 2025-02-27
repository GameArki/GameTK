using System;
using System.Collections.Generic;
using UnityEngine;
using GameClasses.SpriteAnimatorLib.Template;

namespace GameClasses.SpriteAnimatorLib {

    public class SpriteAnimatorStateModel {

        public int stateID;
        public string stateName;
        public float fps;
        public Sprite[] sprites;
        public bool isLoop;
        bool isEnd;

        public List<SpriteAnimatorTransitionModel> transitions;

        // == Temp ==
        public float time;
        public int index;

        public SpriteAnimatorStateModel() {
            transitions = new List<SpriteAnimatorTransitionModel>();
        }

        public void FromTM(SpriteAnimatorStateTM tm) {
            stateID = tm.stateID;
            stateName = tm.stateName;
            fps = tm.fps;
            isLoop = tm.isLoop;
            sprites = tm.sprites;
            transitions.Clear();
            foreach (SpriteAnimatorTransitionTM transitionTM in tm.transitions) {
                SpriteAnimatorTransitionModel transitionModel = new SpriteAnimatorTransitionModel();
                transitionModel.FromTM(transitionTM);
                transitions.Add(transitionModel);
            }
        }

        public void Reset() {
            time = 0;
            index = 0;
            isEnd = false;
        }

        public void Tick(float dt, out Sprite sprite) {

            time += dt;

            if (time >= fps) {
                time -= fps;
                index++;
            }

            if (index >= sprites.Length) {
                if (isLoop) {
                    time = 0;
                    index = 0;
                } else {
                    index = sprites.Length - 1;
                    isEnd = true;
                }
            }

            sprite = sprites[index];
        }

        public bool TryTransition(IReadOnlyDictionary<int, SpriteAnimatorParameterModel> parameters, out int toStateID) {
            foreach (SpriteAnimatorTransitionModel transition in transitions) {
                if (transition.isAutoExit) {
                    if (isEnd) {
                        toStateID = transition.toStateID;
                        return true;
                    }
                }
                foreach (SpriteAnimatorParameterConditionModel condition in transition.conditions) {
                    var logicType = condition.equalLogicType;
                    var parameterID = condition.parameterID;
                    var conditionValue = condition.conditionValue;
                    if (parameters.TryGetValue(parameterID, out SpriteAnimatorParameterModel parameter)) {
                        if (logicType.HasFlag(SpriteAnimatorEqualLogicType.Equal)) {
                            if (parameter.value == conditionValue) {
                                toStateID = transition.toStateID;
                                return true;
                            }
                        }

                        if (logicType.HasFlag(SpriteAnimatorEqualLogicType.GreaterThanCondition)) {
                            if (parameter.value > conditionValue) {
                                toStateID = transition.toStateID;
                                return true;
                            }
                        }

                        if (logicType.HasFlag(SpriteAnimatorEqualLogicType.LessThanCondition)) {
                            if (parameter.value < conditionValue) {
                                toStateID = transition.toStateID;
                                return true;
                            }
                        }
                    }
                }
            }

            toStateID = -1;
            return false;

        }

    }

}