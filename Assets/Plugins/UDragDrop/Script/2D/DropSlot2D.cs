using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/Drop Slot 2D")]
    [DisallowMultipleComponent]
    public class DropSlot2D : DropSlotBase
    {
        public override bool CheckInSlot(DragItemBase item)
        {
            return true;
        }
    }
}