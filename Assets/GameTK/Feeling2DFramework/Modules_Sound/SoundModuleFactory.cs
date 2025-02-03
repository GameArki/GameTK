using System;
using UnityEngine;

namespace NJM.Modules_Sound {

    public static class SoundModuleFactory {

        public static SoundModuleEntity Create(SoundModuleContext ctx, SoundModuleSO so, UniqueSignature belong, Vector2 happenPos) {
            var entity = ctx.pool_entity.Get();
            entity.id = ++ctx.idRecord;
            entity.typeID = so.typeID;
            entity.belong = belong;
            entity.layer = so.layer;
            entity.isLoop = so.isLoop;
            entity.isFollowBelong = so.isFollowBelong;
            entity.happenPos = happenPos;
            entity.clip = so.clip;
            entity.isEffectByDistance = so.isEffectByDistance;
            entity.volumePercent = so.volumePercent;
            entity.player = CreateSource(ctx);
            return entity;
        }

        public static SoundModuleEntity Create(SoundModuleContext ctx, int typeID, UniqueSignature belong, Vector2 happenPos) {
            bool has = ctx.template.TryGet(typeID, out var sfxSO);
            if (!has) {
                Debug.LogError($"SoundModule.Create: typeID={typeID} not found");
                return null;
            }
            return Create(ctx, sfxSO, belong, happenPos);
        }

        public static AudioSource CreateSource(SoundModuleContext ctx) {
            var go = new GameObject("SoundPlayer");
            go.transform.SetParent(ctx.poolRoot);
            var player = go.AddComponent<AudioSource>();
            return player;
        }

    }

}