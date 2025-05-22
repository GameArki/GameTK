using System.Collections.Generic;

namespace NJM {

    public enum InputKey : ushort {

        None = 0,
        MoveUp = 1,
        MoveDown = 2,
        MoveLeft = 3,
        MoveRight = 4,
        Jump = 5,
        Melee = 6, // Flash
        Skill1 = 7, // ExFlash
        Skill2 = 8, // Fireball
        Skill3 = 9, // Shibi

        Interact = 11,
        Pause = 12,

        LockMove = 14,

        Cancel = 15,
        Submit = 16,

    }

    public static class InputKeyExtension {

        static readonly Dictionary<InputKey, string> enumToStringDict = new Dictionary<InputKey, string>() {
            { InputKey.None, string.Empty },
            { InputKey.MoveUp, "MoveUp" },
            { InputKey.MoveDown, "MoveDown" },
            { InputKey.MoveLeft, "MoveLeft" },
            { InputKey.MoveRight, "MoveRight" },
            { InputKey.Jump, "Jump" },
            { InputKey.Melee, "Melee" },
            { InputKey.Skill1, "Skill1" },
            { InputKey.Skill2, "Skill2" },
            { InputKey.Skill3, "Skill3" },
            { InputKey.Interact, "Interact" },
            { InputKey.Pause, "Pause" },
            { InputKey.LockMove, "LockMove" },
            { InputKey.Cancel, "Cancel" },
            { InputKey.Submit, "Submit" },
        };
        public static string ToInputKeyString(this InputKey key) {
            if (enumToStringDict.TryGetValue(key, out string value)) {
                return value;
            }
            return string.Empty;
        }

    }

}