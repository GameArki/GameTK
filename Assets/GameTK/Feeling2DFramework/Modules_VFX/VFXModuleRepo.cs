using System;
using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace NJM.Modules_VFX {

    public class VFXModuleRepo {

        Dictionary<int, VFXModuleSM> all;
        Dictionary<int, HashSet<VFXModuleSM>> allByTypeID;

        Pool<HashSet<VFXModuleSM>> pool;

        VFXModuleSM[] temp;
        VFXModuleSM[] tempForBelong;

        public VFXModuleRepo() {
            all = new Dictionary<int, VFXModuleSM>();
            allByTypeID = new Dictionary<int, HashSet<VFXModuleSM>>();
            pool = new Pool<HashSet<VFXModuleSM>>(10, () => new HashSet<VFXModuleSM>());
            temp = new VFXModuleSM[1000];
            tempForBelong = new VFXModuleSM[1000];
        }

        public void Add(VFXModuleSM vfx) {
            bool succ = all.TryAdd(vfx.id, vfx);
            if (!succ) {
                Debug.LogError($"VFXRepo.AddSeq: Add failed, id={vfx.id}");
            }
            
            if (!allByTypeID.TryGetValue(vfx.typeID, out var set)) {
                set = pool.Get();
                allByTypeID.Add(vfx.typeID, set);
            }
            set.Add(vfx);
        }

        public void Remove(int id) {
            bool has = all.TryGetValue(id, out var vfx);
            if (has) {
                all.Remove(id);

                allByTypeID.TryGetValue(vfx.typeID, out var set);
                if (set != null) {
                    set.Remove(vfx);
                    if (set.Count == 0) {
                        allByTypeID.Remove(vfx.typeID);
                        pool.Return(set);
                    }
                }
            }
        }

        public bool IsExistLoop(int typeID, UniqueSignature belong) {
            bool res = allByTypeID.TryGetValue(typeID, out var vfxs);
            if (res) {
                foreach (var vfx in vfxs) {
                    if (vfx.belong == belong && vfx.IsLoop()) {
                        return true;
                    }
                }
            }
            return false;
        }

        public int TakeBelong(UniqueSignature belong, out VFXModuleSM[] result) {
            int count = 0;
            foreach (var kv in all) {
                if (kv.Value.belong == belong) {
                    tempForBelong[count++] = kv.Value;
                }
            }
            result = tempForBelong;
            return count;
        }

        public int TakeAll(out VFXModuleSM[] result) {
            if (all.Count > temp.Length) {
                temp = new VFXModuleSM[all.Count];
            }
            all.Values.CopyTo(temp, 0);
            result = temp;
            return all.Count;
        }

    }

}