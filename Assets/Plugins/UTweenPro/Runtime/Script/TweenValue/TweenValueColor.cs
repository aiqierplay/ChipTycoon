using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueColor<TTarget> : Tweener<TTarget, Color>
        where TTarget : UnityEngine.Object
    {
        public ColorMode ColorMode;
        public ColorLerpMode ColorLerpMode;
        public Gradient Gradient;

        public ColorValue From = new ColorValue();
        public ColorValue To = new ColorValue();

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
            Color result;
            Color temp;
            if (ColorMode == ColorMode.FromTo)
            {
                if (ColorLerpMode == ColorLerpMode.RGB)
                {
                    temp = Color.LerpUnclamped(from, to, factor);
                }
                else
                {
                    temp = LerpUtil.LerpColorHsvUnclamped(from, to, factor);
                }
            }
            else
            {
                temp = Gradient.Evaluate(factor);
            }

            if (EnableAxis)
            {
                result = EnableFromGetter ? ValueGetter() : Value;
                if (AxisX) result.r = temp.r;
                if (AxisY) result.g = temp.g;
                if (AxisZ) result.b = temp.b;
                if (AxisW) result.a = temp.a;
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
        public override Color MinValue => Color.clear;
        public override Color MaxValue => Color.white;

        public override Color ClampMin(Color value)
        {
            if (value.r < MinValue.r) value.r = MinValue.r;
            if (value.g < MinValue.g) value.g = MinValue.g;
            if (value.b < MinValue.b) value.b = MinValue.b;
            if (value.a < MinValue.a) value.a = MinValue.a;
            return value;
        }

        public override Color ClampMax(Color value)
        {
            if (value.r > MaxValue.r) value.r = MaxValue.r;
            if (value.g > MaxValue.g) value.g = MaxValue.g;
            if (value.b > MaxValue.b) value.b = MaxValue.b;
            if (value.a > MaxValue.a) value.a = MaxValue.a;
            return value;
        }

        public override void Reset()
        {
            base.Reset();
            FromValueRef = From;
            ToValueRef = To;
            From.Reset(Color.black);
            To.Reset(Color.white);
            ColorMode = ColorMode.FromTo;
            ColorLerpMode = ColorLerpMode.RGB;
            Gradient = new Gradient();
            Gradient.SetKeys(new[] {new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.white, 1f)}, new[] {new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f)});
        }

        public override void ReverseFromTo()
        {
            base.ReverseFromTo();
            Gradient.Reverse();
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueColor<TTarget> : Tweener<TTarget, Color>
        where TTarget : UnityEngine.Object
    {
        public override string AxisXName => "R";
        public override string AxisYName => "G";
        public override string AxisZName => "B";
        public override string AxisWName => "A";

        [TweenerProperty, NonSerialized] public SerializedProperty ColorModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty ColorLerpModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty GradientProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
            FromValueRef = From;
            ToValueRef = To;
        }

        public override void DrawFromToValue()
        {
            GUIUtil.DrawToolbarEnum(ColorModeProperty, "Mode", typeof(ColorMode));
            if (ColorMode == ColorMode.FromTo)
            {
                GUIUtil.DrawToolbarEnum(ColorLerpModeProperty, "Lerp", typeof(ColorLerpMode));
                base.DrawFromToValue();
            }

            if (ColorMode == ColorMode.Gradient)
            {
                using (GUIHorizontal.Create())
                {
                    EditorGUILayout.PropertyField(GradientProperty, new GUIContent(nameof(Color)));
                }
            }
        }
    }
#endif

    #region Extension

    public abstract partial class TweenValueColor<TTarget> : Tweener<TTarget, Color>
        where TTarget : UnityEngine.Object
    {
        public TweenValueColor<TTarget> SetColorMode(ColorMode colorMode)
        {
            ColorMode = colorMode;
            return this;
        }

        public TweenValueColor<TTarget> SetColorLerpMode(ColorLerpMode colorLerpMode)
        {
            ColorLerpMode = colorLerpMode;
            return this;
        }

        public TweenValueColor<TTarget> SetGradient(Gradient gradient)
        {
            Gradient = gradient;
            return this;
        }
    }

    #endregion
}
