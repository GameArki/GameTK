using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameTK.Modules_VFX {

    public class VFXModuleTemplate {

        Dictionary<ulong, VFXModuleSM> all;
        AsyncOperationHandle handle;

        public VFXModuleTemplate() {
            all = new Dictionary<ulong, VFXModuleSM>();
        }

        public IEnumerator LoadAllIE() {
            const string LABEL = "VFX";
            var handle = Addressables.LoadAssetsAsync<GameObject>(LABEL, null);
            while (!handle.IsDone) {
                yield return null;
            }
            var list = handle.Result;
            foreach (var item in list) {
                var sm = item.GetComponent<VFXModuleSM>();
                ulong key = GetKey(sm.typeGroup, sm.typeID);
                all.Add(key, sm);
            }
            this.handle = handle;
        }

        public void Relase() {
            if (handle.IsValid()) {
                Addressables.Release(handle);
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