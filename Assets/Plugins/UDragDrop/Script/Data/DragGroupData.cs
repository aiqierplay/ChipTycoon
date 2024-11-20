using System;
using System.Collections.Generic;

namespace Aya.DragDrop
{
    [Serializable]
    public class DragGroupData
    {
        public string GroupKey;

        public List<DropSlotBase> SlotList = new List<DropSlotBase>();
        public List<DragItemBase> ItemList = new List<DragItemBase>();
        public List<DragAreaBase> AreaList = new List<DragAreaBase>();

        public DragGroupData(string groupKey)
        {
            GroupKey = groupKey;
        }
    }
}