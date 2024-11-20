using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueQuaternion<TTarget> : Tweener<TTarget, Quaternion>
        where TTarget : UnityEngine.Object
    {
        public QuaternionValue From = new QuaternionValue();
        public QuaternionValue To = new QuaternionValue();

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
            Quaternion result;
            var temp = Quaternion.LerpUnclamped(from, to, factor);
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

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(Quaternion.identity);
            To.Reset(Quaternion.identity);
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueQuaternion<TTarget> : Tweener<TTarget, Quaternion>
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
