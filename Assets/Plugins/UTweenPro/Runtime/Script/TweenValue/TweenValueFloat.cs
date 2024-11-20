using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueFloat<TTarget> : Tweener<TTarget, float>
        where TTarget : UnityEngine.Object
    {
        public FloatValue From = new FloatValue();
        public FloatValue To = new FloatValue();

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
            var result = SampleAngle ? LerpUtil.LerpAngleUnclamped(from, to, factor) : LerpUtil.LerpUnclamped(from, to, factor);
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
        public override float MinValue => 0f;
        public override float MaxValue => 1f;

        public override float ClampMin(float value)
        {
            return value < MinValue ? MinValue : value;
        }

        public override float ClampMax(float value)
        {
            return value > MaxValue ? MaxValue : value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(0f);
            To.Reset(1f);
        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueFloat<TTarget> : Tweener<TTarget, float>
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
