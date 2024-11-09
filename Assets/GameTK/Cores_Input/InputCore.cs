using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTK.Cores_Input.Internal;

namespace GameTK.Cores_Input {

    public class InputCore : MonoBehaviour {

        GeneratedInputActions inputActions;

        // **** Custom ****
        Vector2 moveAxis;
        public Vector2 MoveAxis => moveAxis;
        // **** Custom ****

        public void Ctor() {
            inputActions = new GeneratedInputActions();
        }

        public void Enable() {
            inputActions.Enable();
        }

        public void Disable() {
            inputActions.Disable();
        }

        public void Tick(float dt) {
            {
                moveAxis = inputActions.Player.Move.ReadValue<Vector2>();
            }
        }

    }

}
