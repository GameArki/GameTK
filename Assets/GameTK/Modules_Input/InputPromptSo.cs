// WIN_ENABLE_SWITCH 需要手动定义
#if (WIN_ENABLE_SWITCH) || (UNITY_SWITCH)
#define SWITCH_ENABLE
#endif

#if WIN_ENABLE_SWITCH && !UNITY_SWITCH && !UNITY_STANDALONE_LINUX
#define SWITCH_PRO_HID_ENABLE
#endif

#if !UNITY_STANDALONE_LINUX && !UNITY_SWITCH && !UNITY_EDITOR && !UNITY_ANDROID
#define PS5_HID_ENABLE
#endif

#if !UNITY_SWITCH && !UNITY_STANDALONE_LINUX && !UNITY_ANDROID
#define PS4_GAMEPAD_ENABLE
#endif

#if UNITY_STANDALONE_LINUX && !UNITY_ANDROID
#define PS4_GAMEPAD_ENABLE_LINUX
#endif

#if !UNITY_SWITCH
#define XBOX_GAMEPAD_ENABLE
#endif

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TriInspector;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem.DualShock;
#if SWITCH_ENABLE || SWITCH_PRO_HID_ENABLE
using UnityEngine.InputSystem.Switch;
#endif

namespace GameTK {

    [Serializable]
    [DeclareHorizontalGroup("KV")]
    public struct InputKeySprite {
        [Group("KV")] public string key;
        [Group("KV")] public Sprite sprite;
    }

    [CreateAssetMenu(fileName = "So_InputPrompt", menuName = "NJM/TM/InputPromptSo")]
    public class InputPromptSo : ScriptableObject {

        // ==== Binding ====
        public InputKeySprite[] keyboard_pairs;
        public InputKeySprite[] gamepad_genericPairs;
        public InputKeySprite[] gamepad_psPairs;
        public InputKeySprite[] gamepad_nsPairs;

        // ==== Runtime ====
        Dictionary<string, Sprite> dictKB;
        Dictionary<string, Sprite> dictGamepad_NS;
        Dictionary<string, Sprite> dictGamepad_PS;
        Dictionary<string, Sprite> dictGamepad_Generic;

        public void BakeDict() {
            dictKB = new Dictionary<string, Sprite>(keyboard_pairs.Length);
            foreach (var kv in keyboard_pairs) {
                string key;
                if (string.IsNullOrEmpty(kv.key)) {
                    key = KeyPathConst.NONE;
                } else {
                    key = KeyPathConst.KB + "/" + kv.key;
                }
                dictKB.Add(key, kv.sprite);
                dictKB.TryAdd(key.ToLower(), kv.sprite);
            }

            dictGamepad_Generic = new Dictionary<string, Sprite>(gamepad_genericPairs.Length);
            foreach (var kv in gamepad_genericPairs) {
                string key;
                if (string.IsNullOrEmpty(kv.key)) {
                    key = KeyPathConst.NONE;
                } else {
                    key = KeyPathConst.JS + "/" + kv.key;
                }
                dictGamepad_Generic.Add(key, kv.sprite);
                dictGamepad_Generic.TryAdd(key.ToLower(), kv.sprite);
            }

            dictGamepad_NS = new Dictionary<string, Sprite>(gamepad_nsPairs.Length);
            foreach (var kv in gamepad_nsPairs) {
                string key;
                if (string.IsNullOrEmpty(kv.key)) {
                    key = KeyPathConst.NONE;
                } else {
                    key = KeyPathConst.JS + "/" + kv.key;
                }
                dictGamepad_NS.Add(key, kv.sprite);
                dictGamepad_NS.TryAdd(key.ToLower(), kv.sprite);
            }

            dictGamepad_PS = new Dictionary<string, Sprite>(gamepad_psPairs.Length);
            foreach (var kv in gamepad_psPairs) {
                string key;
                if (string.IsNullOrEmpty(kv.key)) {
                    key = KeyPathConst.NONE;
                } else {
                    key = KeyPathConst.JS + "/" + kv.key;
                }
                dictGamepad_PS.Add(key, kv.sprite);
                dictGamepad_PS.TryAdd(key.ToLower(), kv.sprite);
            }

        }

        public bool TryGetKeyboard(string keyPath, out Sprite spr) {
            if (keyPath == null) {
                spr = null;
                return false;
            }
            return dictKB.TryGetValue(keyPath, out spr);
        }

        public bool TryGetXInput(string keyPath, out Sprite spr) {
            if (keyPath == null) {
                spr = null;
                return false;
            }
            return dictGamepad_Generic.TryGetValue(keyPath, out spr);
        }

        // 注: 别删, 用于Switch
        // 注: 别删, 用于Switch
        // 注: 别删, 用于Switch
        public bool TryGetNPad(string keyPath, out Sprite spr) {
            if (keyPath == null) {
                spr = null;
                return false;
            }
            return dictGamepad_NS.TryGetValue(keyPath, out spr);
        }

