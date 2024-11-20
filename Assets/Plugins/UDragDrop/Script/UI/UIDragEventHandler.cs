using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aya.DragDrop
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class UIDragEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action<PointerEventData> OnPointerDownAction = delegate { };
        public Action<PointerEventData> OnPointerUpAction = delegate { };
        public Action<PointerEventData> OnBeginDragAction = delegate { };
        public Action<PointerEventData> OnDragAction = delegate { };
        public Action<PointerEventData> OnEndDragAction = delegate { };

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownAction?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpAction?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragAction?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragAction?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragAction?.Invoke(eventData);
        }
    }
}