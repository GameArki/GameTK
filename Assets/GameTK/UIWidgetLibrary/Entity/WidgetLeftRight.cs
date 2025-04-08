using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameTK {

    public class WidgetLeftRight : MonoBehaviour {

        [SerializeField] public Button btn_full;

        [SerializeField] TextMeshProUGUI txt_label; // 外侧的标签

        [SerializeField] TextMeshProUGUI txt_value; // 中间的值
        [SerializeField] Slider slider_value; // 中间的值

        [SerializeField] Button btn_minus; // -
        [SerializeField] Button btn_add; // +
        [SerializeField] WidgetButton ele_btn;

        public Action OnFullClickHandle;
        public Action OnLeftClickHandle;
        public Action OnRightClickHandle;

        public Action OnSelectHandle;
        public Action OnSetValueHandle;

        public void Ctor() {
            // btn_full.onClick.AddListener(() => {
            //     OnFullClickHandle?.Invoke();
            // });
            // btn_minus.onClick.AddListener(() => {
            //     OnLeftClickHandle?.Invoke();
            // });
            // btn_add.onClick.AddListener(() => {
            //     OnRightClickHandle?.Invoke();
            // });
            ele_btn.OnSelectHandle = () => {
                OnSelectHandle?.Invoke();
            };
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void SetLabel(string label) {
            txt_label.text = label;
        }

        public bool IsInteractable() {
            return btn_full.interactable && btn_minus.interactable && btn_add.interactable;
        }

        public void SetInteractable(bool isInteractable) {
            btn_minus.interactable = isInteractable;
            btn_add.interactable = isInteractable;
            btn_full.interactable = isInteractable;
        }

        /// <summary>
        /// mode: 0-文本, 1-Slider
        /// </summary>
        public void SetValue(byte mode, string txtValue, float sliderValue, bool isReachMax, bool isReachMin) {

            if (mode == 0) {
                txt_value.enabled = true;
                if (txt_value.text != txtValue) {
                    WidgetUtil.Sound_PlayValueChange();
                }
                txt_value.text = txtValue;
                slider_value.enabled = false;
            } else if (mode == 1) {
                txt_value.enabled = false;
                slider_value.enabled = true;
                if (slider_value.value != sliderValue) {
                    WidgetUtil.Sound_PlayValueChange();
                }
                slider_value.value = sliderValue;
            }

            if (isReachMax) {
                btn_add.interactable = false;
                btn_minus.interactable = true;
            } else if (isReachMin) {
                btn_add.interactable = true;
                btn_minus.interactable = false;
            } else {
                btn_add.interactable = true;
                btn_minus.interactable = true;
            }

            OnSetValueHandle?.Invoke();

        }

    }

}