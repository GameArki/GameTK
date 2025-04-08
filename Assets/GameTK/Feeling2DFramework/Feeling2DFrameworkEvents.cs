using System;
using UnityEngine;

namespace GameTK.Framework_Feeling2D {

    public class Feeling2DFrameworkEvents {

        public Func<UniqueSignature, Vector2> OnGetBelongPosHandle;
        public Vector2 OnGetBelongPos(UniqueSignature belong) {
            if (OnGetBelongPosHandle != null) {
                return OnGetBelongPosHandle.Invoke(belong);
            }
            return Vector2.zero;
        }

    }

}