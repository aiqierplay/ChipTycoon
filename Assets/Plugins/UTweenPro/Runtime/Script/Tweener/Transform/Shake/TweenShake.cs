using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Shake", nameof(Transform), -997)]
    [Serializable]
    public partial class TweenShake : TweenValueFloat<Transform>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public TweenShakeData ShakeData = new TweenShakeData();

        public override bool SupportIndependentAxis => true;
        public override bool SupportSetCurrentValue => false;
        public override bool SupportSpace => true;

        public override int AxisCount => 3;

        public override float Value { get; set; }

        public bool EnablePosition
        {
            get => AxisX;
            set => AxisX = value;
        }

        public bool EnableRotation
        {
            get => AxisY;
            set => AxisY = value;
        }

        public bool EnableScale
        {
            get => AxisZ;
            set => AxisZ = value;
        }

        public Vector3 ValuePosition
        {
            get => Space == SpaceMode.World ? Target.position : Target.localPosition;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.position = value;
                }
                else
                {
                    Target.localPosition = value;
                }
            }
        }

        public Vector3 ValueRotation
        {
            get => Space == SpaceMode.World ? Target.eulerAngles : Target.localEulerAngles;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.eulerAngles = value;
                }
                else
                {
                    Target.localEulerAngles = value;
                }
            }
        }

        public Vector3 ValueScale
        {
            get => Target.localScale;
            set => Target.localScale = value;
        }

        private Vector3 _offsetPosition;
        private Vector3 _offsetRotation;
        private Vector3 _offsetScale;

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _startScale;

        public override void PrepareSample()
        {
             base.PrepareSample();

            if (EnablePosition) _startPosition = ValuePosition;
            if (EnableRotation) _startRotation = ValueRotation;
            if (EnableScale) _startScale = ValueScale;

            _offsetPosition = ShakeData.Position.GetValue();
            _offsetRotation = ShakeData.Rotation.GetValue();
            _offsetScale = ShakeData.Scale.GetValue();
        }


        public override void LoopStart()
        {
            base.LoopStart();
            if (EnablePosition && ShakeData.Position.GetValueOnLoopStart) _offsetPosition = ShakeData.Position.GetValue();
            if (EnableRotation && ShakeData.Rotation.GetValueOnLoopStart) _startRotation = ShakeData.Rotation.GetValue();
            if (EnableScale && ShakeData.Scale.GetValueOnLoopStart) _offsetScale = ShakeData.Scale.GetValue();
        }

        public override void StopSample()
        {
            if (EnablePosition) ValuePosition = _startPosition;
            if (EnableRotation) ValueRotation = _startRotation;
            if (EnableScale) ValueScale = _startScale;
        }

        public override void Sample(float factor)
        {
            var step = 1f / ShakeData.Count;
            var power = 1f - factor;
            var curveFactor = (factor % step) / step;
            var currentFactor = ShakeData.Curve.Evaluate(curveFactor) * power;
            if (AxisX)
            {
                var position = Vector3.LerpUnclamped(_startPosition, _startPosition + _offsetPosition, currentFactor);
                ValuePosition = position;
            }

            if (AxisY)
            {
                var rotation = Vector3.LerpUnclamped(_startRotation, _startRotation + _offsetRotation, currentFactor);
                ValueRotation = rotation;
            }

            if (AxisZ)
            {
                var scale = Vector3.LerpUnclamped(_startScale, _startScale + _offsetScale, currentFactor);
                ValueScale = scale;
            }
        }

        public override void Reset()
        {
            base.Reset();
            ShakeData.Reset();
            EnablePosition = true;
            EnableRotation = false;
            EnableScale = false;
#if UNITY_EDITOR
            EnableAxis = true;
#endif
        }
    }

#if UNITY_EDITOR

    public partial class TweenShake : TweenValueFloat<Transform>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty ShakeDataProperty;

        public override string AxisXName => "P";
        public override string AxisYName => "R";
        public override string AxisZName => "S";

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            ShakeData.Tweener = this;
            base.InitEditor(index, animation, tweenerProperty);
        }

        public override void DrawIndependentAxis()
        {
        }

        public override void DrawFromToValue()
        {
            ShakeData.DrawInspector();
            base.DrawFromToValue();
        }
    }

