using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using GameTK;
using GameFunctions;

namespace NJM {

    public class InputModule : MonoBehaviour {

        [SerializeField] AssetReferenceT<InputPromptSo> inputPromptSORef;
        InputPromptSo inputPromptSO;

        InputModuleInputActions player1;
        InputActionRebindingExtensions.RebindingOperation rebindOpHandle;

        Dictionary<InputKey, InputAction> keyToActionMap;

        Vector2 moveAxis;
        public Vector2 MoveAxis => moveAxis;

        bool isLockMove;
        public bool IsLockMove => isLockMove;

        Vector2 aimAxis;
        public Vector2 AimAxis => aimAxis;

        public Vector2 uiAxis;
        public Vector2Int UIAxisInt => new Vector2Int((int)uiAxis.x.ToOne(), (int)uiAxis.y.ToOne());

        // - 下落减缓(长按保持跳得更高)
        [SerializeField] float fallingRaiseAxis;
        public float FallingRaiseAxis => fallingRaiseAxis;

        // - 解决连跳问题
        bool isJumpDown;
        public bool IsJump => isJumpDown;

        bool isJumpUp;
        public bool IsJumpUp => isJumpUp;

        float meleeAxis;
        public float MeleeAxis => meleeAxis;
        public bool IsDialogueDown => meleeAxis > 0;

        // 技能1
        float skillorAxis1;
        public float SkillorAxis1 => skillorAxis1;

        // 技能2
        float skillorAxis2;
        public float SkillorAxis2 => skillorAxis2;

        float skillorAxis3;
        public float SkillorAxis3 => skillorAxis3;

        // Confirm
        public bool isConfirm;
        public void Confirm_Reset() {
            Reset_ConfirmCancelInteractSkipGroup();
        }

        // Cancle;
        public bool isCancel;
        public void Cancel_Reset() {
            Reset_ConfirmCancelInteractSkipGroup();
        }

        bool isInteract;
        public bool IsInteract => isInteract;
        public void Interact_Reset() {
            Reset_ConfirmCancelInteractSkipGroup();
        }

        public bool isPause;
        public void Pause_Reset() {
            Reset_ConfirmCancelInteractSkipGroup();
        }

        public bool isGamepadDisconnect;
        public bool isSteamOverlayActive;

        void Reset_ConfirmCancelInteractSkipGroup() {
            isInteract = false;
            isCancel = false;
            isConfirm = false;
            isPause = false;
        }

        public bool LastDeviceIsKB => lastInputDevice is Keyboard || lastInputDevice is Mouse;
        InputDevice currentInputDevice;
        InputDevice lastInputDevice;

        // 检测按键
        bool isLoaded;

        public void Ctor() {
            player1 = new InputModuleInputActions();
            player1.Enable();

            currentInputDevice = null;
            foreach (var device in InputSystem.devices) {
                if (device is Keyboard) {
                    currentInputDevice = device;
                    break;
                }
            }

            InputSystem.onDeviceChange += (d, change) => {
                if (d is Gamepad gamepad && change == InputDeviceChange.Disconnected) {
                    isGamepadDisconnect = true;
                }
            };

            var world = player1.World;
            var ui = player1.UI;
            keyToActionMap = new Dictionary<InputKey, InputAction> {
                { InputKey.MoveLeft, world.MoveLeft },
                { InputKey.MoveRight, world.MoveRight },
                { InputKey.MoveUp, world.MoveUp },
                { InputKey.MoveDown, world.MoveDown },
                { InputKey.Melee, world.Melee },
                { InputKey.Jump, world.Jump },
                { InputKey.Skill1, world.Skill1 },
                { InputKey.Skill2, world.Skill2 },
                { InputKey.Skill3, world.Skill3 },
                { InputKey.Interact, world.Interact },
                { InputKey.Pause, ui.Pause },
                {InputKey.LockMove,world.LockMove},
            };
        }

        public void TearDown() {
            rebindOpHandle?.Dispose();
            rebindOpHandle = null;
        }

