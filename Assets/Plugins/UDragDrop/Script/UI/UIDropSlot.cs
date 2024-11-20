using System;
using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/UI Drop Slot")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class UIDropSlot : DropSlotBase
    {
        [NonSerialized] public RectTransform RectTransform;

        protected override void Awake()
        {
            base.Awake();
            RectTransform = Transform.GetComponent<RectTransform>();
            if (PlacePos == null)
            {
                PlacePos = RectTransform;
            }
        }

        public override bool CheckInSlot(DragItemBase item)
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