using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NJM.Modules_VFX;

namespace NJM {

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

        public async Task InitAsync() {
            await ctx.template.LoadAll();

            ctx.template.Foreach((VFXModuleSM tm) => {
                ctx.poolService.InitVFXSequence(tm.typeID, 6, () => VFXModuleFactory.VFX_New(ctx, tm.typeID));
            });
        }

        #region Lifecycle
        public VFXModuleSM Spawn(int typeID, UniqueSignature belong, Vector2 pos) {
            if (ctx.vfxRepo.IsExistLoop(typeID, belong)) {
                return null;
            }
            var vfxEntity = VFXModuleFactory.VFX_Create(ctx, ++ctx.idRecord, typeID, belong, pos);
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

        public void UnspawnBelongAndTypeID(UniqueSignature belong, int typeID) {
            int count = ctx.vfxRepo.TakeBelong(belong, out var vfxs);
            for (int i = 0; i < count; i++) {
                var vfx = vfxs[i];
                if (vfx.typeID != typeID) {
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