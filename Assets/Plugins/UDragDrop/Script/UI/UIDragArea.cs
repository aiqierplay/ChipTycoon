using System;
using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/UI Drag Area")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class UIDragArea : DragAreaBase
    {
        [NonSerialized] public RectTransform RectTransform;

        protected override void Awake()
        {
            base.Awake();
            RectTransform = Transform.GetComponent<RectTransform>();
        }

        public override bool CheckInArea(DragItemBase item)
        {
            var uiItem = item as UIDragItem;
            if (uiItem == null) return false;
            var srcRect = uiItem.RectTransform;
            var dstRect = RectTransform;
            var result = DragDropUtil.CheckInRect(srcRect, dstRect);
            return result;
        }
    }
}