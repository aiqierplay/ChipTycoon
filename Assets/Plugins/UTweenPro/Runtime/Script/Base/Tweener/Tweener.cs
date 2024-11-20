using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class Tweener
    {
        [NonSerialized] public int InstanceID;

        public bool Active;
        public float Duration;
        public float Delay;
        public bool HoldStart;
        public bool HoldEnd;
        public int Ease;
        public float Strength;
        public EaseCurveMode CurveMode;
        public AnimationCurve EditCurve;
        public AnimationCurve Curve;
        public SpaceMode Space;

        [SerializeField] internal bool FoldOut = true;
        [SerializeField] internal DurationMode DurationMode = DurationMode.DurationDelay;

        public virtual bool SupportTarget => true;
        public virtual bool SupportFromTo => true;
        public virtual bool SupportSpace => false;
        public virtual bool SupportSpeedBased => false;
        public virtual bool SupportSetCurrentValue => true;
        public virtual bool SupportGizmos => false;
        public virtual bool SupportIndependentAxis => false;
        public virtual int AxisCount => 1;

        public bool IsPrepared { get; set; }
        public bool SingleMode => Animation != null && Animation.SingleMode;
        public float TotalDuration => Delay + Duration;

        protected Tweener()
        {
            InstanceID = UTween.InstanceIDCounter;
            UTween.InstanceIDCounter++;
        }

        #region Cache

        [NonSerialized] public TweenAnimation Animation;
        [NonSerialized] public float Factor;

        public float DurationFrom
        {
            get => Delay;
            set
            {
                if (value < 0) Delay = 0;
                else Delay = value;
            }
        }

        public float DurationTo
        {
            get => Delay + Duration;
            set
            {
                if (value > Delay)
                {
                    Duration = value - Delay;
                }
                else
                {
                    if (value < 0)
                    {
                        Delay = 0;
                    }
                    else
                    {
                        Delay = value;
                    }

                    Duration = 0;
                }
            }
        }


        public float DurationFromNormalized
        {
            get => DurationFrom / Animation.Duration;
            set => DurationFrom = Animation.Duration * value;
        }

        public float DurationToNormalized
        {
            get => DurationTo / Animation.Duration;
            set => DurationTo = Animation.Duration * value;
        }

        public EaseFunction CacheEaseFunction
        {
            get
            {
                if (_cacheEaseFunction == null || _cacheEaseFunction.Type != Ease)
                {
                    _cacheEaseFunction = EaseType.FunctionDic[Ease];
                }

                return _cacheEaseFunction;
            }
        }

        private EaseFunction _cacheEaseFunction;

        public bool IsCustomCurve => Ease < 0;

        #endregion

        #region Internal Property

        // TODO.. 用于确保子动画能结束在最终状态的临时解决方案，待替换实现
        internal bool IsCurrentLoopFinished;

        #endregion

        #region Loop
       
        public virtual void PrepareSample()
        {
            IsPrepared = true;
            IsCurrentLoopFinished = false;
            var _ = CacheEaseFunction;
        }

        public virtual void StopSample()
        {

        }

        public virtual void LoopStart()
        {

        }

        public virtual void LoopEnd()
        {

        } 

        #endregion

        public virtual float GetSpeedBasedDuration()
        {
            return Duration;
        }

        public virtual float GetFactor(float normalizedDuration, out bool valid)
        {
            var currentDuration = Animation.Duration * normalizedDuration;
            if (currentDuration < 0)
            {
                valid = false;
                return 0f;
            }

            var singleMode = Animation.CacheSingleMode;
            float delta;
            if (!singleMode && currentDuration < Delay)
            {
                delta = 0f;
                if (!IsCurrentLoopFinished && Animation.Forward)
                {
                    IsCurrentLoopFinished = true;
                }
                
                if (!HoldStart)
                {
                    Factor = delta;
                    valid = false;
                    return delta;
                }
            }
            else if (!singleMode && currentDuration > Delay + Duration)
            {
                delta = 1f;
                if (!IsCurrentLoopFinished && Animation.Forward)
                {
                    IsCurrentLoopFinished = true;
                }
                
                if (!HoldEnd)
                {
                    Factor = delta;
                    valid = false;
                    return delta;
                }
            }
            else
            {
                delta = (currentDuration - Delay) / Duration;
            }

            if (_cacheEaseFunction == null)
            {
                _cacheEaseFunction = CacheEaseFunction;
            }

            var factor = Ease < 0 ? Curve.Evaluate(delta) : _cacheEaseFunction.Ease(0f, 1f, delta, Strength);
            Factor = factor;
            valid = true;
            return factor;
        }

        public abstract void Sample(float factor);

        // Editor Only
        public virtual void SetDirty()
        {

        }

        // Editor Only
        public virtual void RecordObject()
        {

        }

        // Editor Only
        public virtual void RestoreObject()
        {

        }

        public virtual void OnAdded()
        {

        }

        public virtual void OnRemoved()
        {

        }

        protected static Keyframe[] DefaultKeyFrames = new[] { new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1) };

        public virtual void Reset()
        {
            ResetCallback();
            IsPrepared = false;
            Active = true;
            Duration = 1f;
            Delay = 0f;
            HoldStart = true;
            HoldEnd = false;

            Ease = EaseType.Linear;
            Strength = 1f;
            if (Curve == null)
            {
                Curve = new AnimationCurve(DefaultKeyFrames);
            }
            else
            {
                Curve.keys = DefaultKeyFrames;
            }

            if (EditCurve == null)
            {
                EditCurve = new AnimationCurve(DefaultKeyFrames);
            }
            else
            {
                EditCurve.keys = DefaultKeyFrames;
            }

            CurveMode = EaseCurveMode.TimePosition;
            Space = SpaceMode.World;

            if (Animation != null)
            {
                if (Animation.ControlMode == TweenControlMode.Component)
                {
                    Duration = Animation.Duration;
                }
                else
                {
                    Animation = null;
                }
            }
        }

        public virtual void ResetCallback()
        {

        }

        public virtual void ReverseFromTo()
        {

        }

        public virtual void OnDrawGizmos()
        {

        }
    }
}
