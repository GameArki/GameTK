using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameTK.Cores_Assets {

    public class AssetsModule : MonoBehaviour {

        // **** Custom ****
        Dictionary<string, GameObject> entityPrefabs;
        AsyncOperationHandle entityHandle;
        // **** Custom ****

        public void Ctor() {
            entityPrefabs = new Dictionary<string, GameObject>();
        }

        public async Task LoadAll() {
            try {
                // **** Custom ****
                {
                    string label = "Entity";
                    var handle = Addressables.LoadAssetsAsync<GameObject>(label, null);
                    var list = await handle.Task;
                    foreach (var prefab in list) {
                        entityPrefabs.Add(prefab.name, prefab);
                    }
                    entityHandle = handle;
                }
                // **** Custom ****
            } catch (Exception e) {
                Debug.LogWarning(e);
            }
        }

        public void UnloadAll() {
            if (entityHandle.IsValid()) {
                Addressables.Release(entityHandle);
            }
        }

    }

}
