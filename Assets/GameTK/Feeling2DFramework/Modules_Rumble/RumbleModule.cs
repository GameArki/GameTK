using System.Collections;
using System.Collections.Generic;
using GameFunctions;
using UnityEngine;
using UnityEngine.InputSystem;
using NJM.Modules_Rumble;

namespace NJM {

    public class RumbleModule {

        RumbleModuleContext ctx;

        public RumbleModule() {
            ctx = new RumbleModuleContext();
        }

        public void Inject(Gamepad gamepad, float rumbleIntensity) {
            ctx.Inject(gamepad, rumbleIntensity);
        }

        public void Init() {
            Clear();
        }

        public void Tick(float dt) {
            var gamepad = ctx.gamepad;
            TickRumble(ctx, dt, out float leftFreq, out float rightFreq);
            if (gamepad != null) {
                // Switch 临时处理, 电机频率过高
#if UNITY_SWITCH
                leftFreq *= 0.5f;
                rightFreq *= 0.5f;
#endif
                // 暂停处理
                if (dt == 0) {
                    leftFreq = 0;
                    rightFreq = 0;
                }

                gamepad.SetMotorSpeeds(leftFreq, rightFreq);
            }
        }

        void Clear() {
            ctx.Clear();
        }

        public void StopAll() {
            var gamepad = ctx.gamepad;
            if (gamepad != null) {
                gamepad.SetMotorSpeeds(0, 0);
            }
            Clear();
        }

        public void SetIntensity(float intensity) {
            ctx.rumbleIntensity = intensity;
        }

        public void Rumble(RumbleTM tm) {
            CreateRumbleModuleTaskModel(ctx, tm.motorType, tm.delay, tm.startFreq, tm.endFreq, tm.duration, tm.easingType);
        }

        void CreateRumbleModuleTaskModel(RumbleModuleContext ctx, RumbleMotorType RumbleMotorType, float delay, float startFreq, float endFreq, float duration, GFEasingEnum easingType) {
            if (RumbleMotorType == RumbleMotorType.None) {
                Debug.LogError("RumbleCore " + "CreateRumbleModuleTaskModel " + "RumbleMotorType is not valid");
                return;
            }
            var model = RumbleModuleFactory.CreateRumbleModuleTaskModel(RumbleMotorType, delay, startFreq, endFreq, duration, easingType);
            ctx.AddTask(model);
        }

        void UpdateRumbleFromModel(RumbleModuleContext ctx, RumbleModuleTaskModel model) {
            if (model.RumbleMotorType == RumbleMotorType.Left) {
                var entity = ctx.currentLeftRumble;
                entity.UpdateRumbleFromModel(model);
            } else if (model.RumbleMotorType == RumbleMotorType.Right) {
                var entity = ctx.currentRightRumble;
                entity.UpdateRumbleFromModel(model);
            } else if (model.RumbleMotorType == RumbleMotorType.Both) {
                var leftEntity = ctx.currentLeftRumble;
                leftEntity.UpdateRumbleFromModel(model);
                var rightEntity = ctx.currentRightRumble;
                rightEntity.UpdateRumbleFromModel(model);
            }
        }

        void TickRumble(RumbleModuleContext ctx, float dt, out float leftFreq, out float rightFreq) {

            // Apply Task
            ApplyTaskTime(ctx, dt);

            // Apply Rumble
            var leftRumble = ctx.currentLeftRumble;
            var rightRumble = ctx.currentRightRumble;
            ApplyRumble(leftRumble, dt);
            ApplyRumble(rightRumble, dt);

            leftFreq = leftRumble.currentFreq;
            rightFreq = rightRumble.currentFreq;

        }

        void ApplyTaskTime(RumbleModuleContext ctx, float dt) {
            var len = ctx.GetAllTask(out var modelArr);
            if (len == 0) {
                return;
            }
            for (var i = 0; i < len; i++) {
                var model = modelArr[i];
                model.delay -= dt;
                if (model.delay <= 0) {
                    UpdateRumbleFromModel(ctx, model);
                }
                ctx.UpdateTask(model, i);
            }
            ctx.RemoveAllReadyTask();
        }

        void ApplyRumble(RumbleModuleEntity rumble, float dt) {
            if (rumble.isFinished) {
                return;
            }
            rumble.currentTime += dt;
            if (rumble.currentTime >= rumble.duration) {
                rumble.currentFreq = 0;
                rumble.isFinished = true;
                return;
            }
            rumble.currentFreq = GFEasing.Ease1D(rumble.easingType, rumble.currentTime, rumble.duration, rumble.startFreq, rumble.endFreq);
        }

    }

}