using System;
using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace GameTK.Modules_VFX {

    public class VFXModulePoolService {

        public Dictionary<ulong, Pool<VFXModuleSM>> vfxPool;

        public VFXModulePoolService() {
            vfxPool = new Dictionary<ulong, Pool<VFXModuleSM>>();
        }

        public void InitVFXSequence(int typeGorup, int typeID, int size, Func<VFXModuleSM> createFunc) {
            ulong key = GetKey(typeGorup, typeID);
            if (!vfxPool.TryGetValue(key, out var pool)) {
                pool = new Pool<VFXModuleSM>(size, () => createFunc());
                vfxPool.Add(key, pool);
            }
        }

        public VFXModuleSM Get(int typeGroup, int typeID) {
            ulong key = GetKey(typeGroup, typeID);
            if (!vfxPool.TryGetValue(key, out var pool)) {
                return null;
            }
            return pool.Get();
        }

        public void VFX_Return(VFXModuleSM sm) {
            ulong key = GetKey(sm.typeGroup, sm.typeID);
            if (!vfxPool.TryGetValue(key, out var pool)) {
                pool = new Pool<VFXModuleSM>(1, () => new VFXModuleSM());
                vfxPool.Add(key, pool);
            }
            pool.Return(sm);
        }

        ulong GetKey(int typeGroup, int typeID) {
            return (ulong)typeGroup << 32 | (uint)typeID;
        }

    }

}