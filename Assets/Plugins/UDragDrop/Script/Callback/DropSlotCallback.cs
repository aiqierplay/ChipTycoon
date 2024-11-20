using UnityEngine;

namespace Aya.DragDrop
{
    [AddComponentMenu("UDragDrop/Drop Slot Callback")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DropSlotBase))]
    public class DropSlotCallback : MonoBehaviour
    {
        public Callback<DragItemBase> OnPickup = new Callback<DragItemBase>();
        public Callback<DragItemBase> OnDrop = new Callback<DragItemBase>();

        public Callback<DragItemBase> OnEnter = new Callback<DragItemBase>();
        public Callback<DragItemBase> OnMove = new Callback<DragItemBase>();
        public Callback<DragItemBase> OnExit = new Callback<DragItemBase>();
    }
}