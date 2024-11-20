using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu(UDragDrop.AddComponentMenuPath + "/Drag Item 2D")]
    [DisallowMultipleComponent]
    public class DragItem2D : DragItemBase
    {
        #region MonoBehaviour

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        #endregion

        #region Register / DeRegister

        public override void Register()
        {
            base.Register();
            // var dragItemList = UDragDropManager.Ins.GetDragItem2DList(GroupKey);
            // dragItemList.Add(this);
        }

        public override void DeRegister()
        {
            base.DeRegister();
            // if (UDragDropManager.Ins == null) return;
            // var dragItemList = UDragDropManager.Ins.GetDragItem2DList(GroupKey);
            // dragItemList.Remove(this);
        }

        #endregion
    }
}