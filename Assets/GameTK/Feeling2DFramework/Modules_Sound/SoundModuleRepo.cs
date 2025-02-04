using System;
using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace NJM.Modules_Sound {

    public class SoundModuleRepo {

        Dictionary<int, SoundModuleEntity> all;
        Dictionary<UniqueSignature, List<SoundModuleEntity>> belongDict;
        Pool<List<SoundModuleEntity>> belongListPool;

        SoundModuleEntity[] tempArray;
        SoundModuleEntity[] tempBelongArray;

        public SoundModuleRepo() {
            all = new Dictionary<int, SoundModuleEntity>();
            belongDict = new Dictionary<UniqueSignature, List<SoundModuleEntity>>();
            belongListPool = new Pool<List<SoundModuleEntity>>(10, () => new List<SoundModuleEntity>());
            tempArray = new SoundModuleEntity[100];
            tempBelongArray = new SoundModuleEntity[100];
        }

        public void Add(SoundModuleEntity entity) {
            all.Add(entity.id, entity);
            if (entity.isLoop) {
                if (!belongDict.TryGetValue(entity.belong, out var list)) {
                    list = belongListPool.Get();
                    belongDict.Add(entity.belong, list);
                }
                list.Add(entity);
            }
        }

        public bool TryGet(int id, out SoundModuleEntity entity) {
            return all.TryGetValue(id, out entity);
        }

        public bool TryGetByTypeID(int typeGorup, int typeID, out SoundModuleEntity entity) {
            foreach (var item in all.Values) {
                if (item.typeGroup == typeGorup && item.typeID == typeID) {
                    entity = item;
                    return true;
                }
            }
            entity = null;
            return false;
        }

        public int TakeAllBelong(UniqueSignature belong, out SoundModuleEntity[] array) {
            array = null;
            if (!belongDict.TryGetValue(belong, out var list)) {
                return 0;
            }
            if (tempBelongArray.Length < list.Count) {
                tempBelongArray = new SoundModuleEntity[list.Count];
            }
            list.CopyTo(tempBelongArray, 0);
            array = tempBelongArray;
            return list.Count;
        }

        public void Remove(int id) {
            bool has = all.TryGetValue(id, out var entity);
            if (!has) {
                return;
            }
            all.Remove(id);
            if (entity.isLoop) {
                if (belongDict.TryGetValue(entity.belong, out var list)) {
                    list.Remove(entity);
                    if (list.Count == 0) {
                        belongDict.Remove(entity.belong);
                        belongListPool.Return(list);
                    }
                }
            }
        }

        public void Remove(SoundModuleEntity entity) {
            Remove(entity.id);
        }

        public int TakeAll(out SoundModuleEntity[] array) {
            if (tempArray.Length < all.Count) {
                tempArray = new SoundModuleEntity[all.Count];
            }
            all.Values.CopyTo(tempArray, 0);
            array = tempArray;
            return all.Count;
        }
    }
}