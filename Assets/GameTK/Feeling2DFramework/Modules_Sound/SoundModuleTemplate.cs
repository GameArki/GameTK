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
            const string LABEL = "SFX";
            var asyncOperationHandle = Addressables.LoadAssetsAsync<SoundModuleSO>(LABEL, null);
            var list = await asyncOperationHandle.Task;
            foreach (var item in list) {
                all.Add(item.typeID, item);
            }
            Addressables.Release(asyncOperationHandle);
        }

        public bool TryGet(int typeID, out SoundModuleSO sfxSO) {
            return all.TryGetValue(typeID, out sfxSO);
        }

    }

}