        public bool TryGetGamepad(string keyPath, InputDevice inputDevice, out Sprite spr) {
            if (keyPath == null) {
                spr = null;
                return false;
            }
            bool succ = false;
#if SWITCH_ENABLE
            if (inputDevice is UnityEngine.InputSystem.Switch.NPad ) {
                succ = TryGetNPad(keyPath, out spr);
                return succ;
            }
#endif
#if SWITCH_PRO_HID_ENABLE
            if (inputDevice is SwitchProControllerHID) {
                succ = dictGamepad_NS.TryGetValue(keyPath, out spr);
                return succ;
            }
#endif
#if PS4_GAMEPAD_ENABLE
            if (inputDevice is DualShockGamepad || inputDevice is DualShock3GamepadHID || inputDevice is DualShock4GamepadHID) {
                succ = dictGamepad_PS.TryGetValue(keyPath, out spr);
                return succ;
            }
#endif
#if PS4_GAMEPAD_ENABLE_LINUX
            if (inputDevice is DualShockGamepad || inputDevice.name.Contains("DualShock3") || inputDevice.name.Contains("DualShock4")) {
                succ = dictGamepad_PS.TryGetValue(keyPath, out spr);
                return succ;
            }
#endif
#if PS5_HID_ENABLE
            if (inputDevice is DualSenseGamepadHID || inputDevice.name.Contains("DualSense")) {
                succ = dictGamepad_PS.TryGetValue(keyPath, out spr);
                return succ;
            }
#endif
#if XBOX_GAMEPAD_ENABLE
            if (inputDevice is Gamepad || inputDevice is XInputController) {
                succ = dictGamepad_Generic.TryGetValue(keyPath, out spr);
                return succ;
            }
#endif
            succ = dictGamepad_Generic.TryGetValue(keyPath, out spr);
            return succ;
        }

        public bool TryGetCancelKeyPath(InputDevice device, out string keyPath) {
#if SWITCH_ENABLE
            if (device is UnityEngine.InputSystem.Switch.NPad) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif
            if (device is Keyboard || device is Mouse) {
                keyPath = KeyPathConst.KB_ESC;
                return true;
            }

#if PS4_GAMEPAD_ENABLE
            if (device is DualShockGamepad || device is DualShock3GamepadHID || device is DualShock4GamepadHID) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif

#if PS4_GAMEPAD_ENABLE
            if (device is DualShockGamepad || device is DualShock3GamepadHID || device is DualShock4GamepadHID) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif

#if PS4_GAMEPAD_ENABLE_LINUX
            if (device is DualShockGamepad || device.name.Contains("DualShock3") || device.name.Contains("DualShock4")) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif

#if SWITCH_PRO_HID_ENABLE
            if (device is SwitchProControllerHID) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif

#if PS5_HID_ENABLE
            if (device is DualSenseGamepadHID || device.name.Contains("DualSense")) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif

#if XBOX_GAMEPAD_ENABLE
            if (device is Gamepad || device is XInputController) {
                keyPath = KeyPathConst.JS_East;
                return true;
            }
#endif
            keyPath = KeyPathConst.JS_East;
            return true;
        }

        public bool TryGetSubmitKeyPath(InputDevice device, out string keyPath) {
#if SWITCH_ENABLE
            if (device is UnityEngine.InputSystem.Switch.NPad ) {
                keyPath = KeyPathConst.JS_East;
                return true;
            }
#endif

#if SWITCH_PRO_HID_ENABLE
            if (device is SwitchProControllerHID) {
                keyPath = KeyPathConst.JS_East;
                return true;
            }
#endif

            if (device is Keyboard || device is Mouse) {
                keyPath = KeyPathConst.KB_ENTER;
                return true;
            }
#if PS4_GAMEPAD_ENABLE
            if (device is DualShockGamepad || device is DualShock3GamepadHID || device is DualShock4GamepadHID) {
                keyPath = KeyPathConst.JS_East;
                return true;
            }
#endif

#if PS4_GAMEPAD_ENABLE_LINUX
            if (device is DualShockGamepad || device.name.Contains("DualShock3") || device.name.Contains("DualShock4")) {
                keyPath = KeyPathConst.JS_East;
                return true;
            }
#endif

#if PS5_HID_ENABLE
            if (device is DualSenseGamepadHID || device.name.Contains("DualSense")) {
                keyPath = KeyPathConst.JS_East;
                return true;
            }
#endif

#if XBOX_GAMEPAD_ENABLE
            if (device is Gamepad || device is XInputController) {
                keyPath = KeyPathConst.JS_South;
                return true;
            }
#endif
            keyPath = KeyPathConst.JS_South;
            return true;
        }

        public bool TryGet(string keyPath, InputDevice inputDevice, out Sprite spr) {
            if (keyPath == null) {
                spr = null;
                return false;
            }
            bool succ = false;
            if (inputDevice is Keyboard || inputDevice is Mouse) {
                succ = TryGetKeyboard(keyPath, out spr);
                return succ;
            } else {
                succ = TryGetGamepad(keyPath, inputDevice, out spr);
                return succ;
            }
        }

    }

}