        public async Task LoadAll() {
            inputPromptSO = await inputPromptSORef.LoadAssetAsync<InputPromptSo>().Task;
            inputPromptSO.BakeDict();
            isLoaded = true;
        }

        public void DefaultBind() {
            player1.RemoveAllBindingOverrides();
        }

        public void RestoreDefault() {
            DefaultBind();
        }

        void CheckAndSetCurrentDevice() {
            var world = player1.World.Get().actions;
            var ui = player1.UI.Get().actions;

            for (int i = 0; i < world.Count; i += 1) {
                var action = world[i];
                if (action.triggered) {
                    var control = action.activeControl;
                    if (control != null) {
                        if (control.device is Mouse) {
                            continue;
                        }
                        currentInputDevice = control.device;
                    }
                }
            }

            if (lastInputDevice != currentInputDevice) {
                lastInputDevice = currentInputDevice;
            }
        }

        public bool CurrentInputDeviceIsKeyboard() {
            return currentInputDevice is Keyboard || currentInputDevice is Mouse;
        }

        public InputDevice GetCurrentInputDevice() {
            return currentInputDevice;
        }

        public void Process(float dt) {

            if (!isLoaded) {
                return;
            }

            var world = player1.World;

            CheckAndSetCurrentDevice();

            // - Move
            {
                moveAxis.x = world.MoveRight.ReadValue<float>() + world.MoveLeft.ReadValue<float>();
                moveAxis.y = world.MoveUp.ReadValue<float>() + world.MoveDown.ReadValue<float>();
                if (moveAxis != Vector2.zero) {
                    moveAxis.Normalize();

                    float angle = Mathf.Atan2(moveAxis.y, moveAxis.x) * Mathf.Rad2Deg;
                    // Lock 8 directions
                    if (angle < 0) {
                        angle += 360;
                    }
                    if (angle >= 0 && angle < 22.5f) {
                        moveAxis = new Vector2(1, 0);
                    } else if (angle >= 22.5f && angle < 45 + 22.5f) {
                        moveAxis = new Vector2(1, 1);
                    } else if (angle >= 67.5f && angle < 112.5f) {
                        moveAxis = new Vector2(0, 1);
                    } else if (angle >= 112.5f && angle < 157.5f) {
                        moveAxis = new Vector2(-1, 1);
                    } else if (angle >= 157.5f && angle < 202.5f) {
                        moveAxis = new Vector2(-1, 0);
                    } else if (angle >= 202.5f && angle < 247.5f) {
                        moveAxis = new Vector2(-1, -1);
                    } else if (angle >= 247.5f && angle < 292.5f) {
                        moveAxis = new Vector2(0, -1);
                    } else if (angle >= 292.5f && angle < 337.5f) {
                        moveAxis = new Vector2(1, -1);
                    } else if (angle >= 337.5f && angle < 360) {
                        moveAxis = new Vector2(1, 0);
                    }

                    moveAxis.Normalize();
                }

                aimAxis = moveAxis;

                Vector2 rsAim = world.Aim.ReadValue<Vector2>();
                if (rsAim != Vector2.zero) {
                    aimAxis = rsAim;
                }

            }

            // - LockMove
            {
                isLockMove = world.LockMove.ReadValue<float>() != 0;
            }

            // - FallingRaise
            {
                fallingRaiseAxis = world.Jump.ReadValue<float>();
            }

            // - Jump
            {
                // - Record Continuous Jump
                isJumpDown = world.Jump.WasPressedThisFrame();
                isJumpUp = world.Jump.WasReleasedThisFrame();
            }

            // - Melee
            {
                bool isMeleeDown = world.Melee.WasPressedThisFrame();
                if (isMeleeDown) {
                    meleeAxis = 1;
                } else {
                    meleeAxis = 0;
                }
            }

            // - Skill1
            {
                bool isSkill1Down = world.Skill1.WasPressedThisFrame();
                if (isSkill1Down) {
                    skillorAxis1 = 1;
                } else {
                    skillorAxis1 = 0;
                }
            }

            // - skill2
            {
                bool isSkill2Down = world.Skill2.WasPressedThisFrame();
                if (isSkill2Down) {
                    skillorAxis2 = 1;
                } else {
                    skillorAxis2 = 0;
                }
            }

            // - skill3
            {
                bool isSkill3Down = world.Skill3.WasPressedThisFrame();
                if (isSkill3Down) {
                    skillorAxis3 = 1;
                } else {
                    skillorAxis3 = 0;
                }
            }

            // - Interact
            {
                bool isKBDown = world.Interact.WasPressedThisFrame();
                isInteract |= isKBDown;
            }

            var ui = player1.UI;
            // - UI: Move
            {
                uiAxis = Vector2.zero;
                if (ui.Navigate.WasPressedThisFrame()) {
                    uiAxis = ui.Navigate.ReadValue<Vector2>();
                } else {
                    if (world.MoveUp.WasPressedThisFrame()) {
                        uiAxis.y = 1;
                    } else if (world.MoveDown.WasPressedThisFrame()) {
                        uiAxis.y = -1;
                    } else if (world.MoveLeft.WasPressedThisFrame()) {
                        uiAxis.x = -1;
                    } else if (world.MoveRight.WasPressedThisFrame()) {
                        uiAxis.x = 1;
                    }
                }

            }

            // - UI: Confirm
            {
                if (rebindOpHandle == null) {
                    isConfirm = ui.Submit.WasPressedThisFrame();
                }
            }

            // - UI: Cancel
            {
                isCancel = ui.Cancel.WasPressedThisFrame();
            }

            // - UI: Pause
            {
                isPause = ui.Pause.WasPressedThisFrame();
            }

        }

