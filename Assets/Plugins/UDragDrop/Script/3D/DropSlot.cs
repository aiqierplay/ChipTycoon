using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/Drop Slot")]
    [DisallowMultipleComponent]
    public class DropSlot : DropSlotBase
    {
        #region Check
       
        public override bool CheckInSlot(DragItemBase item)
        {
            var dragItem = item as DragItem;
            if (dragItem == null) return false;
            var slot = dragItem.FindDropSlot();
            return slot == this;
        } 

        #endregion
    }
}