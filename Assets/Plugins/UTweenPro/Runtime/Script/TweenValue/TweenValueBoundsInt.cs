using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueBoundsInt<TTarget> : Tweener<TTarget, BoundsInt>
        where TTarget : UnityEngine.Object
    {
        public BoundsIntValue From = new BoundsIntValue();
        public BoundsIntValue To = new BoundsIntValue();

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
            BoundsInt result;
            var position = Vector3.LerpUnclamped(from.position, to.position, factor);
            var size = Vector3.LerpUnclamped(from.size, to.size, factor);
            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.position = new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
                if (AxisY) result.size = new Vector3Int(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y), Mathf.RoundToInt(size.z));
            }
            else
            {
                result = new BoundsInt(new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z)), 
                    new Vector3Int(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y), Mathf.RoundToInt(size.z)));
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
            From.Reset(new BoundsInt(Vector3Int.zero, Vector3Int.zero));
            To.Reset(new BoundsInt(Vector3Int.zero, Vector3Int.one));
        }

        public override void ResetCallback()
        {
            base.ResetCallback();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueBoundsInt<TTarget> : Tweener<TTarget, BoundsInt>
        where TTarget : UnityEngine.Object
    {
        public override string AxisXName => "P";
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
