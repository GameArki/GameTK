using UnityEngine;
using GameFunctions;
using NJM.Modules_Camera;

namespace NJM {

    public class CameraModule {

        CameraContext ctx;

        public CameraModule() {
            ctx = new CameraContext();
        }

        public void Inject(Camera mainCamera, params Camera[] otherCameras) {
            ctx.Inject(mainCamera, otherCameras);
        }

        public void Init() {
            var cam = ctx.mainCamera;
            ctx.orthographicBaseSize = cam.orthographicSize;
            ctx.cameraHandleID = ctx.pfCore.Init(cam.transform.position, cam.orthographicSize, cam.aspect);
        }

        public void StopAllEffects() {
            ctx.pfCore.Effect_Shake_Stop(ctx.cameraHandleID);
            ctx.pfCore.Effect_ZoomIn_Stop(ctx.cameraHandleID);
        }

        public Vector3 WorldToScreenPoint(Vector2 worldPos) {
            Vector3 res = ctx.mainCamera.WorldToScreenPoint(worldPos);
            return res;
        }

        public void Orthographic_SetBaseSize(float orthographicSize, float aspect) {
            ctx.orthographicBaseSize = orthographicSize;
            ctx.aspect = aspect;
            ctx.pfCore.OrthographicSize_Set(ctx.cameraHandleID, orthographicSize, aspect);
        }

        public void Orthographic_SetMultiplier(float multiplier) {
            ctx.pfCore.OrthographicSize_Set(ctx.cameraHandleID, ctx.orthographicBaseSize * multiplier, ctx.aspect);
        }

        public void Tick(Vector2 targetPos, float dt) {

            Follow_Update(targetPos);

            var res = ctx.pfCore.Tick(dt);

            var mainCam = ctx.mainCamera;
            mainCam.aspect = res.aspect;
            mainCam.orthographicSize = res.orthographicSize;

            if (ctx.otherCameras != null) {
                foreach (var other in ctx.otherCameras) {
                    other.aspect = res.aspect;
                    other.orthographicSize = res.orthographicSize;
                }
            }

            mainCam.transform.position = new Vector3(res.pos.x, res.pos.y, mainCam.transform.position.z);
        }

        public void FastForward(float dt) {
            ctx.pfCore.Tick(dt);
        }

        void Follow_Update(Vector2 targetPos) {
            ctx.pfCore.Follow_Update(ctx.cameraHandleID, targetPos);
        }

        public void Confiner_Enable(bool isEnable) {
            ctx.pfCore.Confine_Enable(ctx.cameraHandleID, isEnable);
        }

        public void Confiner_Set(Vector2 leftDown, Vector2 rightUp) {
            ctx.pfCore.Confine_Set(ctx.cameraHandleID, leftDown, rightUp);
        }

        public void Follow_Enable(bool isEnable) {
            ctx.pfCore.Follow_Enable(ctx.cameraHandleID, isEnable);
        }

        public void Follow_Init(Vector2 targetPos, Vector3 offset, float xDamping, float yDamping) {
            ctx.pfCore.Follow_Set(ctx.cameraHandleID, targetPos, offset, xDamping, yDamping);
        }

        public void Follow_ChangeTarget(Vector2 targetPos) {
            ctx.pfCore.Follow_Update(ctx.cameraHandleID, targetPos);
        }

        public void Effect_Shake(Vector2 amplitude, float frequency, float duration) {
            ctx.pfCore.Effect_Shake_Begin(ctx.cameraHandleID, amplitude, frequency, duration);
        }

        public void Follow_DeadZone_Set(Vector2 size) {
            ctx.pfCore.Follow_DeadZone_Set(ctx.cameraHandleID, size);
        }

        public void Effect_ZoomIn(GFEasingEnum easingType, float multiplier, float duration, bool isAutoRestore, GFEasingEnum restoreEasingType, float restoreDuration, float restoreDelay) {
            if (isAutoRestore) {
                ctx.pfCore.Effect_ZoomIn_BeginAndAutoRestore(ctx.cameraHandleID, easingType, multiplier, duration, restoreEasingType, restoreDuration, restoreDelay);
            } else {
                ctx.pfCore.Effect_ZoomIn_Begin(ctx.cameraHandleID, easingType, multiplier, duration);
            }
        }

    }

}