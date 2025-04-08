using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameTK.Modules_Sound {

    public class SoundModuleTemplate {

        Dictionary<ulong, SoundModuleSO> all;

        public SoundModuleTemplate() {
            all = new Dictionary<ulong, SoundModuleSO>();
        }

        public async Task LoadAll() {
            try {
                const string LABEL = "Sound";
                var asyncOperationHandle = Addressables.LoadAssetsAsync<SoundModuleSO>(LABEL, null);
                var list = await asyncOperationHandle.Task;
                foreach (var item in list) {
                    ulong key = GetKey(item.tm.typeGroup, item.tm.typeID);
                    all.Add(key, item);
                }
                Addressables.Release(asyncOperationHandle);
            } catch (InvalidKeyException) {

            } catch (Exception e) {
                Debug.LogError(e);
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