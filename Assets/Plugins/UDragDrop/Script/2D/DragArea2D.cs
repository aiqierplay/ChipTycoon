using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/Drag Area 2D")]
    [DisallowMultipleComponent]
    public class DragArea2D : DragAreaBase
    {
        public override bool CheckInArea(DragItemBase item)
        {
            return true;
        }
    }
}