        public bool IsInputKeyDown(InputKey key) {
            if (keyToActionMap.TryGetValue(key, out var action)) {
                return action.IsPressed();
            }
            return false;
        }

        public bool TryGetKeyToAction(InputKey key, out InputAction action) {
            return keyToActionMap.TryGetValue(key, out action);
        }

        public void Reset() {
            moveAxis = Vector2.zero;
            fallingRaiseAxis = 0;
            // isContinousJump = false;
            // continousJumpMaintainSec = 0;
            meleeAxis = 0;
            skillorAxis1 = 0;
            skillorAxis2 = 0;
            isInteract = false;
            isConfirm = false;
            isPause = false;
            isCancel = false;
        }

        // According to settings, to show tips
        public bool TryGetFirstBindingKeyPath(InputKey key, out string keyPath) {
            string keyName = key.ToInputKeyString();
            if (string.IsNullOrEmpty(keyName)) {
                keyPath = "";
                return false;
            }

            InputAction action = player1.FindAction(keyName);
            if (action == null || action.bindings.Count == 0) {
                keyPath = "";
                Debug.LogWarning("InputEntity.TryGetFirstBinding: Action not found: " + keyName);
                return false;
            }

            bool isKeyboardOrMouse = currentInputDevice is Keyboard || currentInputDevice is Mouse;
            string group = isKeyboardOrMouse ? KeyPathConst.DEVICE_KB : KeyPathConst.DEVICE_JS;
            foreach (var binding in action.bindings) {
                keyPath = binding.effectivePath;
                if (binding.groups == group) {
                    return true;
                } else if (binding.groups.Contains(group)) {
                    if (keyPath.Contains("Cancel")) {
                        bool exist = inputPromptSO.TryGetCancelKeyPath(currentInputDevice, out keyPath);
                        if (exist) {
                            return true;
                        }
                    } else if (keyPath.Contains("Submit")) {
                        bool exist = inputPromptSO.TryGetSubmitKeyPath(currentInputDevice, out keyPath);
                        if (exist) {
                            return true;
                        }
                    }
                }
            }

            keyPath = "";
            return false;
        }

        public bool TryGetFirstBindingSprite(InputKey key, out Sprite sprite) {
            TryGetFirstBindingKeyPath(key, out var keyPath);
            bool succ = inputPromptSO.TryGet(keyPath, currentInputDevice, out sprite);
            return succ;
        }

