using System;
using System.Collections;
using UnityEngine;
using GameTK.Modules_VFX;

namespace GameTK {

    public class VFXModule {

        VFXModuleContext ctx;

        VFXModuleEvents events;
        public VFXModuleEvents Events => events;

        public VFXModule() {
            events = new VFXModuleEvents();
            ctx = new VFXModuleContext();
            ctx.poolService = new VFXModulePoolService();
        }

        public void Inject() {
            ctx.poolRoot = new GameObject("VFXPoolRoot").transform;
        }

        public IEnumerator InitIE() {
            yield return ctx.template.LoadAllIE();

            ctx.template.Foreach((VFXModuleSM sm) => {
                ctx.poolService.InitVFXSequence(sm.typeGroup, sm.typeID, 6, () => VFXModuleFactory.VFX_New(ctx, sm.typeGroup, sm.typeID));
            });
        }

        public void Release() {
            ctx.template.Relase();
        }

        #region Lifecycle
        public VFXModuleSM Spawn(VFXModuleSM prefab, UniqueSignature belong, Vector2 pos) {
            return Spawn(prefab.typeGroup, prefab.typeID, belong, pos);
        }

        public VFXModuleSM Spawn(int typeGroup, int typeID, UniqueSignature belong, Vector2 pos) {
            if (ctx.vfxRepo.IsExistLoop(typeGroup, typeID, belong)) {
                return null;
            }
            var vfxEntity = VFXModuleFactory.VFX_Create(ctx, ++ctx.idRecord, typeGroup, typeID, belong, pos);
            if (vfxEntity != null) {
                ctx.vfxRepo.Add(vfxEntity);
                return vfxEntity;
            }
            return null;
        }

        public void Unspawn(VFXModuleSM vfx) {
            ctx.vfxRepo.Remove(vfx.id);
            VFXModuleFactory.VFX_Release(ctx, vfx);
        }

        public void UnspawnBelongAndTypeID(UniqueSignature belong, int typeGroup, int typeID) {
            int count = ctx.vfxRepo.TakeBelong(belong, out var vfxs);
            for (int i = 0; i < count; i++) {
                var vfx = vfxs[i];
                if (vfx.typeGroup == typeGroup && vfx.typeID != typeID) {
                    continue;
                }
                if (vfx.isDestroyWhenBelongDestroy) {
                    Unspawn(vfx);
                }
            }
        }

        public void UnspawnBelong(UniqueSignature belong) {
            int count = ctx.vfxRepo.TakeBelong(belong, out var vfxs);
            for (int i = 0; i < count; i++) {
                var vfx = vfxs[i];
                if (vfx.isDestroyWhenBelongDestroy) {
                    Unspawn(vfx);
                }
            }
        }

        public void Tick(float dt) {

            int len = ctx.vfxRepo.TakeAll(out var vfxs);
            for (int i = 0; i < len; i++) {
                var vfx = vfxs[i];

                vfx.Tick(dt);

                // - Follow belong
                if (vfx.isFollowBelong) {
                    Vector2 pos = events.OnGetBelongPos(vfx.belong);
                    vfx.transform.position = pos;
                }

                if (vfx.isEnd) {
                    Unspawn(vfx);
                }
            }

        }

        public void TearDown() {
            int len = ctx.vfxRepo.TakeAll(out var vfxs);
            for (int i = 0; i < len; i++) {
                var vfx = vfxs[i];
                Unspawn(vfx);
            }
        }
        #endregion

    }

}