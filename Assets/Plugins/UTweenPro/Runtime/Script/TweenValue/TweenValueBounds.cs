using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueBounds<TTarget> : Tweener<TTarget, Bounds>
        where TTarget : UnityEngine.Object
    {
        public BoundsValue From = new BoundsValue();
        public BoundsValue To = new BoundsValue();

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
            Bounds result;
            var center = Vector3.LerpUnclamped(from.center, to.center, factor);
            var size = Vector3.LerpUnclamped(from.size, to.size, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.center = center;
                if (AxisY) result.size = size;
            }
            else
            {
                result = new Bounds(center, size);
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

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(new Bounds(Vector3.zero, Vector3.zero));
            To.Reset(new Bounds(Vector3.zero, Vector3.one));

        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueBounds<TTarget> : Tweener<TTarget, Bounds>
        where TTarget : UnityEngine.Object
    {
        public override string AxisXName => "C";
        public override string AxisYName => "S";

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
            FromValueRef = From;
            ToValueRef = To;
        }
    }

#endif
}
