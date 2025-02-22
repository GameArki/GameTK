using System;
using UnityEngine;

namespace NJM.Modules_Canvas {

    public class CursorEntity {

        public bool isAllowVisible;

        public bool isAutoHide;
        public float autoHideSec;
        public float autoHideTimer;

        KeyCode awakeKey;

        Vector2 lastMousePos;

        public CursorEntity() { }

        public void Tick(float dt) {
            if (isAllowVisible) {
                if (Input.GetKeyDown(awakeKey)) {
                    autoHideTimer = autoHideSec;
                    Cursor.visible = true;
                }

                Vector2 mousePos = Input.mousePosition;
                if (lastMousePos != mousePos) {
                    autoHideTimer = autoHideSec;
                }
                lastMousePos = mousePos;
            }

            autoHideTimer -= dt;
            if (autoHideTimer <= 0) {
                autoHideTimer = 0;
                Cursor.visible = false;
            }
        }

        public void SetVisible(bool isAllowVisible, bool isAutoHide, float autoHideSec, KeyCode awakeKey) {
            this.isAllowVisible = isAllowVisible;
            this.isAutoHide = isAutoHide;
            this.autoHideSec = autoHideSec;
            this.awakeKey = awakeKey;

            if (isAutoHide) {
                Cursor.visible = false;
                this.autoHideTimer = 0;
            } else {
                Cursor.visible = isAllowVisible;
            }
            if (!isAllowVisible) {
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

    }

}