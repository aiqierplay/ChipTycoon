using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu("UDragDrop/Drag Item Callback")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DragItemBase))]
    public class DragItemCallback : MonoBehaviour
    {
        public Callback OnDragBegin = new Callback();
        public Callback OnDrag = new Callback();
        public Callback OnDragEnd = new Callback();

        public Callback OnPickup = new Callback();
        public Callback OnDrop = new Callback();
        public Callback<DropSlotBase> OnDropToSlot = new Callback<DropSlotBase>();

        public Callback<DropSlotBase> OnEnter = new Callback<DropSlotBase>();
        public Callback<DropSlotBase> OnMove = new Callback<DropSlotBase>();
        public Callback<DropSlotBase> OnExit = new Callback<DropSlotBase>();
    }
}