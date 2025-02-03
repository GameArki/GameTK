using System;
using System.Runtime.InteropServices;

namespace NJM {

    [StructLayout(LayoutKind.Explicit)]
    [Serializable]
    public struct TypeSignature : IEquatable<TypeSignature> {

        public static TypeSignature Zero = new TypeSignature(EntityType.None, 0, 0);

        [FieldOffset(0)]
        [UnityEngine.HideInInspector] public decimal value;

        [FieldOffset(0)]
        public EntityType entityType;

        [FieldOffset(4)]
        public int typeID;

        [FieldOffset(8)]
        public int specIndex;

        public TypeSignature(EntityType entityType, int typeID, int specIndex) {
            value = 0;
            this.entityType = entityType;
            this.typeID = typeID;
            this.specIndex = specIndex;
        }

        public string GetString() {
            return $"{entityType.ToString()}_{typeID}_{specIndex}";
        }

        bool IEquatable<TypeSignature>.Equals(TypeSignature other) {
            return value == other.value;
        }

        public override bool Equals(object obj) {
            if (obj is TypeSignature other) {
                return value == other.value;
            }
            return false;
        }

        public override int GetHashCode() {
            return value.GetHashCode();
        }

        public static bool operator ==(TypeSignature a, TypeSignature b) {
            return a.value == b.value;
        }

        public static bool operator !=(TypeSignature a, TypeSignature b) {
            return a.value != b.value;
        }

    }

}