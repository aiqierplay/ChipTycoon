using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueBoolean<TTarget> : Tweener<TTarget, bool>
        where TTarget : UnityEngine.Object
    {
        public BooleanValue From = new BooleanValue();
        public BooleanValue To = new BooleanValue();

        public override void PrepareSample()
        {
            FromValueRef = From;
            ToValueRef = To;
            base.PrepareSample();
        }

        public override void Sample(float factor)
        {
            var from = (EnableFromGetter ? From.ValueGetter() : FromValue) ? 1f : 0f;
            var to = (EnableToGetter ? To.ValueGetter() : ToValue) ? 1f : 0f;
            var result = Mathf.LerpUnclamped(from, to, factor) > 0.5f;
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

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(false);
            To.Reset(true);
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueBoolean<TTarget> : Tweener<TTarget, bool>
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
