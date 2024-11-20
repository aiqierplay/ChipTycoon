using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class Tweener<TTarget, TValue> : Tweener<TTarget>
        where TTarget : UnityEngine.Object
    {
        #region From / To

        // Access to subclass From / To
        [NonSerialized] public TweenValue<TValue> FromValueRef;
        [NonSerialized] public TweenValue<TValue> ToValueRef;

        // Cache current running From / To Value
        [NonSerialized] public TValue FromValue;
        [NonSerialized] public TValue ToValue;

        public Tweener<TTarget, TValue> SetFrom(TValue from)
        {
            FromValueRef.Value = from;
            return this;
        }

        public TValue GetFrom()
        {
            if (FromValueRef.Mode == TweenValueMode.Current) return Value;
            return FromValueRef.GetValue();
        }

        public Tweener<TTarget, TValue> SetTo(TValue to)
        {
            ToValueRef.Value = to;
            return this;
        }

        public TValue GetTo()
        {
            if (ToValueRef.Mode == TweenValueMode.Current) return Value;
            return ToValueRef.GetValue();
        }

        #endregion

        [NonSerialized] public Func<TValue> ValueGetter;
        [NonSerialized] public Action<TValue> ValueSetter;

        public Action<TValue> OnUpdate;

        protected bool EnableFromGetter;
        protected bool EnableToGetter;
        protected bool EnableValueGetter;
        protected bool EnableValueSetter;

        #region Axis Constraint

        public AxisConstraint Axis;
        public bool EnableAxis = false;

        public bool AxisX
        {
            get => !EnableAxis || (Axis & AxisConstraint.X) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.X;
                }
                else
                {
                    Axis &= ~AxisConstraint.X;
                    EnableAxis = true;
                }
            }
        }

        public bool AxisY
        {
            get => !EnableAxis || (Axis & AxisConstraint.Y) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.Y;
                }
                else
                {
                    Axis &= ~AxisConstraint.Y;
                    EnableAxis = true;
                }
            }
        }

        public bool AxisZ
        {
            get => !EnableAxis || (Axis & AxisConstraint.Z) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.Z;
                }
                else
                {
                    Axis &= ~AxisConstraint.Z;
                    EnableAxis = true;
                }
            }
        }

        public bool AxisW
        {
            get => !EnableAxis || (Axis & AxisConstraint.W) > 0;
            set
            {
                if (value)
                {
                    Axis |= AxisConstraint.W;
                }
                else
                {
                    Axis &= ~AxisConstraint.W;
                    EnableAxis = true;
                }
            }
        }

        #endregion

        public abstract TValue Value { get; set; }
        public virtual TValue RecordValue { get; set; }

        #region Clamp Value

        public virtual bool SupportClampValue => false;
        public virtual bool RequireClampMin => false;
        public virtual TValue MinValue => default(TValue);

        public virtual TValue ClampMin(TValue value)
        {
            return default;
        }

        public virtual bool RequireClampMax => false;
        public virtual TValue MaxValue => default(TValue);

        public virtual TValue ClampMax(TValue value)
        {
            return default;
        }

        #endregion

        public override void PrepareSample()
        {
            base.PrepareSample();

            if (FromValueRef.GetValueOnPreSample) FromValue = GetFrom();
            if (ToValueRef.GetValueOnPreSample) ToValue = GetTo();

            EnableFromGetter = FromValueRef.Mode == TweenValueMode.Func;
            EnableToGetter = ToValueRef.Mode == TweenValueMode.Func;
            EnableValueGetter = ValueGetter != null;
            EnableValueSetter = ValueSetter != null;
        }

        public override void LoopStart()
        {
            base.LoopStart();
            if (FromValueRef.GetValueOnLoopStart) FromValue = GetFrom();
            if (ToValueRef.GetValueOnLoopStart) ToValue = GetTo();
        }

        public override void RecordObject()
        {
            RecordValue = Value;
        }

        public override void RestoreObject()
        {
            Value = RecordValue;
        }

        public virtual void DisableIndependentAxis()
        {
            Axis = AxisConstraint.All;
            EnableAxis = false;
        }

        public override void Reset()
        {
            base.Reset();
            ResetGetterSetter();
            DisableIndependentAxis();
        }

        public virtual void ResetGetterSetter()
        {
            ValueGetter = null;
            ValueSetter = null;

            EnableFromGetter = false;
            EnableToGetter = false;
            EnableValueGetter = false;
            EnableValueSetter = false;
        }

        public override void ReverseFromTo()
        {
            TweenValue<TValue>.Switch(FromValueRef, ToValueRef);
        }

        public override void ResetCallback()
        {
            OnUpdate = null;
        }
    }
}