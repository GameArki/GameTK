using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameTK.Modules_Sound {

    public class SoundModuleTemplate {

        Dictionary<ulong, SoundModuleSO> all;
        AsyncOperationHandle handle;

        public SoundModuleTemplate() {
            all = new Dictionary<ulong, SoundModuleSO>();
        }

        public IEnumerator LoadAllIE() {
            const string LABEL = "Sound";
            var handle = Addressables.LoadAssetsAsync<SoundModuleSO>(LABEL, null);
            while (!handle.IsDone) {
                yield return null;
            }
            var list = handle.Result;
            foreach (var item in list) {
                ulong key = GetKey(item.tm.typeGroup, item.tm.typeID);
                all.Add(key, item);
            }
            this.handle = handle;
        }

        public void Release() {
            if (handle.IsValid()) {
                Addressables.Release(handle);
            }
        }

        public bool TryGet(int typeGroup, int typeID, out SoundModuleSO sfxSO) {
            ulong key = GetKey(typeGroup, typeID);
            return all.TryGetValue(key, out sfxSO);
        }

        ulong GetKey(int typeGroup, int typeID) {
            return (ulong)typeGroup << 32 | (uint)typeID;
        }

    }

}