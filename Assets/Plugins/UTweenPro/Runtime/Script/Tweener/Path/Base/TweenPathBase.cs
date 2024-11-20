using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public abstract partial class TweenPathBase : TweenValueFloat<Transform>
    {
        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public bool KeepForward;
        public bool PreCache;

        public bool CanUseRealtimeSample => !KeepForward && !Animation.SpeedBased;

        [NonSerialized] public float Precision;
        public const float DefaultPrecision = 0.01f;

        public override bool SupportSetCurrentValue => false;
        public override bool SupportSpeedBased => true;
        public override bool SupportGizmos => true;

        public override float Value { get; set; }

        public Vector3 ValuePosition
        {
            get => Target.position;
            set => Target.position = value;
        }

        public Vector3 ValueEulerAngle
        {
            get => Target.eulerAngles;
            set => Target.eulerAngles = value;
        }

        public override float GetSpeedBasedDuration()
        {
            RefreshCache();
            var speed = Duration;
            var duration = Length / speed;
            return duration;
        }

        public override void Sample(float factor)
        {
            factor = Mathf.Clamp01(factor);
            if (Animation.SpeedBased)
            {
                SampleByFactorSpeedBased(factor, out var position, out var eulerAngle);
                ValuePosition = position;
                if (KeepForward) ValueEulerAngle = eulerAngle;
            }
            else
            {
                if (!KeepForward && !PreCache)
                {
                    var position = GetPositionByFactor(factor);
                    ValuePosition = position;
                }
                else
                {
                    SampleByFactor(factor, out var position, out var eulerAngle);
                    ValuePosition = position;
                    ValueEulerAngle = eulerAngle;
                }
            }
        }

        #region Sample Impl
        
        protected bool IsCached = false;
        protected List<TweenPathCachePoint> CachePointList = new List<TweenPathCachePoint>();
        protected int CachePointCount;
        protected float Length;

        public virtual void ClearCache()
        {
            IsCached = false;
            CachePointCount = 0;
            Length = 0f;
            CachePointList.Clear();
        }

        public virtual void RefreshCache(bool forceRefresh = false)
        {
            if (IsCached && !forceRefresh) return;
            var length = 0f;
            CachePointList.Clear();
            if (Precision < 1e-6f) Precision = DefaultPrecision;
            for (var factor = 0f; factor <= 1f; factor += Precision)
            {
                var point = GetPositionByFactor(factor);
                CachePointList.Add(new TweenPathCachePoint()
                {
                    Position = point
                });
            }

            CachePointCount = CachePointList.Count;
            for (var i = 0; i < CachePointCount - 1; i++)
            {
                var p1 = CachePointList[i];
                var p2 = CachePointList[i + 1];
                var distance = Vector3.Distance(p1.Position, p2.Position);
                var forward = (p2.Position - p1.Position).normalized;
                if (forward.sqrMagnitude < 1e-6f)
                {
                    p1.EulerAngle = Vector3.zero;
                }
                else
                {
                    p1.EulerAngle = Quaternion.LookRotation(forward).eulerAngles;
                }

                p2.Distance = distance;
                p2.Length = length;
                length += distance;

                if (i == 0)
                {
                    p1.Distance = 0;
                    p1.Length = 0;
                }
            }

            IsCached = true;
            Length = length;
        }

        public virtual void SampleByFactorSpeedBased(float factor, out Vector3 position, out Vector3 eulerAngle)
        {
            RefreshCache();
            factor = Mathf.Clamp01(factor);
            var targetDistance = factor * Length;
            var distanceCounter = 0f;
            for (var i = 0; i < CachePointCount - 1; i++)
            {
                var distance = CachePointList[i].Distance;
                if (distanceCounter + distance >= targetDistance)
                {
                    var p1 = CachePointList[i];
                    var p2 = CachePointList[i + 1];
                    var diff = targetDistance - distanceCounter;
                    var delta = diff / distance;
                    TweenPathCachePoint.Lerp(p1, p2, delta, out position, out eulerAngle);
                    return;
                }

                distanceCounter += distance;
            }

            var point = CachePointList[CachePointCount - 1];
            position = point.Position;
            eulerAngle = point.EulerAngle;
        }

        public virtual void SampleByFactor(float factor, out Vector3 position, out Vector3 eulerAngle)
        {
            RefreshCache();
            factor = Mathf.Clamp01(factor);
            var factorCounter = 0f;
            var each = 1f / (CachePointCount - 1);
            for (var i = 0; i < CachePointCount - 1; i++)
            {
                factorCounter += each;
                if (factorCounter >= factor)
                {
                    var p1 = CachePointList[i];
                    var p2 = CachePointList[i + 1];
                    var delta = (factorCounter - factor) / each;
                    TweenPathCachePoint.Lerp(p1, p2, delta, out position, out eulerAngle);
                    return;
                }
            }

            var point = CachePointList[CachePointCount - 1];
            position = point.Position;
            eulerAngle = point.EulerAngle;
        }

        public abstract Vector3 GetPositionByFactor(float factor); 

        #endregion

        public override void Reset()
        {
            base.Reset();

            ClearCache();

            KeepForward = false;
            PreCache = false;
            Precision = DefaultPrecision;
        }

        protected Vector3 StartPosition;

        public override void RecordObject()
        {
            StartPosition = ValuePosition;
        }

        public override void RestoreObject()
        {
            ValuePosition = StartPosition;
        }

        public override void OnDrawGizmos()
        {
            if (Target == null) return;

            Gizmos.color = Color.green;
            if (!IsCached || CachePointList.Count < 2)
            {
                RefreshCache(true);
            }

            for (var i = 0; i < CachePointCount - 1; i++)
            {
                var p1 = CachePointList[i];
                var p2 = CachePointList[i + 1];
                Gizmos.DrawLine(p1.Position, p2.Position);
            }
        }
    }

#if UNITY_EDITOR

        public abstract partial class TweenPathBase : TweenValueFloat<Transform>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty KeepForwardProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty PreCacheProperty;

        public override void DrawAppend()
        {
            base.DrawAppend();
            using (GUIHorizontal.Create())
            {
                GUIUtil.DrawToggleButton(KeepForwardProperty);
                using (GUIEnableArea.Create(CanUseRealtimeSample))
                {
                    GUIUtil.DrawToggleButton(PreCacheProperty);
                }

                if (!CanUseRealtimeSample) PreCacheProperty.boolValue = true;
            }

            if (Animation.HasObjectChanged && !Animation.IsInProgress)
            {
                IsCached = false;
            }
        }
    }

#endif

    #region Extensnion

    public static partial class TweenPathExtension
    {
        public static TTweener SetKeepForward<TTweener>(this TTweener tweener, bool keepForward = true) where TTweener : TweenPathBase
        {
            tweener.KeepForward = keepForward;
            if (keepForward)
            {
                tweener.PreCache = true;
            }

            return tweener;
        }

        public static TTweener SetPreCache<TTweener>(this TTweener tweener, bool preCache = true) where TTweener : TweenPathBase
        {
            tweener.PreCache = preCache;
            if (tweener.KeepForward)
            {
                tweener.PreCache = true;
            }

            return tweener;
        }

        public static TTweener SetPrecision<TTweener>(this TTweener tweener, float precision) where TTweener : TweenPathBase
        {
            tweener.Precision = precision;
            return tweener;
        }
    }

    #endregion

}
