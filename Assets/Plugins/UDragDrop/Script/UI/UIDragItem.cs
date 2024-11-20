using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/UI Drag Item")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class UIDragItem : DragItemBase
    {
        [Header("UI")]
        public MaskableGraphic TargetGraphic;        
        // 调整 Transform 的 SiblingIndex 到下方确保 UI 等节点保持最上层显示
        public bool AdjustSiblingIndex = true;

        [NonSerialized] public RectTransform RectTransform;
        [NonSerialized] public Canvas Canvas;
        [NonSerialized] public UIDragEventHandler DragEventHandler;

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            Group = UDragDrop.GetGroupData(GroupKey);
            RectTransform = Transform.GetComponent<RectTransform>();
            Canvas = GetComponentInParent<Canvas>();
            DragEventHandler = TargetGraphic.GetComponent<UIDragEventHandler>();
            if (DragEventHandler == null) DragEventHandler = TargetGraphic.gameObject.AddComponent<UIDragEventHandler>();
            DragEventHandler.OnPointerDownAction += OnPointerDown;
            DragEventHandler.OnPointerUpAction += OnPointerUp;
            DragEventHandler.OnBeginDragAction += OnBeginDrag;
            DragEventHandler.OnDragAction += OnDrag;
            DragEventHandler.OnEndDragAction += OnEndDrag;
        }

        #endregion

        #region Event Handler

        public void OnPointerDown(PointerEventData eventData)
        {
            DragBeginPosition = GetCurrentPosition(eventData);
            Pickup();
            if (AdjustSiblingIndex && transform.parent != null)
            {
                transform.SetAsLastSibling();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DragEndPosition = GetCurrentPosition(eventData);
            Drop();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragBegin();
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragMovePosition = GetCurrentPosition(eventData) - DragBeginPosition;
            Drag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragEndPosition = GetCurrentPosition(eventData);
            DragEnd();
            if (AdjustSiblingIndex && BeginTransData != null && Transform.parent != null)
            {
                Transform.SetSiblingIndex(BeginTransData.SiblingIndex);
            }
        }

        #endregion

        public virtual Vector3 GetCurrentPosition(PointerEventData eventData)
        {
            var uiCamera = UDragDrop.GetUICamera();
            if (Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                var position = eventData.position;
                var screenPosition = RectTransformUtility.WorldToScreenPoint(uiCamera, position);
                var worldPosition = uiCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
                return worldPosition;
            }
            else
            {
                var position = uiCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0f));
                return position;
            }
        }

        #region Check

        public override Vector3 ClampInArea(Vector3 position)
        {
            var areaList = UDragDrop.GetGroupData(GroupKey).AreaList;
            var minDis = float.MaxValue;
            var closestPos = position;
            var pList = new List<Vector3>();
            foreach (var dragArea in areaList)
            {
                var area = dragArea as UIDragArea;
                if (area == null) continue;
                var corners = new Vector3[4];
                area.RectTransform.GetWorldCorners(corners);
                var leftBottomPos = corners[0];
                var leftTopPos = corners[1];
                var rightTopPos = corners[2];
                var rightBottomPos = corners[3];
                var centerPos = leftTopPos + (rightBottomPos - leftTopPos) / 2f;
                pList.Clear();
                var r1 = DragDropUtil.LineSegmentIntersection(position, centerPos,leftTopPos, rightTopPos, out var p1);
                var r2 = DragDropUtil.LineSegmentIntersection(position, centerPos, rightTopPos, rightBottomPos, out var p2);
                var r3 = DragDropUtil.LineSegmentIntersection(position, centerPos, leftBottomPos, rightBottomPos, out var p3);
                var r4 = DragDropUtil.LineSegmentIntersection(position, centerPos, leftTopPos, leftBottomPos, out var p4);
                if (r1) pList.Add(p1);
                if (r2) pList.Add(p2);
                if (r3) pList.Add(p3);
                if (r4) pList.Add(p4);
                foreach (var pos in pList)
                {
                    var dis = (position - pos).sqrMagnitude;
                    if (!(dis < minDis)) continue;
                    minDis = dis;
                    closestPos = pos;
                }
            }

            var result = closestPos;
            result.z = position.z;

            return result;
        }

        #endregion
    }
}