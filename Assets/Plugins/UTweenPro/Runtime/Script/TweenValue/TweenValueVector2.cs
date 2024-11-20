using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueVector2<TTarget> : Tweener<TTarget, Vector2>
        where TTarget : UnityEngine.Object
    {
        public Vector2Value From = new Vector2Value();
        public Vector2Value To = new Vector2Value();

        public override bool SupportIndependentAxis => true;
        public override int AxisCount => 2;
        public virtual bool SampleAngle => false;

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
            Vector2 result;
            var temp = SampleAngle ? LerpUtil.LerpAngleUnclamped(from, to, factor) : LerpUtil.LerpUnclamped(from, to, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.x = temp.x;
                if (AxisY) result.y = temp.y;
            }
            else
            {
                result = temp;
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
        public override Vector2 MinValue => Vector2.zero;
        public override Vector2 MaxValue => Vector2.one;

        public override Vector2 ClampMin(Vector2 value)
        {
            if (value.x < MinValue.x) value.x = MinValue.x;
            if (value.y < MinValue.y) value.y = MinValue.y;
            return value;
        }

        public override Vector2 ClampMax(Vector2 value)
        {
            if (value.x > MaxValue.x) value.x = MaxValue.x;
            if (value.y > MaxValue.y) value.y = MaxValue.y;
            return value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(Vector2.zero);
            To.Reset(Vector2.one);
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueVector2<TTarget> : Tweener<TTarget, Vector2>
        where TTarget : UnityEngine.Object
    {
        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
            FromValueRef = From;
            ToValueRef = To;
        }
    }
#endif
}
