using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.DragDrop
{
    [DisallowMultipleComponent]
    public abstract class DragItemBase : DragDropBase
    {
        public DragTargetMode TargetMode = DragTargetMode.Slot;
        public DropFailMode FailMode = DropFailMode.Restore;

        public Transform PickupTrans;
        public TransformData DragOffset;
        public DragLerpData Lerp;

        [NonSerialized] public DragItemState ItemState;
        [NonSerialized] public DragItemLerpState LerpState;

        // 当前所处槽位
        [NonSerialized] public DropSlotBase Slot;

        // 本次移动前来自于哪个槽位
        [NonSerialized] public DropSlotBase FromSlot;

        // 当前移动经过的槽位
        [NonSerialized] public DropSlotBase MoveSlot;

        // 本次拖动存在的限制区域
        [NonSerialized] public List<DragAreaBase> CurrentDragAreaList;


        #region Callback

        public DragItemCallback Callback
        {
            get
            {
                if (CallbackIns == null)
                {
                    CallbackIns = GetComponent<DragItemCallback>();
                    if (CallbackIns == null)
                    {
                        CallbackIns = gameObject.AddComponent<DragItemCallback>();
                    }
                }

                return CallbackIns;
            }
        }

        internal DragItemCallback CallbackIns;

        #endregion

        public virtual void Init(DropSlotBase initSlot = null)
        {
            ItemState = DragItemState.Idle;
            LerpState = DragItemLerpState.Idle;
            DragTimer = 0f;
            DropTimer = 0f;
            Slot = null;
            FromSlot = null;
            MoveSlot = null;
            CurrentDragAreaList = null;
            if (initSlot != null)
            {
                DropToSlot(initSlot, true);
            }
        }

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            CallbackIns = GetComponent<DragItemCallback>();
        }

        protected virtual void OnEnable()
        {
            Register();
        }

        public virtual void LateUpdate()
        {
            UpdateImpl();
        }

        protected virtual void OnDisable()
        {
            DeRegister();
        }

        #endregion

        #region Register / DeRegister

        public override void Register()
        {
            Group.ItemList.Add(this);
        }

        public override void DeRegister()
        {
            Group.ItemList.Remove(this);
        }

        #endregion

        #region State

        public virtual void RecordState()
        {
            BeginTransData.CopyFrom(Transform);
        }

        public virtual void RestoreState()
        {
            if (BeginTransData == null) return;
            BeginTransData.CopyTo(Transform);
        }

        #endregion

        #region Lerp

        [NonSerialized] public TransformData BeginTransData = new TransformData();
        [NonSerialized] public TransformData FromTransData = new TransformData();
        [NonSerialized] public TransformData TempTransData = new TransformData();

        // 拖动计时器
        [NonSerialized] public float DragTimer;
        // 放置计时器
        [NonSerialized] public float DropTimer;

        public virtual void UpdateImpl()
        {
            FromTransData.CopyFrom(Transform);
            var from = FromTransData;
            if (ItemState != DragItemState.Idle)
            {
                DragTimer += Lerp.DeltaTime;
            }

            if (LerpState == DragItemLerpState.Lerp)
            {
                DropTimer += Lerp.DeltaTime;
            }

            switch (ItemState)
            {
                case DragItemState.Idle:
                    break;
                case DragItemState.Pickup:
                    {
                        var factor = DragTimer / Lerp.LerpDuration;
                        TransformData.Lerp(Lerp.LerpType, TransformData.Zero, DragOffset, factor, TempTransData);
                        TransformData.Add(BeginTransData, TempTransData, TempTransData);
                        TempTransData.CopyTo(Transform);
                    }
                    break;
                case DragItemState.Drag:
                    {
                        DragLastPosition = Transform.position;
                        var factor = DragTimer / Lerp.LerpDuration;
                        TransformData.Lerp(Lerp.LerpType, TransformData.Zero, DragOffset, factor, TempTransData);
                        TransformData.Add(BeginTransData, TempTransData, TempTransData);
                        TempTransData.Position += DragMovePosition;
                        TempTransData.CopyTo(Transform);

                        if (!CheckInArea())
                        {
                            var clampPos = ClampInArea(Transform.position);
                            Transform.position = clampPos;
                        }
                    }
                    break;
                case DragItemState.DropAnyWhere:
                    {
                        var to = from;
                        from.EulerAngle = BeginTransData.EulerAngle;
                        from.LocalScale = BeginTransData.LocalScale;
                        DropLerpUpdateImpl(from, to);
                    }
                    break;
                case DragItemState.DropToSlot:
                    {
                        var to = Slot.PlaceTransData;
                        DropLerpUpdateImpl(from, to);
                    }
                    break;
                case DragItemState.DropBackToStart:
                    {
                        var to = BeginTransData;
                        DropLerpUpdateImpl(from, to);
                    }
                    break;
            }
        }

        public virtual void DropLerpUpdateImpl(TransformData from, TransformData to)
        {
            if (LerpState == DragItemLerpState.Idle)
            {
                DropTimer = 0f;
                if (Lerp.LerpType == LerpType.None)
                {
                    to.CopyTo(Transform);
                    DropEnd();
                    return;
                }

                LerpState = DragItemLerpState.Lerp;
            }

            var factor = DropTimer / Lerp.LerpDuration;
            if (factor >= 1f)
            {
                DropEnd();
                if (Slot != null) Slot.Place(this);
            }
            else
            {
                TransformData.Lerp(Lerp.LerpType, from, to, factor, TempTransData);
                TempTransData.CopyTo(Transform);
            }
        }

        #endregion

        #region Pickup / Drop / Drag

        // 用于缓存不同模式下记录到的拖动开始，偏移量，结束值，上一次拖动到达的位置
        // 需要统一转换到世界坐标使用
        [NonSerialized] protected Vector3 DragBeginPosition;
        [NonSerialized] protected Vector3 DragMovePosition;
        [NonSerialized] protected Vector3 DragEndPosition;
        [NonSerialized] protected Vector3 DragLastPosition;

        public virtual void Pickup()
        {
            if (!CheckCanPickup()) return;
            if (PickupTrans != null) Transform.SetParent(PickupTrans, true);
            RecordState();
            ItemState = DragItemState.Pickup;
            DragTimer = 0f;
            CurrentDragAreaList = UDragDrop.GetGroupData(GroupKey).AreaList;

            if (Slot != null)
            {
                var slot = Slot;
                slot.OnPickup(this);
                if (slot.CallbackIns != null) slot.CallbackIns.OnPickup.Invoke(this);
            }

            if (CallbackIns != null) CallbackIns.OnPickup.Invoke();
        }

        public virtual void Drop()
        {
            if (ItemState != DragItemState.Pickup && ItemState != DragItemState.Drag) return;
            if (CallbackIns != null) CallbackIns.OnDrop.Invoke();
            if (ItemState != DragItemState.Drag)
            {
                ItemState = DragItemState.Drag;
                DragEnd();
            }
        }

        public virtual void DragBegin()
        {
            if (ItemState != DragItemState.Pickup) return;
            ItemState = DragItemState.Drag;

            if (CallbackIns != null) CallbackIns.OnDragBegin.Invoke();
        }

        public virtual void Drag()
        {
            if (ItemState != DragItemState.Drag) return;
            var slot = FindDropSlot();
            if (MoveSlot != null)
            {
                if (MoveSlot != slot)
                {
                    Exit(MoveSlot);
                }
                else
                {
                    Move(slot);
                }
            }
            else
            {
                if (slot != null)
                {
                    Enter(slot);
                }
            }

            if (CallbackIns != null) CallbackIns.OnDrag.Invoke();
        }

        // 返回值表示是否成功落在设定的区域，或者槽位
        public virtual void DragEnd()
        {
            if (ItemState != DragItemState.Drag) return;
            CurrentDragAreaList = null;
            if (MoveSlot != null)
            {
                Exit(MoveSlot);
            }

            var slot = FindDropSlot();
            if (slot != null && slot.CheckCanDrop(this))
            {
                DropToSlot(slot);
            }
            else if (TargetMode == DragTargetMode.Free)
            {
                ItemState = DragItemState.DropAnyWhere;
            }
            else
            {
                // Fail
                switch (FailMode)
                {
                    case DropFailMode.None:
                        ItemState = DragItemState.DropAnyWhere;
                        break;
                    case DropFailMode.Restore:
                        if (FromSlot != null && FromSlot.CheckCanDrop(this))
                        {
                            DropToSlot(FromSlot);
                        }
                        else
                        {
                            ItemState = DragItemState.DropBackToStart;
                        }

                        break;
                }
            }

            if (CallbackIns != null) CallbackIns.OnDragEnd.Invoke();
        }

        #endregion

        #region Drop To

        public virtual void DropEnd()
        {
            LerpState = DragItemLerpState.Idle;
            ItemState = DragItemState.Idle;

            if (Slot != null)
            {
                if (CallbackIns != null) CallbackIns.OnDropToSlot.Invoke(Slot);
                if (Slot.CallbackIns != null) Slot.CallbackIns.OnDrop.Invoke(this);
            }
        }

        public virtual void DropToSlot(DropSlotBase slot, bool immediately = false)
        {
            if (BeginTransData == null) RecordState();
            ItemState = DragItemState.DropToSlot;
            slot.OnDrop(this, immediately);
            if (immediately)
            {
                var target = slot.PlaceTransData;
                target.CopyTo(Transform);
                DropEnd();
            }
        }

        #endregion

        #region Enter / Move / Exit

        public virtual void Enter(DropSlotBase slot)
        {
            MoveSlot = slot;
            slot.OnEnter(this);

            if (CallbackIns != null) CallbackIns.OnEnter.Invoke(slot);
            if (slot.CallbackIns != null) slot.CallbackIns.OnEnter.Invoke(this);
        }

        public virtual void Move(DropSlotBase slot)
        {
            slot.OnMove(this);

            if (CallbackIns != null) CallbackIns.OnMove.Invoke(slot);
            if (slot.CallbackIns != null) slot.CallbackIns.OnMove.Invoke(this);
        }

        public virtual void Exit(DropSlotBase slot)
        {
            MoveSlot = null;
            slot.OnExit(this);

            if (CallbackIns != null) CallbackIns.OnExit.Invoke(slot);
            if (slot.CallbackIns != null) slot.CallbackIns.OnExit.Invoke(this);
        }

        #endregion

        #region Check

        public virtual bool CheckCanPickup()
        {
            var c1 = LerpState == DragItemLerpState.Idle;
            var c2 = ItemState == DragItemState.Idle;
            return c1 && c2;
        }

        public virtual DropSlotBase FindDropSlot()
        {
            var dropSlotList = Group.SlotList;
            foreach (var slot in dropSlotList)
            {
                var isInArea = slot.CheckInSlot(this);
                if (isInArea) return slot;
            }

            return default;
        }

        public virtual bool CheckInArea()
        {
            if (TargetMode == DragTargetMode.Free) return true;
            if (CurrentDragAreaList == null) return true;
            if (CurrentDragAreaList.Count == 0) return true;
            foreach (var area in CurrentDragAreaList)
            {
                if (area.CheckInArea(this)) return true;
            }

            return false;
        }

        public virtual Vector3 ClampInArea(Vector3 position)
        {
            return DragLastPosition;
        }

        #endregion
    }
}