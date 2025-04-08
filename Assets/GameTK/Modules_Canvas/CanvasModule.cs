using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameTK.Modules_Canvas;

namespace GameTK {

    public class CanvasModule : MonoBehaviour {

        CanvasModuleContext ctx;

        public void Ctor() {
            ctx = new CanvasModuleContext();

            CanvasEntity[] canvases = GetComponentsInChildren<CanvasEntity>();
            foreach (var canvas in canvases) {
                ctx.canvases.Add(canvas.layer, canvas);
            }
        }

        #region Create
        public void CreateOrUpdate(CanvasModuleRootLayer layer, Vector2Int resolution) {
            if (!ctx.canvases.TryGetValue(layer, out var entity)) {
                entity = Canvas_Create(layer, resolution);
                ctx.canvases.Add(layer, entity);
            }
            entity.SetResolution(resolution);
        }

        internal CanvasEntity Canvas_Create(CanvasModuleRootLayer layer, Vector2Int resolution) {
            var go = new GameObject($"Canvas_{layer.ToString()}");
            go.layer = LayerMask.NameToLayer("UI");

            var entity = go.AddComponent<CanvasEntity>();
            entity.layer = layer;

            var canvas = go.AddComponent<Canvas>();
            canvas.transform.SetParent(transform, false);
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.pixelPerfect = true;
            canvas.sortingOrder = 1000 + (int)layer;
            entity.canvas = canvas;

            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = resolution;
            scaler.matchWidthOrHeight = 1f;
            go.AddComponent<GraphicRaycaster>();
            entity.canvasScaler = scaler;

            var root = new GameObject("Root").AddComponent<RectTransform>();
            root.gameObject.layer = LayerMask.NameToLayer("UI");
            root.SetParent(go.transform, false);
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.pivot = new Vector2(0.5f, 0.5f);
            entity.root = root;

            var fit = root.gameObject.AddComponent<AspectRatioFitter>();
            fit.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            fit.aspectRatio = (float)resolution.x / resolution.y;
            entity.aspectRatioFitter = fit;

            return entity;
        }
        #endregion

        #region Tick
        public void Tick(float dt) {
            var evs = EventSystem.current;
            if (evs.currentSelectedGameObject == null) {
                evs.SetSelectedGameObject(ctx.lastSelectedGameObject);
            } else {
                ctx.lastSelectedGameObject = evs.currentSelectedGameObject;
            }

            ctx.cursorEntity.Tick(dt);
        }
        #endregion

        #region API
        public Transform GetRoot(CanvasModuleRootLayer layer) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                return entity.root;
            }
            return null;
        }

        public void Cursor_SetVisible(bool isVisible, bool isAutoHide, float autoHideSec, KeyCode awakeKey) {
            var cursor = ctx.cursorEntity;
            cursor.SetVisible(isVisible, isAutoHide, autoHideSec, awakeKey);
        }

        public void Canvas_SetResolution(CanvasModuleRootLayer layer, Vector2Int resolution) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetResolution(resolution);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetFitRatio(CanvasModuleRootLayer layer, AspectRatioFitter.AspectMode mode, float ratio) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetFitRatio(mode, ratio);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetFitRatio_DontChangeMode(CanvasModuleRootLayer layer, float ratio) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetFitRatio_DontChangeMode(ratio);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetFitRatioAll(AspectRatioFitter.AspectMode mode, float ratio) {
            foreach (var entity in ctx.canvases.Values) {
                entity.SetFitRatio(mode, ratio);
            }
        }

        public void Canvas_SetSortOrder(CanvasModuleRootLayer layer, int order) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetSortOrder(order);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetRenderMode(CanvasModuleRootLayer layer, RenderMode mode) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetRenderMode(mode);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetPixelPerfect(CanvasModuleRootLayer layer, bool isPixelPerfect) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetPixelPerfect(isPixelPerfect);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetCamera(CanvasModuleRootLayer layer, Camera camera) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetCamera(camera);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetCameraAll(CanvasModuleRootLayer layer, Camera camera) {
            foreach (var entity in ctx.canvases.Values) {
                entity.SetCamera(camera);
            }
        }

        public void Canvas_SetMatchWidthOrHeight(CanvasModuleRootLayer layer, float matchHeightPercent) {
            if (ctx.canvases.TryGetValue(layer, out var entity)) {
                entity.SetMatchWidthOrHeight(matchHeightPercent);
            } else {
                Debug.LogError("Canvas not found for layer: " + layer);
            }
        }

        public void Canvas_SetMatchWidthOrHeightAll(float matchHeightPercent) {
            foreach (var entity in ctx.canvases.Values) {
                entity.SetMatchWidthOrHeight(matchHeightPercent);
            }
        }
        #endregion

    }

}