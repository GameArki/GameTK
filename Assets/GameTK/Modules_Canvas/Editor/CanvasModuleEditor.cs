#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GameTK.Modules_Canvas.Editor {

    public static class CanvasModuleEditor {

        [MenuItem("GameObject/UI/CanvasModule")]
        public static CanvasModule CreateFromMenu() {
            return New();
        }

        public static CanvasModule New() {

            var go = new GameObject("CanvasModule");
            var module = go.AddComponent<CanvasModule>();

            Vector2Int resolution = new Vector2Int(1280, 720);

            var canvas_superBottom = module.Canvas_Create(CanvasModuleRootLayer.SuperBottom, resolution);
            var canvas_bottom = module.Canvas_Create(CanvasModuleRootLayer.Bottom, resolution);
            var canvas_middle = module.Canvas_Create(CanvasModuleRootLayer.Middle, resolution);
            var canvas_top = module.Canvas_Create(CanvasModuleRootLayer.Top, resolution);
            var canvas_hud_top = module.Canvas_Create(CanvasModuleRootLayer.HUD_Top, resolution);
            var canvas_superTop = module.Canvas_Create(CanvasModuleRootLayer.SuperTop, resolution);

            return module;
        }

    }

}
#endif