using System;
using UnityEngine;
using RectOffset = UnityEngine.RectOffset;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueRectOffset<TTarget> : Tweener<TTarget, RectOffset>
        where TTarget : UnityEngine.Object
    {
        public RectOffsetValue From = new RectOffsetValue();
        public RectOffsetValue To = new RectOffsetValue();

        public override bool SupportIndependentAxis => true;
        public override int AxisCount => 4;

        public override void PrepareSample()
        {
            FromValueRef = From;
            ToValueRef = To;
            base.PrepareSample();
        }

        public override void Sample(float factor)
        {
            var from = EnableFromGetter ? From.ValueGetter() : FromValue;
            var to = EnableToGetter ? To.ValueGetter() : ToValue;
            RectOffset result;
            var left = Mathf.RoundToInt(Mathf.LerpUnclamped(from.left, to.left, factor));
            var right = Mathf.RoundToInt(Mathf.LerpUnclamped(from.right, to.right, factor));
            var top = Mathf.RoundToInt(Mathf.LerpUnclamped(from.top, to.top, factor));
            var bottom = Mathf.RoundToInt(Mathf.LerpUnclamped(from.bottom, to.bottom, factor));
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.left = left;
                if (AxisY) result.right = right;
                if (AxisZ) result.top = top;
                if (AxisW) result.bottom = bottom;
            }
            else
            {
                result = new RectOffset(left, right, top, bottom);
            }

            if (EnableValueSetter)
            {
                ValueSetter.Invoke(result);
            }
            else
            {
                Value = result;
            }

            OnUpdate?.Invoke(result);
        }

        public override bool SupportClampValue => true;

        public override RectOffset ClampMin(RectOffset value)
        {
            if (value.left < MinValue.left) value.left = MinValue.left;
            if (value.right < MinValue.right) value.right = MinValue.right;
            if (value.top < MinValue.top) value.top = MinValue.top;
            if (value.bottom < MinValue.bottom) value.bottom = MinValue.bottom;
            return value;
        }

        public override RectOffset ClampMax(RectOffset value)
        {
            if (value.left > MaxValue.left) value.left = MaxValue.left;
            if (value.right > MaxValue.right) value.right = MaxValue.right;
            if (value.top > MaxValue.top) value.top = MaxValue.top;
            if (value.bottom > MaxValue.bottom) value.bottom = MaxValue.bottom;
            return value;
        }

        public override void Reset()
        {
            base.Reset(); 
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(new RectOffset());
            To.Reset(new RectOffset(1, 1, 1, 1));
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueRectOffset<TTarget> : Tweener<TTarget, RectOffset>
        where TTarget : UnityEngine.Object
    {
        public override string AxisXName => "L";
        public override string AxisYName => "R";
        public override string AxisZName => "T";
        public override string AxisWName => "B";

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
            FromValueRef = From;
            ToValueRef = To;
        }
    }
#endif
}
