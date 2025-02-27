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
            bool has = stateNameToID.TryGetValue(stateName, out int stateID);
            if (has) {
                Play(stateID);
            } else {
                throw new Exception($"State {stateName} not found");
            }
        }

        public void Play(int stateID) {
            bool has = states.TryGetValue(currentStateID, out SpriteAnimatorStateModel state);
            if (has) {
                currentStateID = stateID;
                state.Reset();
            } else {
                throw new Exception($"State {stateID} not found");
            }
        }

        public void Parameter_Set(int parameterID, float value) {
            bool has = parameters.TryGetValue(parameterID, out SpriteAnimatorParameterModel parameter);
            if (has) {
                parameter.value = value;
            } else {
                throw new Exception($"Parameter {parameterID} not found");
            }
        }

        public void Parameter_Set(string parameterName, float value) {
            bool has = parameterNameToID.TryGetValue(parameterName, out int parameterID);
            if (has) {
                Parameter_Set(parameterID, value);
            } else {
                throw new Exception($"Parameter {parameterName} not found");
            }
        }

        public void Tick(float dt) {
            bool has = states.TryGetValue(currentStateID, out SpriteAnimatorStateModel state);
            if (!has) {
                throw new Exception($"State {currentStateID} not found");
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