#endif

    #region Extension

    public partial class TweenShake : TweenValueFloat<Transform>
    {
        public TweenShake SetAxis(bool enablePosition, bool enableRotation, bool enableScale)
        {
            EnablePosition = enablePosition;
            EnableRotation = enableRotation;
            EnableScale = enableScale;
            return this;
        }

        public TweenShake SetShakeData(TweenShakeData shakeData)
        {
            ShakeData = shakeData;
            return this;
        }

        public TweenShake SetShakePosition(Vector3 shakePosition)
        {
            ShakeData.Position.SetValue(shakePosition);
            EnablePosition = true;
            return this;
        }
        
        public TweenShake SetShakePosition(float shakePosition)
        {
            ShakeData.Position.SetValue(Vector3.one * shakePosition);
            EnablePosition = true;
            return this;
        }
        
        public TweenShake SetShakeRotation(Vector3 shakeRotation)
        {
            ShakeData.Rotation.SetValue(shakeRotation);
            EnableRotation = true;
            return this;
        }
        
        public TweenShake SetShakeRotation(float shakeRotation)
        {
            ShakeData.Rotation.SetValue(Vector3.one * shakeRotation);
            EnableRotation = true;
            return this;
        }
        
        public TweenShake SetShakeScale(Vector3 shakeScale)
        {
            ShakeData.Scale.SetValue(shakeScale);
            EnableScale = true;
            return this;
        }
        
        public TweenShake SetShakeScale(float shakeScale)
        {
            ShakeData.Scale.SetValue(Vector3.one * shakeScale);
            EnableScale = true;
            return this;
        }

        public TweenShake SetShakeCount(int shakeCount)
        {
            ShakeData.Count = shakeCount;
            return this;
        }

        public TweenShake SetShakeCurve(AnimationCurve shakeCurve)
        {
            ShakeData.Curve = shakeCurve;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenShake Shake(Transform transform, TweenShakeData shakeData, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetShakeData(shakeData)
                .SetAxis(true, true, true);
            return tweener;
        }

        public static TweenShake Shake(Transform transform, Vector3 shakePosition, Vector3 shakeRotation, Vector3 shakeScale, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetShakePosition(shakePosition)
                .SetShakeRotation(shakeRotation)
                .SetShakeScale(shakeScale)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        public static TweenShake Shake(Transform transform, float shakePosition, float shakeRotation, float shakeScale, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetShakePosition(shakePosition)
                .SetShakeRotation(shakeRotation)
                .SetShakeScale(shakeScale)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        public static TweenShake ShakePosition(Transform transform, Vector3 shakePosition, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetAxis(true, false, false)
                .SetShakePosition(shakePosition)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        
        public static TweenShake ShakePosition(Transform transform, float shakePosition, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetAxis(true, false, false)
                .SetShakePosition(shakePosition)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        public static TweenShake ShakeRotation(Transform transform, Vector3 shakeRotation, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetAxis(false, true, false)
                .SetShakeRotation(shakeRotation)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        public static TweenShake ShakeRotation(Transform transform, float shakeRotation, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetAxis(false, true, false)
                .SetShakeRotation(shakeRotation)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        public static TweenShake ShakeScale(Transform transform, Vector3 shakeScale, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetAxis(false, false, true)
                .SetShakeScale(shakeScale)
                .SetShakeCount(shakeCount);
            return tweener;
        }
        
        public static TweenShake ShakeScale(Transform transform, float shakeScale, int shakeCount, float duration)
        {
            var tweener = Play<TweenShake, Transform, float>(transform, 0f, 1f, duration)
                .SetAxis(false, false, true)
                .SetShakeScale(shakeScale)
                .SetShakeCount(shakeCount);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenShake Shake(this Transform transform, TweenShakeData shakeData, float duration)
        {
            var tweener = UTween.Shake(transform, shakeData, duration);
            return tweener;
        }

        public static TweenShake Shake(this Transform transform, Vector3 shakePosition, Vector3 shakeRotation, Vector3 shakeScale, int shakeCount, float duration)
        {
            var tweener = UTween.Shake(transform, shakePosition, shakeRotation, shakeScale, shakeCount, duration);
            return tweener;
        }
        
        public static TweenShake Shake(this Transform transform, float shakePosition, float shakeRotation, float shakeScale, int shakeCount, float duration)
        {
            var tweener = UTween.Shake(transform, shakePosition, shakeRotation, shakeScale, shakeCount, duration);
            return tweener;
        }
        
        public static TweenShake ShakePosition(this Transform transform, Vector3 shakePosition, int shakeCount, float duration)
        {
            var tweener = UTween.ShakePosition(transform, shakePosition, shakeCount, duration);
            return tweener;
        }
        
        
        public static TweenShake ShakePosition(this Transform transform, float shakePosition, int shakeCount, float duration)
        {
            var tweener = UTween.ShakePosition(transform, shakePosition, shakeCount, duration);
            return tweener;
        }
        
        public static TweenShake ShakeRotation(this Transform transform, Vector3 shakeRotation, int shakeCount, float duration)
        {
            var tweener = UTween.ShakeRotation(transform, shakeRotation, shakeCount, duration);
            return tweener;
        }
        
        public static TweenShake ShakeRotation(this Transform transform, float shakeRotation, int shakeCount, float duration)
        {
            var tweener = UTween.ShakeRotation(transform, shakeRotation, shakeCount, duration);
            return tweener;
        }
        
        public static TweenShake ShakeScale(this Transform transform, Vector3 shakeScale, int shakeCount, float duration)
        {
            var tweener = UTween.ShakeScale(transform, shakeScale, shakeCount, duration);
            return tweener;
        }
        
        public static TweenShake ShakeScale(this Transform transform, float shakeScale, int shakeCount, float duration)
        {
            var tweener = UTween.ShakeScale(transform, shakeScale, shakeCount, duration);
            return tweener;
        }
    }

    #endregion
}