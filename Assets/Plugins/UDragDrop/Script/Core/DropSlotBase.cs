using System;
using UnityEngine;

namespace Aya.DragDrop
{
    [DisallowMultipleComponent]
    public abstract class DropSlotBase : DragDropBase
    {
        public DropMode DropMode = DropMode.DropEmpty;
        public DropPlaceMode PlaceMode = DropPlaceMode.OnlyPos;
        public Transform PlacePos;

        [Header("State")]
        public GameObject CanDropTip;
        public GameObject CanNotDropTip;

        [NonSerialized] public DragItemBase Item;
        [NonSerialized] public TransformData PlaceTransData;

        #region Cache

        public DropSlotCallback Callback
        {
            get
            {
                if (CallbackIns == null)
                {
                    CallbackIns = GetComponent<DropSlotCallback>();
                    if (CallbackIns == null)
                    {
                        CallbackIns = gameObject.AddComponent<DropSlotCallback>();
                    }
                }

                return CallbackIns;
            }
        }

        internal DropSlotCallback CallbackIns;

        #endregion

        public virtual void Init()
        {
            Item = null;
        }

        public virtual void ClearTipState()
        {
            if (CanDropTip != null) CanDropTip.SetActive(false);
            if (CanNotDropTip != null) CanNotDropTip.SetActive(false);
        }

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            CallbackIns = GetComponent<DropSlotCallback>();
            if (PlacePos == null) PlacePos = Transform;
        }

        protected virtual void OnEnable()
        {
            Register();
            ClearTipState();
        }

        protected virtual void OnDisable()
        {
           DeRegister();
        }

        #endregion

        #region Register / DeRegister

        public override void Register()
        {
            Group.SlotList.Add(this);
        }

        public override void DeRegister()
        {
            Group.SlotList.Remove(this);
        }

        #endregion

        #region Check

        public virtual bool CheckCanDrop(DragItemBase item)
        {
            switch (DropMode)
            {
                case DropMode.DropEmpty:
                    return Item == null;
                case DropMode.DropReplace:
                    var c1 = Item == null;
                    var c2 = Item != null && Item.CheckCanPickup() && item.FromSlot != null && item.FromSlot != this;
                    return c1 || c2;
                case DropMode.Custom:
                    return CheckCanDropCustom(item);
            }

            return true;
        }

        public virtual bool CheckCanDropCustom(DragItemBase item)
        {
            return true;
        }

        #endregion

        #region Pickup / Drop

        public virtual void OnPickup(DragItemBase item)
        {
            item.FromSlot = this;
            item.Slot = null;
            Item = null;
        }

        public virtual void OnDrop(DragItemBase item, bool immediately = false)
        {
            PlaceTransData = new TransformData(PlacePos);
            switch (DropMode)
            {
                case DropMode.DropEmpty:
                    OnDropEmpty(item, immediately);
                    break;
                case DropMode.DropReplace:
                    OnDropReplace(item, immediately);
                    break;
                case DropMode.Custom:
                    OnDropCustom(item, immediately);
                    break;
            }

            ClearTipState();
        }

        public virtual void OnDropEmpty(DragItemBase item, bool immediately = false)
        {
            item.FromSlot = null;
            item.Slot = this;
            Item = item;
            if (immediately) Place(item);
        }

        public virtual void OnDropReplace(DragItemBase item, bool immediately = false)
        {
            if (Item != null)
            {
                var fromSlot = item.FromSlot;
                var replaceItem = Item;
                replaceItem.Pickup();
                if (fromSlot != null)
                {
                    replaceItem.DropToSlot(fromSlot, immediately);
                }
            }

            item.FromSlot = null;
            item.Slot = this;
            Item = item;
            if (immediately) Place(item);
        }

        public virtual void Place(DragItemBase item)
        {
            switch (PlaceMode)
            {
                case DropPlaceMode.OnlyPos:
                    break;
                case DropPlaceMode.MoveToTransWithPos:
                    item.Transform.SetParent(PlacePos, true);
                    break;
            }
        }

        public virtual void OnDropCustom(DragItemBase item, bool immediately = false)
        {

        }

        #endregion

        #region Enter / Move / Exit

        public virtual void OnEnter(DragItemBase item)
        {
            var canDrop = CheckCanDrop(item);
            if (CanDropTip != null) CanDropTip.SetActive(canDrop);
            if (CanNotDropTip != null) CanNotDropTip.SetActive(!canDrop);
        }

        public virtual void OnMove(DragItemBase item)
        {

        }

        public virtual void OnExit(DragItemBase item)
        {
            ClearTipState();
        }

        #endregion

        #region Check

        public abstract bool CheckInSlot(DragItemBase item);

        #endregion
    }
}