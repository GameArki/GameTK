using System;
using UnityEngine;

namespace NJM.Modules_Sound {

    public static class SoundModuleFactory {

        public static SoundModuleEntity Create(SoundModuleContext ctx, int typeID, UniqueSignature belong, Vector2 happenPos) {
            bool has = ctx.template.TryGet(typeID, out var sfxSO);
            if (!has) {
                Debug.LogError($"SoundModule.Create: typeID={typeID} not found");
                return null;
            }
            var entity = ctx.pool_entity.Get();
            entity.id = ++ctx.idRecord;
            entity.typeID = typeID;
            entity.belong = belong;
            entity.layer = sfxSO.layer;
            entity.isLoop = sfxSO.isLoop;
            entity.isFollowBelong = sfxSO.isFollowBelong;
            entity.happenPos = happenPos;
            entity.clip = sfxSO.clip;
            entity.isEffectByDistance = sfxSO.isEffectByDistance;
            entity.volumePercent = sfxSO.volumePercent;
            entity.player = CreateSource(ctx);
            return entity;
        }

        public static AudioSource CreateSource(SoundModuleContext ctx) {
            var go = new GameObject("SoundPlayer");
            go.transform.SetParent(ctx.poolRoot);
            var player = go.AddComponent<AudioSource>();
            return player;
        }

    }

}