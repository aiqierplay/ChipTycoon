using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueDouble<TTarget> : Tweener<TTarget, double>
        where TTarget : UnityEngine.Object
    {
        public DoubleValue From = new DoubleValue();
        public DoubleValue To = new DoubleValue();

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
            var result = from + (to - from) * factor;
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
        public override double MinValue => 0;
        public override double MaxValue => 1;

        public override double ClampMin(double value)
        {
            return value < MinValue ? MinValue : value;
        }

        public override double ClampMax(double value)
        {
            return value > MaxValue ? MaxValue : value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(0.0);
            To.Reset(1.0);
        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueDouble<TTarget> : Tweener<TTarget, double>
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
