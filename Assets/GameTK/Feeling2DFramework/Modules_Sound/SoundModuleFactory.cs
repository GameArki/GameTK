using System;
using UnityEngine;

namespace NJM.Modules_Sound {

    public static class SoundModuleFactory {

        public static SoundModuleEntity Create(SoundModuleContext ctx, SoundModuleTM tm, UniqueSignature belong, Vector2 happenPos) {
            var entity = ctx.pool_entity.Get();
            entity.id = ++ctx.idRecord;
            entity.typeID = tm.typeID;
            entity.belong = belong;
            entity.layer = tm.layer;
            entity.isLoop = tm.isLoop;
            entity.isFollowBelong = tm.isFollowBelong;
            entity.happenPos = happenPos;
            entity.clip = tm.clip;
            entity.isEffectByDistance = tm.isEffectByDistance;
            entity.volumePercent = tm.volumePercent;
            if (entity.player == null) {
                entity.player = CreateSource(ctx);
            }
            return entity;
        }

        public static SoundModuleEntity Create(SoundModuleContext ctx, int typeID, UniqueSignature belong, Vector2 happenPos) {
            bool has = ctx.template.TryGet(typeID, out var sfxSO);
            if (!has) {
                Debug.LogError($"SoundModule.Create: typeID={typeID} not found");
                return null;
            }
            return Create(ctx, sfxSO.tm, belong, happenPos);
        }

        public static AudioSource CreateSource(SoundModuleContext ctx) {
            var go = new GameObject("SoundPlayer");
            go.transform.SetParent(ctx.poolRoot);
            var player = go.AddComponent<AudioSource>();
            return player;
        }

    }

}