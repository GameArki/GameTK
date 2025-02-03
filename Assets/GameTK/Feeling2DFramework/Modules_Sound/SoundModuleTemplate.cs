using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NJM.Modules_Sound {

    public class SoundModuleTemplate {

        Dictionary<int, SoundModuleSO> all;

        public SoundModuleTemplate() {
            all = new Dictionary<int, SoundModuleSO>();
        }

        public async Task LoadAll() {
            try {
                const string LABEL = "Sound";
                var asyncOperationHandle = Addressables.LoadAssetsAsync<SoundModuleSO>(LABEL, null);
                var list = await asyncOperationHandle.Task;
                foreach (var item in list) {
                    all.Add(item.tm.typeID, item);
                }
                Addressables.Release(asyncOperationHandle);
            } catch (InvalidKeyException) {

            } catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public bool TryGet(int typeID, out SoundModuleSO sfxSO) {
            return all.TryGetValue(typeID, out sfxSO);
        }

    }

}