using System;
using System.Runtime.InteropServices;
using GameFunctions;

namespace NJM {

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct StageSignature : IEquatable<StageSignature> {
        public static StageSignature Zero => new StageSignature(0, 0, 0);
        [FieldOffset(0)]
        [NonSerialized]
        public int value;
        [FieldOffset(0)]
        [NonSerialized]
        public uint uvalue;
        [FieldOffset(0)]
        public ushort chapterNumber;
        [FieldOffset(2)]
        public byte stageNumber;
        [FieldOffset(3)]
        public byte subStageNumber;
        public StageSignature(ushort chapter, byte stage, byte subStage) {
            this.value = 0;
            this.uvalue = 0;
            this.chapterNumber = chapter;
            this.stageNumber = stage;
            this.subStageNumber = subStage;
        }

        public readonly void CopyTo(out StageSignature other) {
            other = this;
        }

        public string GetString() {
            return chapterNumber + "-" + stageNumber + "-" + subStageNumber;
        }

        public static bool operator ==(StageSignature a, StageSignature b) {
            return a.value == b.value;
        }

        public static bool operator !=(StageSignature a, StageSignature b) {
            return a.value != b.value;
        }

        public override bool Equals(object obj) {
            return obj is StageSignature signature && value == signature.value;
        }

        public override int GetHashCode() {
            return value;
        }

        public static implicit operator StageSignature(uint uvalue) {
            return new StageSignature { uvalue = uvalue };
        }

        public static implicit operator uint(StageSignature signature) {
            return signature.uvalue;
        }

        public bool Equals(StageSignature other) {
            return value == other.value;
        }
    }
}