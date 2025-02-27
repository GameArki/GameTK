using System;
using System.Collections.Generic;
using UnityEngine;
using GameClasses.SpriteAnimatorLib.Template;

namespace GameClasses.SpriteAnimatorLib {

    public class SpriteAnimator {

        SpriteRenderer sr;

        Dictionary<int, SpriteAnimatorStateModel> states;
        Dictionary<string, int> stateNameToID;

        Dictionary<int, SpriteAnimatorParameterModel> parameters;
        Dictionary<string, int> parameterNameToID;

        int currentStateID;

        bool isEnable;

        public SpriteAnimator() {
            states = new Dictionary<int, SpriteAnimatorStateModel>();
            stateNameToID = new Dictionary<string, int>();

            parameters = new Dictionary<int, SpriteAnimatorParameterModel>();
            parameterNameToID = new Dictionary<string, int>();
        }

        public void Inject(SpriteRenderer sr) {
            this.sr = sr;
        }

        public void FromTM(SpriteAnimatorSO so) {

            isEnable = true;

            foreach (SpriteAnimatorStateTM stateTM in so.states) {
                SpriteAnimatorStateModel model = new SpriteAnimatorStateModel();
                model.FromTM(stateTM);
                states.Add(model.stateID, model);
                stateNameToID.Add(model.stateName, model.stateID);
            }

            foreach (SpriteAnimatorParameterTM parameterTM in so.parameters) {
                SpriteAnimatorParameterModel model = new SpriteAnimatorParameterModel();
                model.FromTM(parameterTM);
                parameters.Add(model.parameterID, model);
                parameterNameToID.Add(model.parameterName, model.parameterID);
            }

            currentStateID = so.states[0].stateID;

        }

        public void Play(string stateName) {
            if (!isEnable) {
                return;
            }
            bool has = stateNameToID.TryGetValue(stateName, out int stateID);
            if (has) {
                Play(stateID);
            } else {
                Debug.LogWarning($"State {stateName} not found");
            }
        }

        public void Play(int stateID) {
            if (!isEnable) {
                return;
            }
            bool has = states.TryGetValue(currentStateID, out SpriteAnimatorStateModel state);
            if (has) {
                currentStateID = stateID;
                state.Reset();
            } else {
                Debug.LogWarning($"State {stateID} not found");
            }
        }

        public void Parameter_Set(int parameterID, float value) {
            if (!isEnable) {
                return;
            }
            bool has = parameters.TryGetValue(parameterID, out SpriteAnimatorParameterModel parameter);
            if (has) {
                parameter.value = value;
            } else {
                Debug.LogWarning($"Parameter {parameterID} not found");
            }
        }

        public void Parameter_Set(string parameterName, float value) {
            if (!isEnable) {
                return;
            }
            bool has = parameterNameToID.TryGetValue(parameterName, out int parameterID);
            if (has) {
                Parameter_Set(parameterID, value);
            } else {
                Debug.LogWarning($"Parameter {parameterName} not found");
            }
        }

        public void Tick(float dt) {
            if (!isEnable) {
                return;
            }

            bool has = states.TryGetValue(currentStateID, out SpriteAnimatorStateModel state);
            if (!has) {
                Debug.LogWarning($"State {currentStateID} not found");
            }

            bool isLoopEnd = state.Tick(dt, out Sprite sprite);
            sr.sprite = sprite;

            if (isLoopEnd) {
                if (state.TryTransition(parameters, out int toStateID)) {
                    Play(toStateID);
                }
            }
        }

    }

}