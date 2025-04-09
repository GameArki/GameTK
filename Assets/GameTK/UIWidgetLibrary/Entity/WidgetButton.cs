using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace GameTK {

    public class WidgetButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler {

        public Action OnSelectHandle;
        public Action OnDeselectHandle;

        public void Select() {
            Button button = GetComponentInChildren<Button>();
            if (button != null) {
                button.Select();
            } else {
                Debug.LogWarning("Button component not found in children.");
            }
        }

        public void Anim_Normal() {
            Button button = GetComponentInChildren<Button>();
            if (button != null) {
                button.animator.SetTrigger("Normal");
            } else {
                Debug.LogWarning("Button component not found in children.");
            }
        }

        public void Anim_Pressed() {
            Button button = GetComponentInChildren<Button>();
            if (button != null) {
                button.animator.SetTrigger("Pressed");
            } else {
                Debug.LogWarning("Button component not found in children.");
            }
        }

        public void Anim_Disabled() {
            Button button = GetComponentInChildren<Button>();
            if (button != null) {
                button.interactable = false;
                button.animator.SetTrigger("Disabled");
            } else {
                Debug.LogWarning("Button component not found in children.");
            }
        }

        public void SetText(string text, int additionWidth = 0) {
            TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) {
                tmp.text = text;

                if (additionWidth != 0) {
                    float btnWidth = tmp.preferredWidth + additionWidth;
                    var rect = GetComponent<RectTransform>();
                    Vector2 oldSize = rect.sizeDelta;
                    rect.sizeDelta = new Vector2(btnWidth, oldSize.y);
                }
            }
        }

        #region Events
        void ISelectHandler.OnSelect(BaseEventData eventData) {
            OnSelectHandle?.Invoke();
            WidgetUtil.Sound_PlaySelect();
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData) {
            OnDeselectHandle?.Invoke();
        }

        void ISubmitHandler.OnSubmit(BaseEventData eventData) {
            WidgetUtil.Sound_PlayConfirm();
        }

        void OnDestroy() {
            OnSelectHandle = null;
            OnDeselectHandle = null;
        }
        #endregion

    }

}