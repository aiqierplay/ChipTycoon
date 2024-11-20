using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueRectInt<TTarget> : Tweener<TTarget, RectInt>
        where TTarget : UnityEngine.Object
    {
        public RectIntValue From = new RectIntValue();
        public RectIntValue To = new RectIntValue();

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
            RectInt result;
            var pos = Vector2.LerpUnclamped(from.position, to.position, factor);
            var size = Vector2.LerpUnclamped(from.size, to.size, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.x = Mathf.RoundToInt(pos.x);
                if (AxisY) result.y = Mathf.RoundToInt(pos.y);
                if (AxisZ) result.width = Mathf.RoundToInt(size.x);
                if (AxisW) result.height = Mathf.RoundToInt(size.y);
            }
            else
            {
                result = new RectInt(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
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

        public override RectInt ClampMin(RectInt value)
        {
            if (value.x < MinValue.x) value.x = MinValue.x;
            if (value.y < MinValue.y) value.y = MinValue.y;
            if (value.width < MinValue.width) value.width = MinValue.width;
            if (value.height < MinValue.height) value.height = MinValue.height;
            return value;
        }

        public override RectInt ClampMax(RectInt value)
        {
            if (value.x > MaxValue.x) value.x = MaxValue.x;
            if (value.y > MaxValue.y) value.y = MaxValue.y;
            if (value.width > MaxValue.width) value.width = MaxValue.width;
            if (value.height > MaxValue.height) value.height = MaxValue.height;
            return value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset();
            To.Reset();
        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueRectInt<TTarget> : Tweener<TTarget, RectInt>
        where TTarget : UnityEngine.Object
    {
        public override string AxisZName => "W";
        public override string AxisWName => "H";

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
            FromValueRef = From;
            ToValueRef = To;
        }
    }
#endif
}
