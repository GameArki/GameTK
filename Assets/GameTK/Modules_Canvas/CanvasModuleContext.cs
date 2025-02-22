using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NJM.Modules_Canvas {

    public class CanvasModuleContext {

        public GameObject lastSelectedGameObject;

        public CursorEntity cursorEntity;

        public Dictionary<CanvasModuleRootLayer, CanvasEntity> canvases;

        public CanvasModuleContext() {
            cursorEntity = new CursorEntity();
            canvases = new Dictionary<CanvasModuleRootLayer, CanvasEntity>();
        }

    }

}