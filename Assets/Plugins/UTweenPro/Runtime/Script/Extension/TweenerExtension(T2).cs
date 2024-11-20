using System;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public static partial class TweenerExtension
    {

    }

    public abstract partial class Tweener<TTarget, TValue> : Tweener<TTarget>
        where TTarget : Object
    {
        #region Set From / To / ValueGetter / ValueSetter

        public Tweener<TTarget, TValue> SetFromCurrent()
        {
            FromValueRef.Mode = TweenValueMode.Current;
            return this;
        }

        public Tweener<TTarget, TValue> SetToCurrent()
        {
            ToValueRef.Mode = TweenValueMode.Current;
            return this;
        }

        public Tweener<TTarget, TValue> SetFromRandom(TValue randomFrom, TValue randomTo)
        {
            FromValueRef.Mode = TweenValueMode.Random;
            FromValueRef.RandomFrom = randomFrom;
            FromValueRef.RandomTo = randomTo;
            return this;
        }

        public Tweener<TTarget, TValue> SetToRandom(TValue randomFrom, TValue randomTo)
        {
            ToValueRef.Mode = TweenValueMode.Random;
            ToValueRef.RandomFrom = randomFrom;
            ToValueRef.RandomTo = randomTo;
            return this;
        }

        public Tweener<TTarget, TValue> SetFromRandomEachLoop(TValue randomFrom, TValue randomTo)
        {
            FromValueRef.Mode = TweenValueMode.RandomEachLoop;
            FromValueRef.RandomFrom = randomFrom;
            FromValueRef.RandomTo = randomTo;
            return this;
        }

        public Tweener<TTarget, TValue> SetToRandomEachLoop(TValue randomFrom, TValue randomTo)
        {
            ToValueRef.Mode = TweenValueMode.RandomEachLoop;
            ToValueRef.RandomFrom = randomFrom;
            ToValueRef.RandomTo = randomTo;
            return this;
        }

        public Tweener<TTarget, TValue> SetFromGetter(Func<TValue> fromGetter)
        {
            FromValueRef.Mode = TweenValueMode.Func;
            FromValueRef.ValueGetter = fromGetter;
            return this;
        }

        public Tweener<TTarget, TValue> SetToGetter(Func<TValue> toGetter)
        {
            ToValueRef.Mode = TweenValueMode.Func;
            ToValueRef.ValueGetter = toGetter;
            return this;
        }

        public Tweener<TTarget, TValue> SetValueGetter(Func<TValue> valueGetter)
        {
            ValueGetter = valueGetter;
            return this;
        }

        public Tweener<TTarget, TValue> SetValueSetter(Action<TValue> valueSetter)
        {
            ValueSetter = valueSetter;
            return this;
        }

        #endregion

        #region Set Current <-> Value

        public Tweener<TTarget, TValue> SetCurrent2From()
        {
            SetFrom(Value);
            return this;
        }

        public Tweener<TTarget, TValue> SetFrom2Current()
        {
            Value = GetFrom();
            return this;
        }

        public Tweener<TTarget, TValue> SetCurrent2To()
        {
            SetTo(Value);
            return this;
        }

        public Tweener<TTarget, TValue> SetTo2Current()
        {
            Value = GetTo();
            return this;
        } 

        #endregion

        #region Set Axis

        public Tweener<TTarget, TValue> SetAxisX(bool enable)
        {
            AxisX = enable;
            return this;
        }

        public Tweener<TTarget, TValue> SetAxisY(bool enable)
        {
            AxisY = enable;
            return this;
        }

        public Tweener<TTarget, TValue> SetAxisZ(bool enable)
        {
            AxisZ = enable;
            return this;
        }

        public Tweener<TTarget, TValue> SetAxisW(bool enable)
        {
            AxisW = enable;
            return this;
        }

        public Tweener<TTarget, TValue> SetAxis(bool enable)
        {
            AxisX = enable;
            AxisY = enable;
            AxisZ = enable;
            AxisW = enable;
            return this;
        }

        public Tweener<TTarget, TValue> SetAxis(AxisConstraint axis)
        {
            Axis = axis;
            EnableAxis = true;
            return this;
        }

        #endregion

        #region Set Event

        public Tweener<TTarget, TValue> SetOnUpdate(Action<TValue> onUpdate)
        {
            OnUpdate += onUpdate;
            return this;
        }

        #endregion
    }
}