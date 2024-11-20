using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueVector2Int<TTarget> : Tweener<TTarget, Vector2Int>
        where TTarget : UnityEngine.Object
    {
        public Vector2IntValue From = new Vector2IntValue();
        public Vector2IntValue To = new Vector2IntValue();

        public override bool SupportIndependentAxis => true;
        public override int AxisCount => 2;

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
            Vector2Int result;
            var temp = Vector2.LerpUnclamped(from, to, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.x = Mathf.RoundToInt(temp.x);
                if (AxisY) result.y = Mathf.RoundToInt(temp.y);
            }
            else
            {
                result = new Vector2Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y));
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
        public override Vector2Int MinValue => Vector2Int.zero;
        public override Vector2Int MaxValue => Vector2Int.one;

        public override Vector2Int ClampMin(Vector2Int value)
        {
            if (value.x < MinValue.x) value.x = MinValue.x;
            if (value.y < MinValue.y) value.y = MinValue.y;
            return value;
        }

        public override Vector2Int ClampMax(Vector2Int value)
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
            From.Reset(Vector2Int.zero);
            To.Reset(Vector2Int.one);
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueVector2Int<TTarget> : Tweener<TTarget, Vector2Int>
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
