using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameTK {

    public class WidgetButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler {

        public Action OnSelectHandle;
        public Action OnDeselectHandle;

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
        
    }

}