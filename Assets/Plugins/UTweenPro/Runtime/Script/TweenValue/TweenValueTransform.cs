using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueTransform<TTarget> : Tweener<TTarget, Transform>
        where TTarget : UnityEngine.Object
    {
        public TransformValue From = new TransformValue();
        public TransformValue To = new TransformValue();

        public override bool SupportSpace => true;
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
            var value = EnableFromGetter ? ValueGetter() : Value;
            if (Space == SpaceMode.World)
            {
                var result = (value.position, value.eulerAngles, value.localScale);
                if (AxisX)
                {
                    var position = Vector3.LerpUnclamped(from.position, to.position, factor);
                    result.position = position;
                }

                if (AxisY)
                {
                    var eulerAngles = Vector3.LerpUnclamped(from.eulerAngles, to.eulerAngles, factor);
                    result.eulerAngles = eulerAngles;
                }

                if (AxisZ)
                {
                    var localScale = Vector3.LerpUnclamped(from.localScale, to.localScale, factor);
                    result.localScale = localScale;
                }

                value.position = result.position;
                value.eulerAngles = result.eulerAngles;
                value.localScale = result.localScale;
            }

            if (Space == SpaceMode.Local)
            {
                var result = (value.localPosition, value.localEulerAngles, value.localScale);
                if (AxisX)
                {
                    var position = Vector3.Lerp(from.localPosition, to.localPosition, factor);
                    result.localPosition = position;
                }

                if (AxisY)
                {
                    var eulerAngles = Vector3.Lerp(from.localEulerAngles, to.localEulerAngles, factor);
                    result.localEulerAngles = eulerAngles;
                }

                if (AxisZ)
                {
                    var localScale = Vector3.Lerp(from.localScale, to.localScale, factor);
                    result.localScale = localScale;
                }

                value.localPosition = result.localPosition;
                value.localEulerAngles = result.localEulerAngles;
                value.localScale = result.localScale;
            }
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset();
            To.Reset();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueTransform<TTarget> : Tweener<TTarget, Transform>
        where TTarget : UnityEngine.Object
    {
        public override string AxisXName => "P";
        public override string AxisYName => "R";
        public override string AxisZName => "S";

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
            FromValueRef = From;
            ToValueRef = To;
        }
    }
#endif
}
