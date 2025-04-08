using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameTK.Modules_VFX {

    public class VFXModuleContext {

        public VFXModuleRepo vfxRepo;

        public VFXModulePoolService poolService;
        public VFXModuleTemplate template;

        public Transform poolRoot;

        public int idRecord;

        public VFXModuleContext() {
            vfxRepo = new VFXModuleRepo();
            template = new VFXModuleTemplate();
            idRecord = 1;
        }

    }

}