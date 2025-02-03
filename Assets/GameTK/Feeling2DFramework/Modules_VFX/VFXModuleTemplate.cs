using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NJM.Modules_VFX {

    public class VFXModuleTemplate {

        Dictionary<int, VFXModuleSM> all;

        public VFXModuleTemplate() {
            all = new Dictionary<int, VFXModuleSM>();
        }

        public async Task LoadAll() {
            const string LABEL = "VFX";
            var asyncOperationHandle = Addressables.LoadAssetsAsync<GameObject>(LABEL, null);
            var list = await asyncOperationHandle.Task;
            foreach (var item in list) {
                var sm = item.GetComponent<VFXModuleSM>();
                all.Add(sm.typeID, sm);
            }
            Addressables.Release(asyncOperationHandle);
        }

        public bool TryGet(int typeID, out VFXModuleSM sfxSO) {
            return all.TryGetValue(typeID, out sfxSO);
        }

        public void Foreach(Action<VFXModuleSM> action) {
            foreach (var item in all.Values) {
                action(item);
            }
        }

    }

}