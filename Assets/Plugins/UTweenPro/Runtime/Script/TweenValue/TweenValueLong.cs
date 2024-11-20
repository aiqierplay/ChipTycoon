using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueLong<TTarget> : Tweener<TTarget, long>
        where TTarget : UnityEngine.Object
    {
        public LongValue From = new LongValue();
        public LongValue To = new LongValue();

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
            var result = (long)Math.Round(from + (to - from) * factor);
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

        public override long MinValue => 0;
        public override long MaxValue => 1;

        public override long ClampMin(long value)
        {
            return value < MinValue ? MinValue : value;
        }

        public override long ClampMax(long value)
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

    public abstract partial class TweenValueLong<TTarget> : Tweener<TTarget, long>
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