using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NJM.Modules_Rumble {

    internal class RumbleModuleContext {

        public bool isEnable;

        internal RumbleModuleEntity currentLeftRumble;
        internal RumbleModuleEntity currentRightRumble;

        internal List<RumbleModuleTaskModel> allTask;

        internal RumbleModuleTaskModel[] readyTaskTemp;
        internal RumbleModuleTaskModel[] allTasktemp;

        internal Gamepad gamepad;
        internal float rumbleIntensity;

        internal RumbleModuleContext() {
            allTask = new List<RumbleModuleTaskModel>();
            currentLeftRumble = new RumbleModuleEntity();
            currentRightRumble = new RumbleModuleEntity();
            readyTaskTemp = new RumbleModuleTaskModel[20];
            allTasktemp = new RumbleModuleTaskModel[20];
        }

        public void Inject(Gamepad gamepad, float rumbleIntensity) {
            this.gamepad = gamepad;
            this.rumbleIntensity = rumbleIntensity;
        }

        internal void SetLeftRumble(RumbleModuleEntity entity) {
            currentLeftRumble = entity;
        }

        internal void SetRightRumble(RumbleModuleEntity entity) {
            currentRightRumble = entity;
        }

        internal void AddTask(RumbleModuleTaskModel model) {
            allTask.Add(model);
        }

        internal int GetAllTask(out RumbleModuleTaskModel[] modelArray) {
            int count = allTask.Count;
            if (count > allTasktemp.Length) {
                allTasktemp = new RumbleModuleTaskModel[(int)(count * 1.5f)];
            }
            allTask.CopyTo(allTasktemp, 0);
            modelArray = allTasktemp;
            return count;
        }

        internal void UpdateTask(RumbleModuleTaskModel model, int index) {
            allTask[index] = model;
        }

        internal int TakeAllReadyTask(out RumbleModuleTaskModel[] modelArray) {

            if (allTask.Count >= readyTaskTemp.Length) {
                readyTaskTemp = new RumbleModuleTaskModel[(int)(allTask.Count * 1.5f)];
            }

            int count = 0;
            for (int i = 0; i < allTask.Count; i++) {
                var model = allTask[i];
                if (model.delay <= 0) {
                    readyTaskTemp[i] = model;
                    count++;
                }
            }

            modelArray = readyTaskTemp;

            if (count == 2) {
                Debug.Log("count==2: type: " + readyTaskTemp[0].RumbleMotorType + " " + readyTaskTemp[1].RumbleMotorType);
            }
            if (count == 1) {
                Debug.Log("count==1: type: " + readyTaskTemp[0].RumbleMotorType);
            }
            return count;
        }

        internal void RemoveAllReadyTask() {
            allTask.RemoveAll(model => model.delay <= 0);
        }

        internal void Clear() {
            allTask.Clear();
        }

    }

}