        public bool TryGetActionDisplaySprites(bool isKeyboard, InputKey key, out int binding_index_0, out InputBinding binding_0, out Sprite displaySprite_0, out int binding_index_1, out InputBinding binding_1, out Sprite displaySprite_1) {
            binding_index_0 = -1;
            binding_0 = default;
            displaySprite_0 = null;

            binding_index_1 = -1;
            binding_1 = default;
            displaySprite_1 = null;

            var world = player1.World;
            string keyName = key.ToInputKeyString();
            InputAction action = player1.FindAction(keyName);
            if (action == null || action.bindings.Count == 0) {
                Debug.LogWarning("InputEntity.TryGetFirstBinding: Action not found: " + keyName);
                return false;
            }

            string group = isKeyboard ? KeyPathConst.DEVICE_KB : KeyPathConst.DEVICE_JS;
            var bindings = action.bindings;
            for (int i = 0; i < bindings.Count; i += 1) {
                var binding = bindings[i];
                if (binding.groups != group) {
                    continue;
                }
                if (displaySprite_0 == null) {
                    binding_index_0 = i;
                    binding_0 = binding;
                    bool succ = TryGetDisplaySpriteByPath(isKeyboard, binding, out displaySprite_0);
                    if (!succ) {
                        displaySprite_0 = null;
                    }
                } else if (displaySprite_1 == null) {
                    binding_index_1 = i;
                    binding_1 = binding;
                    bool succ = TryGetDisplaySpriteByPath(isKeyboard, binding, out displaySprite_1);
                    if (!succ) {
                        displaySprite_1 = null;
                    }
                }
                if (displaySprite_0 != null && displaySprite_1 != null) {
                    return true;
                }
            }
            return true;
        }

        // 0x33
        bool TryGetDisplaySpriteByPath(bool isKB, InputBinding binding, out Sprite sprite) {
            bool succ = false;
            string keyPath = binding.effectivePath;
            if (isKB) {
                succ = inputPromptSO.TryGetKeyboard(keyPath, out sprite);
                if (!succ) {
                    Debug.LogWarning("InputEntity.TryGetDisplaySpriteByPath: Keyboard not found: " + keyPath + " " + keyPath.Length);
                }
            } else {
                if (currentInputDevice is Mouse || currentInputDevice is Keyboard) {
                    succ = inputPromptSO.TryGetXInput(keyPath, out sprite);
                } else {
                    succ = inputPromptSO.TryGet(keyPath, currentInputDevice, out sprite);
                }
#if SWITCH_ENABLE
                else if (inputDevice is UnityEngine.InputSystem.Switch.NPad || inputDevice is SwitchProControllerHID) {
                    succ = inputPromptSO.TryGetNPad(keyPath, out sprite);
                    return succ;
                }
#endif
            }
            return succ;
        }

        public bool IsAllowBinding(string keyPath) {
            bool allow = inputPromptSO.TryGet(keyPath, currentInputDevice, out var sprite);
            allow &= keyPath != KeyPathConst.JS_Start;
            allow &= keyPath != KeyPathConst.JS_Select;
            allow &= keyPath != KeyPathConst.JS_LS_Press;
            allow &= keyPath != KeyPathConst.KB_ENTER;
            allow &= keyPath != KeyPathConst.KB_ESC;
            return allow;
        }

        public bool IsConflict(InputAction action, string keyPath) {
            foreach (var other in keyToActionMap.Values) {
                if (other == action) {
                    continue;
                }
                var bindings = other.bindings;
                for (int i = 0; i < bindings.Count; i += 1) {
                    var binding = bindings[i];
                    if (binding.effectivePath == keyPath) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Forbid_Confirm(bool isForbid) {
            if (isForbid) {
                player1.UI.Submit.Disable();
            } else {
                player1.UI.Submit.Enable();
            }
        }

        // ==== Save ====
        public string ToJson() {
            return player1.SaveBindingOverridesAsJson();
        }

        public bool FromJson(string json) {
            bool isSucc = true;
            try {
                player1.LoadBindingOverridesFromJson(json);
            } catch (Exception ex) {
                isSucc = false;
                Debug.LogError("InputEntity.FromJson: " + ex.Message);
                DefaultBind();
            }
            return isSucc;
        }

    }

}