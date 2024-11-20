using System;
using Excel.Log;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueVector3<TTarget> : Tweener<TTarget, Vector3>
        where TTarget : UnityEngine.Object
    {
        public AngleMode AngleMode;

        public Vector3Value From = new Vector3Value();
        public Vector3Value To = new Vector3Value();

        public override bool SupportIndependentAxis => true;
        public virtual bool SampleAngle => false;
        public override int AxisCount => 3;

        internal bool CacheSampleAngle;

        public override void PrepareSample()
        {
            FromValueRef = From;
            ToValueRef = To;
            base.PrepareSample();

            CacheSampleAngle = SampleAngle;
        }

        public override void Sample(float factor)
        {
            var from = EnableFromGetter ? From.ValueGetter() : FromValue;
            var to = EnableToGetter ? To.ValueGetter() : ToValue;
            Vector3 result;
            Vector3 temp;
            if (!CacheSampleAngle)
            {
                temp = LerpUtil.LerpUnclamped(from, to, factor);
            }
            else
            {
                if (AngleMode == AngleMode.Clamp360)
                {
                    var fromQ = Quaternion.Euler(from);
                    var toQ = Quaternion.Euler(to);
                    temp = Quaternion.LerpUnclamped(fromQ, toQ, factor).eulerAngles;
                }
                else
                {
                    temp = Vector3.LerpUnclamped(from, to, factor);
                }
            }

            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.x = temp.x;
                if (AxisY) result.y = temp.y;
                if (AxisZ) result.z = temp.z;
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
        public override Vector3 MinValue => Vector3.zero;
        public override Vector3 MaxValue => Vector3.one;

        public override Vector3 ClampMin(Vector3 value)
        {
            if (value.x < MinValue.x) value.x = MinValue.x;
            if (value.y < MinValue.y) value.y = MinValue.y;
            if (value.z < MinValue.z) value.z = MinValue.z;
            return value;
        }

        public override Vector3 ClampMax(Vector3 value)
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
            From.Reset(Vector3.zero);
            To.Reset(Vector3.one);
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueVector3<TTarget> : Tweener<TTarget, Vector3>
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
