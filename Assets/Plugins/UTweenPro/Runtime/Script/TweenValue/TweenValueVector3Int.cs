using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueVector3Int<TTarget> : Tweener<TTarget, Vector3Int>
        where TTarget : UnityEngine.Object
    {
        public Vector3IntValue From = new Vector3IntValue();
        public Vector3IntValue To = new Vector3IntValue();

        public override bool SupportIndependentAxis => true;
        public override int AxisCount => 3;

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
            Vector3Int result;
            var temp = Vector3.LerpUnclamped(from, to, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.x = Mathf.RoundToInt(temp.x);
                if (AxisY) result.y = Mathf.RoundToInt(temp.y);
                if (AxisZ) result.z = Mathf.RoundToInt(temp.z);
            }
            else
            {
                result = new Vector3Int(Mathf.RoundToInt(temp.x), Mathf.RoundToInt(temp.y), Mathf.RoundToInt(temp.z));
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
        public override Vector3Int MinValue => Vector3Int.zero;
        public override Vector3Int MaxValue => Vector3Int.one;

        public override Vector3Int ClampMin(Vector3Int value)
        {
            if (value.x < MinValue.x) value.x = MinValue.x;
            if (value.y < MinValue.y) value.y = MinValue.y;
            if (value.z < MinValue.z) value.z = MinValue.z;
            return value;
        }

        public override Vector3Int ClampMax(Vector3Int value)
        {
            if (value.x > MaxValue.x) value.x = MaxValue.x;
            if (value.y > MaxValue.y) value.y = MaxValue.y;
            if (value.z > MaxValue.z) value.z = MaxValue.z;
            return value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(Vector3Int.zero);
            To.Reset(Vector3Int.one);
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueVector3Int<TTarget> : Tweener<TTarget, Vector3Int>
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
