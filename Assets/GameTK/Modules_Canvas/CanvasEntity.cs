using System;
using UnityEngine;
using UnityEngine.UI;

namespace NJM.Modules_Canvas {

    public class CanvasEntity : MonoBehaviour {

        public CanvasModuleRootLayer layer;

        public RectTransform root;
        public Canvas canvas;
        public CanvasScaler canvasScaler;
        public AspectRatioFitter aspectRatioFitter;

        public void SetResolution(Vector2Int resolution) {
            canvasScaler.referenceResolution = resolution;
        }

        public void SetFitRatio(AspectRatioFitter.AspectMode mode, float ratio) {
            aspectRatioFitter.aspectMode = mode;
            aspectRatioFitter.aspectRatio = ratio;
        }

        public void SetFitRatio_DontChangeMode(float ratio) {
            aspectRatioFitter.aspectRatio = ratio;
        }

        public void SetSortOrder(int order) {
            canvas.sortingOrder = order;
        }

        public void SetRenderMode(RenderMode mode) {
            canvas.renderMode = mode;
        }

        public void SetPixelPerfect(bool pixelPerfect) {
            canvas.pixelPerfect = pixelPerfect;
        }

        public void SetCamera(Camera camera) {
            canvas.worldCamera = camera;
        }

        public void SetMatchWidthOrHeight(float matchHeightPercent) {
            canvasScaler.matchWidthOrHeight = matchHeightPercent;
        }

    }

}