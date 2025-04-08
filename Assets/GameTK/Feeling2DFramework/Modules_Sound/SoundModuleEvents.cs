using System;
using UnityEngine;

namespace GameTK {

    public class SoundModuleEvents {

        public Func<UniqueSignature, Vector2> OnGetBelongPosHandle;
        public Vector2 OnGetBelongPos(UniqueSignature belong) {
            if (OnGetBelongPosHandle != null) {
                return OnGetBelongPosHandle(belong);
            }
            return Vector2.zero;
        }

        public SoundModuleEvents() {}
    }
}