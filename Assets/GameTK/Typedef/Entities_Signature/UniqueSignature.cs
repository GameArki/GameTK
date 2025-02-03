using System;
using System.Runtime.InteropServices;

namespace NJM {

    [StructLayout(LayoutKind.Explicit)]
    public struct UniqueSignature : IEquatable<UniqueSignature> {

        public static UniqueSignature Zero = new UniqueSignature(0, 0);

        [FieldOffset(0)]
        public ulong value;

        [FieldOffset(0)]
        public int entityType;

        [FieldOffset(4)]
        public int id;

        public UniqueSignature(int entityType, int entityID) {
            value = 0;
            this.entityType = entityType;
            this.id = entityID;
        }

        public string GetString() {
            return $"{entityType.ToString()}_{id}";
        }

        bool IEquatable<UniqueSignature>.Equals(UniqueSignature other) {
            return value == other.value;
        }

        public override bool Equals(object obj) {
            if (obj is UniqueSignature other) {
                return value == other.value;
            }
            return false;
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }

        public static bool operator ==(UniqueSignature a, UniqueSignature b) {
            return a.value == b.value;
        }

        public static bool operator !=(UniqueSignature a, UniqueSignature b) {
            return a.value != b.value;
        }

    }

}