using System.Threading.Tasks;
using UnityEngine;
using GameFunctions;
using NJM.Modules_Sound;

namespace NJM {

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
                ctx.pool_entity.Return(entity);
                entity.Release();
            }
        }

        public void Play(SoundModuleSO so, UniqueSignature belong, Vector2 happenPos) {
            if (so.isLoop) {
                bool has = ctx.repo.TryGetByTypeID(so.typeID, out var existEntity);
                if (has) {
                    return;
                }
            }

            SoundModuleEntity entity = SoundModuleFactory.Create(ctx, so, belong, happenPos);
            if (entity == null) {
                return;
            }
            float volume = GetVolume(entity);
            entity.Play(volume);

            ctx.repo.Add(entity);
        }

        public void Play(int typeID, UniqueSignature belong, Vector2 happenPos) {
            bool has = ctx.template.TryGet(typeID, out var sfxSO);
            if (!has) {
                Debug.LogError($"SoundModule.Play: typeID={typeID} not found");
                return;
            }
            Play(sfxSO, belong, happenPos);
        }

        public void Pause(int typeID, UniqueSignature belong) {
            bool has = ctx.repo.TryGetByTypeID(typeID, out var entity);
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

        public void DestroyBelong(UniqueSignature belong) {
            int count = ctx.repo.TakeAllBelong(belong, out var array);
            for (int i = 0; i < count; i++) {
                var entity = array[i];
                entity.Release();
                ctx.repo.Remove(entity.id);
                ctx.pool_entity.Return(entity);
            }
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
                if (!entity.isLoop) {
                    if (!entity.IsPlaying()) {
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
        void Volume_Set(float sfxVolume, float bgmVolume) {
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