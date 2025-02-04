using System;
using UnityEngine;
using TriInspector;

namespace NJM {

    public class VFXModuleSM : MonoBehaviour {

        public int id;
        public int typeGroup;
        public int typeID;
        public UniqueSignature belong;
        public bool isFollowBelong;
        public bool isDestroyWhenBelongDestroy;
        public int spriteLayer = 700;

        [SerializeField] SpriteRenderer sr;

        [SerializeField] VFXType type;
        [ShowIf(nameof(type), VFXType.SpriteSeq), SerializeField] SpriteSeqModel seq;
        [ShowIf(nameof(type), VFXType.Animator), SerializeField] Animator anim;
        [ShowIf(nameof(type), VFXType.Particle), SerializeField] ParticleSystem ps;
        [NonSerialized] public bool isEnd;

        public void Ctor() { }

        public void Reuse() {
            gameObject.SetActive(true);
        }

        public void Release() {
            isEnd = false;
            if (type == VFXType.SpriteSeq) {
                seq.Reset();
            }
            sr.sortingOrder = spriteLayer;
            gameObject.SetActive(false);
        }

        public void Tick(float dt) {
            if (isEnd) {
                return;
            }
            if (type == VFXType.SpriteSeq) {
                Tick_SpriteSeq(dt);
            }
        }

        void Tick_SpriteSeq(float dt) {
            sbyte status = seq.Tick(dt, out var spr);
            if (status == -1) {
                isEnd = true;
                return;
            }
            if (status == 0 || status == 1) {
                sr.sprite = spr;
            }
            if (status == 1) {
                isEnd = true;
            }
        }

        public bool IsLoop() {
            if (type == VFXType.SpriteSeq) {
                return seq.isLoop;
            }
            return false;
        }

    }

}