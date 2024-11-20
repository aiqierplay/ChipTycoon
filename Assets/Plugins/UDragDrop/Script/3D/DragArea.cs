using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/Drag Area")]
    [DisallowMultipleComponent]
    public class DragArea : DragAreaBase
    {
        public override bool CheckInArea(DragItemBase item)
        {
            return true;
        }
    }
}