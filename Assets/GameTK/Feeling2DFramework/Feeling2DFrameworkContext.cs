using System;
using UnityEngine;
using GameTK.Modules_VFX;
using GameTK.Modules_Sound;
using GameTK.Modules_Renderer;
using GameTK.Modules_Camera;

namespace GameTK.Framework_Feeling2D {

    public class Feeling2DFrameworkContext {

        public CameraModule cameraModule;
        public VFXModule vfxModule;
        public SoundModule soundModule;
        public RendererModule rendererModule;
        public RumbleModule rumbleModule;

        public Feeling2DFrameworkContext() { }

    }

}