using UnityEngine;
using GameClasses.Camera2DLib;

namespace NJM.Modules_Camera {

    public class CameraContext {

        public Camera mainCamera;
        public Camera[] otherCameras;
        public Camera2DCore pfCore;
        public int cameraHandleID;

        public float orthographicBaseSize;
        public float aspect;

        public CameraContext() {
            pfCore = new Camera2DCore();
        }

        public void Inject(Camera mainCamera, params Camera[] otherCameras) {
            this.mainCamera = mainCamera;
            this.otherCameras = otherCameras;
        }

    }
}