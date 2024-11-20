using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueInteger<TTarget> : Tweener<TTarget, int>
        where TTarget : UnityEngine.Object
    {
        public IntegerValue From = new IntegerValue();
        public IntegerValue To = new IntegerValue();

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
            var result = Mathf.RoundToInt(Mathf.LerpUnclamped(from, to, factor));
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
        public override int MinValue => 0;
        public override int MaxValue => 1;

        public override int ClampMin(int value)
        {
            return value < MinValue ? MinValue : value;
        }

        public override int ClampMax(int value)
        {
            return value > MaxValue ? MaxValue : value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(0);
            To.Reset(1);
        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueInteger<TTarget> : Tweener<TTarget, int>
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
