using System;
using UnityEngine;

namespace NJM.Modules_VFX {

    public static class VFXModuleFactory {

        #region VFXSequence
        public static VFXModuleSM VFX_New(VFXModuleContext ctx, int typeID) {
            bool has = ctx.template.TryGet(typeID, out var prefab);
            if (!has) {
                Debug.LogError("No VFXSM Prefab");
                return null;
            }
            var go = GameObject.Instantiate(prefab, ctx.poolRoot);
            VFXModuleSM entity = go.GetComponent<VFXModuleSM>();
            entity.Ctor();
            go.gameObject.SetActive(false);
            return entity;
        }

        public static VFXModuleSM VFX_Create(VFXModuleContext ctx, int id, int typeGroup, int typeID, UniqueSignature belong, Vector2 pos) {
            var vfxEntity = ctx.poolService.Get(typeGroup, typeID);
            vfxEntity.Reuse();
            vfxEntity.belong = belong;
            vfxEntity.id = id;
            vfxEntity.transform.position = pos;
            return vfxEntity;
        }

        public static void VFX_Release(VFXModuleContext ctx, VFXModuleSM vfx) {
            ctx.poolService.VFX_Return(vfx);
            vfx.Release();
        }
        #endregion VFXSequence

    }
}