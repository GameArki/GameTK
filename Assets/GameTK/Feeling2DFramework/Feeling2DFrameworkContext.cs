using System;
using UnityEngine;
using NJM.Modules_VFX;
using NJM.Modules_Sound;
using NJM.Modules_Renderer;
using NJM.Modules_Camera;

namespace NJM.Framework_Feeling2D {

    public class Feeling2DFrameworkContext {

        public CameraModule cameraModule;
        public VFXModule vfxModule;
        public SoundModule soundModule;
        public RendererModule rendererModule;

        public Feeling2DFrameworkContext() { }

    }

}