using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Aya.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIEventListener))]
    [AddComponentMenu("AUI/UI Event Pass")]
    public class UIEventPass : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler,
        IPointerUpHandler, ISelectHandler, IUpdateSelectedHandler, IDeselectHandler, IDropHandler, IMoveHandler,
        IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
    {
        public bool PassOnlyOnce = true;

        protected static Dictionary<GameObject, UIEventPass> UIEventPassCacheDic = new Dictionary<GameObject, UIEventPass>();

        public static HashSet<GameObject> EventPassList = new HashSet<GameObject>();

        protected void OnEnable()
        {
            EventPassList.Add(gameObject);
        }

        protected void OnDisable()
        {
            EventPassList.Remove(gameObject);
        }

        public static UIEventPass Get(GameObject go)
        {
            if (UIEventPassCacheDic.TryGetValue(go, out var ret))
            {
                return ret;
            }

            ret = go.GetComponent<UIEventPass>();
            if (ret == null)
            {
                ret = go.AddComponent<UIEventPass>();
            }

            UIEventPassCacheDic.Add(go, ret);
            return ret;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerClickHandler, PassOnlyOnce);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerDownHandler, PassOnlyOnce);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerEnterHandler, PassOnlyOnce);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerExitHandler, PassOnlyOnce);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerUpHandler, PassOnlyOnce);
        }

        public void OnSelect(BaseEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.selectHandler, PassOnlyOnce);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.updateSelectedHandler, PassOnlyOnce);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.deselectHandler, PassOnlyOnce);
        }

        public void OnDrop(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.dropHandler, PassOnlyOnce);
        }

        public void OnMove(AxisEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.moveHandler, PassOnlyOnce);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.beginDragHandler, PassOnlyOnce);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.initializePotentialDrag, PassOnlyOnce);
        }

        public void OnDrag(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.dragHandler, PassOnlyOnce);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.endDragHandler, PassOnlyOnce);
        }

        public void OnScroll(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.scrollHandler, PassOnlyOnce);
        }

        /// <summary>
        /// 事件向下透传
        /// 当执行过程中产生新UI，会导致unity卡死
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="data">消息</param>
        /// <param name="function">类型</param>
        /// <param name="onlyOne">只传一层</param>
        /// <param name="callback">回调</param>
        public void PassEvent<T>(BaseEventData data, ExecuteEvents.EventFunction<T> function, bool onlyOne = true, Action<object> callback = null) where T : IEventSystemHandler
        {
            if (data == null) return;
            var results = new List<RaycastResult>();
            var eventData = data as PointerEventData;
            if (eventData == null)
            {
                return;
            }

            EventSystem.current.RaycastAll(eventData, results);
            var current = eventData.pointerCurrentRaycast.gameObject;
            for (var i = 0; i < results.Count; i++)
            {
                var target = results[i].gameObject;
                if (current == target) continue;
                if (EventPassList.Contains(target)) continue;
                // RaycastAll 后 UGUI 会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                ExecuteEvents.Execute(target, data, function);
                callback?.Invoke(target);
                if (onlyOne)
                {
                    break;
                }
            }
        }


        /// <summary>
        /// 事件向下透传到指定组件
        /// 当执行过程中产生新UI，会导致unity卡死
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <typeparam name="TComponent">限定对象组件类型</typeparam>
        /// <param name="data">消息</param>
        /// <param name="function">类型</param>
        /// <param name="onlyOne">只传一层</param>
        /// <param name="callback">回调</param>
        public void PassEvent<T, TComponent>(BaseEventData data, ExecuteEvents.EventFunction<T> function, bool onlyOne = true, Action<TComponent> callback = null) where T : IEventSystemHandler
        {
            if (data == null) return;
            var results = new List<RaycastResult>();
            var eventData = data as PointerEventData;
            if (eventData == null) return;
            EventSystem.current.RaycastAll(eventData, results);
            var current = eventData.pointerCurrentRaycast.gameObject;
            for (var i = 0; i < results.Count; i++)
            {
                var target = results[i].gameObject;
                if (EventPassList.Contains(target)) continue;
                var com = target.GetComponent<TComponent>();
                if (com == null) continue;
                if (current == target) continue;
                // RaycastAll 后 UGUI 会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                ExecuteEvents.Execute(target, data, function);
                callback?.Invoke(com);
                if (onlyOne)
                {
                    break;
                }
            }
        }
    }
}