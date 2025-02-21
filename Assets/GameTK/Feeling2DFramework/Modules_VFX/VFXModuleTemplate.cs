using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NJM.Modules_VFX {

    public class VFXModuleTemplate {

        Dictionary<ulong, VFXModuleSM> all;

        public VFXModuleTemplate() {
            all = new Dictionary<ulong, VFXModuleSM>();
        }

        public async Task LoadAll() {
            try {
                const string LABEL = "VFX";
                var asyncOperationHandle = Addressables.LoadAssetsAsync<GameObject>(LABEL, null);
                var list = await asyncOperationHandle.Task;
                foreach (var item in list) {
                    var sm = item.GetComponent<VFXModuleSM>();
                    ulong key = GetKey(sm.typeGroup, sm.typeID);
                    all.Add(key, sm);
                }
                Addressables.Release(asyncOperationHandle);
            } catch (InvalidKeyException e) {
                Debug.LogWarning(e);
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public bool TryGet(int typeGroup, int typeID, out VFXModuleSM sfxSO) {
            ulong key = GetKey(typeGroup, typeID);
            return all.TryGetValue(key, out sfxSO);
        }

        ulong GetKey(int typeGroup, int typeID) {
            return ((ulong)(uint)typeGroup << 32) | (uint)typeID;
        }

        public void Foreach(Action<VFXModuleSM> action) {
            foreach (var item in all.Values) {
                action(item);
            }
        }

    }

}