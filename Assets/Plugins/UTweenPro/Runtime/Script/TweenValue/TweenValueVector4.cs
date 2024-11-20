using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueVector4<TTarget> : Tweener<TTarget, Vector4>
        where TTarget : UnityEngine.Object
    {
        public Vector4Value From = new Vector4Value();
        public Vector4Value To = new Vector4Value();

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
            Vector4 result;
            var temp = LerpUtil.LerpUnclamped(from, to, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.x = temp.x;
                if (AxisY) result.y = temp.y;
                if (AxisZ) result.z = temp.z;
                if (AxisW) result.w = temp.w;
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
        public override Vector4 MinValue => Vector4.zero;
        public override Vector4 MaxValue => Vector4.one;

        public override Vector4 ClampMin(Vector4 value)
        {
            if (value.x < MinValue.x) value.x = MinValue.x;
            if (value.y < MinValue.y) value.y = MinValue.y;
            if (value.z < MinValue.z) value.z = MinValue.z;
            if (value.w < MinValue.w) value.w = MinValue.w;
            return value;
        }

        public override Vector4 ClampMax(Vector4 value)
        {
            if (value.x > MaxValue.x) value.x = MaxValue.x;
            if (value.y > MaxValue.y) value.y = MaxValue.y;
            if (value.z > MaxValue.z) value.z = MaxValue.z;
            if (value.w > MaxValue.w) value.w = MaxValue.w;
            return value;
        }

        public override void Reset()
        {
            base.Reset(); 
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(Vector4.zero);
            To.Reset(Vector4.one);
        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueVector4<TTarget> : Tweener<TTarget, Vector4>
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
