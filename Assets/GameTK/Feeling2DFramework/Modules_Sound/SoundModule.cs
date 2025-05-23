using System.Threading.Tasks;
using UnityEngine;
using GameFunctions;
using GameTK.Modules_Sound;

namespace GameTK {

    public class SoundModule {

        SoundModuleContext ctx;

        SoundModuleEvents events;
        public SoundModuleEvents Events => events;

        public SoundModule() {
            ctx = new SoundModuleContext();
            events = new SoundModuleEvents();
            ctx.pool_entity = new Pool<SoundModuleEntity>(10, () => new SoundModuleEntity());
        }

        public void Inject() {
            ctx.Inject(new GameObject("SoundPoolRoot").transform);
        }

        public async Task InitAsync() {
            await ctx.template.LoadAll();
        }

        public void TearDown_ButBGM() {
            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                if (entity.layer.IsBGM()) {
                    continue;
                }
                ctx.repo.Remove(entity.id);
                ctx.pool_entity.Return(entity);
                entity.Release();
            }
        }

        public void Layer_SetLimited(SoundLayerType layer, int count) {
            if (ctx.processEntity.setting_layerLimited.TryGetValue(layer, out var existCount)) {
                ctx.processEntity.setting_layerLimited[layer] = count;
            } else {
                ctx.processEntity.setting_layerLimited.Add(layer, count);
            }
        }

        public void Play(SoundModuleTM tm, UniqueSignature belong, Vector2 happenPos) {
            if (tm.isLoop) {
                bool has = ctx.repo.TryGetByTypeID(tm.typeGroup, tm.typeID, out var existEntity);
                if (has) {
                    return;
                }
            }

            bool isLimited = ctx.processEntity.setting_layerLimited.TryGetValue(tm.layer, out var layerCount);
            if (isLimited && ctx.repo.GetLayerCount(tm.layer) >= layerCount) {
                return;
            }

            SoundModuleEntity entity = SoundModuleFactory.Create(ctx, tm, belong, happenPos);
            if (entity == null) {
                return;
            }
            float volume = GetVolume(entity);
            entity.Play(volume);

            ctx.repo.Add(entity);
        }

        public void Play(int typeGroup, int typeID, UniqueSignature belong, Vector2 happenPos) {
            bool has = ctx.template.TryGet(typeGroup, typeID, out var sfxSO);
            if (!has) {
                Debug.LogError($"SoundModule.Play: typeID={typeID} not found");
                return;
            }
            Play(sfxSO.tm, belong, happenPos);
        }

        public void Pause(int typeGroup, int typeID, UniqueSignature belong) {
            bool has = ctx.repo.TryGetByTypeID(typeGroup, typeID, out var entity);
            if (!has) {
                return;
            }
            entity.Pause();
        }

        public void BGM_StopAll(float fadeOutDuration) {
            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                if (entity.layer.IsBGM()) {
                    entity.FadeOut_Begin(fadeOutDuration);
                }
            }
        }

        public void BGM_StopAllByLayer(SoundLayerType layer, float fadeOutDuration) {
            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                if (entity.layer.IsBGM() && entity.layer == layer) {
                    entity.FadeOut_Begin(fadeOutDuration);
                }
            }
        }

        public void BGM_PauseAll() {
            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                if (entity.layer.IsBGM()) {
                    entity.Pause();
                }
            }
        }

        public void BGM_ResumeAll() {
            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                if (entity.layer.IsBGM()) {
                    entity.Play(GetVolume(entity));
                }
            }
        }

        public void Sound_Stop(UniqueSignature belong, int typeGroup, int typeID) {
            bool has = ctx.repo.TryGetByTypeID(typeGroup, typeID, out var entity);
            if (!has) {
                return;
            }
            if (entity.belong != belong) {
                return;
            }
            entity.FadeOut_Begin(0);
        }

        public void DestroyBelong(UniqueSignature belong) {
            int count = ctx.repo.TakeAllBelong(belong, out var array);
            for (int i = 0; i < count; i++) {
                var entity = array[i];
                ctx.repo.Remove(entity.id);
                ctx.pool_entity.Return(entity);
                entity.Release();
            }
        }

        public void BGM_Volume_Set(float volume) {
            Volume_Set(ctx.processEntity.sfxVolume, volume);
        }

        public void SFX_Volume_Set(float volume) {
            Volume_Set(volume, ctx.processEntity.bgmVolume);
        }

        float GetVolume(SoundModuleEntity entity) {
            float volume;
            if (entity.layer.IsBGM()) {
                volume = Volume_GetBGM();
            } else {
                volume = Volume_GetSFX();
            }
            if (entity.isEffectByDistance) {
                volume = Volume_GetByDistance(entity.happenPos, volume);
            }
            return volume;
        }

        public void Tick(SoundModuleUpdateArgs args, float dt) {
            var processEntity = ctx.processEntity;
            processEntity.listenerPos = args.listenerPosition;
            if (args.bgmVolume != processEntity.bgmVolume || args.sfxVolume != processEntity.sfxVolume) {
                Volume_Set(args.sfxVolume, args.bgmVolume);
            }

            // 自动移除
            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                entity.FadeOut_Tick(dt);
                if (!entity.IsPlaying()) {
                    if (!entity.isPause) {
                        ctx.repo.Remove(entity.id);
                        ctx.pool_entity.Return(entity);
                    }
                } else {
                    if (entity.isFollowBelong) {
                        Vector2 pos = events.OnGetBelongPos(entity.belong);
                        entity.happenPos = pos;
                        entity.Volume_Set(GetVolume(entity));
                    }
                }
            }
        }

        #region Volume
        public void Volume_Set(float sfxVolume, float bgmVolume) {
            var processEntity = ctx.processEntity;
            processEntity.bgmVolume = bgmVolume;
            processEntity.sfxVolume = sfxVolume;

            int len = ctx.repo.TakeAll(out var array);
            for (int i = 0; i < len; i++) {
                var entity = array[i];
                float volume = GetVolume(entity);
                entity.Volume_Set(volume);
            }
        }

        float Volume_GetBGM() {
            return ctx.processEntity.bgmVolume;
        }

        float Volume_GetSFX() {
            return ctx.processEntity.sfxVolume;
        }
        #endregion

        float Volume_GetByDistance(Vector2 happenPos, float volume) {
            var processEntity = ctx.processEntity;
            var listenerPos = processEntity.listenerPos;
            float thresholdDistance = processEntity.thresholdDistance;
            float dis = Vector2.Distance(listenerPos, happenPos);
            if (dis >= thresholdDistance) {
                return 0;
            }
            return (1 - dis / thresholdDistance) * volume;
        }

    }

}