using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NJM.Modules_Canvas {

    public class CanvasModuleContext {

        public GameObject lastSelectedGameObject;

        public Dictionary<CanvasModuleRootLayer, CanvasEntity> canvases;

        public CanvasModuleContext() {
            canvases = new Dictionary<CanvasModuleRootLayer, CanvasEntity>();
        }

    }

}