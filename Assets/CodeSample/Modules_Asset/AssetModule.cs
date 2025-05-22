using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NJM {

    public class AssetModule {

        Dictionary<string, GameObject> uiPanels;
        Dictionary<int, AssetFakeSO> fakes;

        public AssetModule() {
            uiPanels = new Dictionary<string, GameObject>();
            fakes = new Dictionary<int, AssetFakeSO>();
        }

        public async Task Load() {
            await Panels_Load();
            await Fakes_Load();
        }

        async Task Panels_Load() {
            var handle = Addressables.LoadAssetsAsync<GameObject>("UIPanel", null);
            var list = await handle.Task;
            foreach (var panel in list) {
                uiPanels.Add(panel.name, panel);
            }
            Addressables.Release(handle);
        }

        async Task Fakes_Load() {
            var handle = Addressables.LoadAssetsAsync<AssetFakeSO>("FakeSO", null);
            var list = await handle.Task;
            foreach (var fake in list) {
                fakes.Add(fake.typeID, fake);
            }
            Addressables.Release(handle);
        }

    }

}