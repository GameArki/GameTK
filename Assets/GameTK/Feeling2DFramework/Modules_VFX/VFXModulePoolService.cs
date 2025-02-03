using System;
using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace NJM.Modules_VFX {

    public class VFXModulePoolService {

        public Dictionary<int, Pool<VFXModuleSM>> vfxPool;

        public VFXModulePoolService() {
            vfxPool = new Dictionary<int, Pool<VFXModuleSM>>();
        }

        public void InitVFXSequence(int typeID, int size, Func<VFXModuleSM> createFunc) {
            if (!vfxPool.TryGetValue(typeID, out var pool)) {
                pool = new Pool<VFXModuleSM>(size, () => createFunc());
                vfxPool.Add(typeID, pool);
            }
        }

        public VFXModuleSM Get(int typeID) {
            if (!vfxPool.TryGetValue(typeID, out var pool)) {
                return null;
            }
            return pool.Get();
        }

        public void VFX_Return(VFXModuleSM sm) {
            var typeID = sm.typeID;
            if (!vfxPool.TryGetValue(typeID, out var pool)) {
                pool = new Pool<VFXModuleSM>(1, () => new VFXModuleSM());
                vfxPool.Add(typeID, pool);
            }
            pool.Return(sm);
        }

    }

}