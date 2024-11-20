using System;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class TweenerExtension
    {
        #region Set Property
        
        public static TTweener SetActive<TTweener>(this TTweener tweener, bool active) where TTweener : Tweener
        {
            tweener.Active = active;
            return tweener;
        }

        public static TTweener SetDuration<TTweener>(this TTweener tweener, float duration) where TTweener : Tweener
        {
            tweener.Duration = duration;
            if (tweener.SingleMode)
            {
                tweener.Animation.Duration = tweener.TotalDuration;
            }

            return tweener;
        }

        public static TTweener SetDelay<TTweener>(this TTweener tweener, float delay) where TTweener : Tweener
        {
            tweener.Delay = delay;
            if (tweener.SingleMode)
            {
                tweener.Animation.Duration = tweener.TotalDuration;
            }

            return tweener;
        }

        public static TTweener SetHoldStart<TTweener>(this TTweener tweener, bool holdStart) where TTweener : Tweener
        {
            tweener.HoldStart = holdStart;
            return tweener;
        }

        public static TTweener SetHoldEnd<TTweener>(this TTweener tweener, bool holdEnd) where TTweener : Tweener
        {
            tweener.HoldEnd = holdEnd;
            return tweener;
        }

        public static TTweener SetEase<TTweener>(this TTweener tweener, int easeType) where TTweener : Tweener
        {
            var easeFunction = EaseType.FunctionDic[easeType];
            tweener.Ease = easeType;
            tweener.Strength = easeFunction.DefaultStrength;
            return tweener;
        }

        public static TTweener SetStrength<TTweener>(this TTweener tweener, float strength) where TTweener : Tweener
        {
            tweener.Strength = Mathf.Clamp01(strength);
            return tweener;
        }

        public static TTweener SetCurve<TTweener>(this TTweener tweener, AnimationCurve curve) where TTweener : Tweener
        {
            return SetCurve(tweener, curve, EaseCurveMode.TimePosition);
        }
        public static TTweener SetCurve<TTweener>(this TTweener tweener, AnimationCurve curve, EaseCurveMode curveMode) where TTweener : Tweener
        {
            tweener.Ease = EaseType.Custom;
            tweener.CurveMode = curveMode;
            if (tweener.EditCurve == null)
            {
                tweener.EditCurve = new AnimationCurve();
            }
            else
            {
                while (tweener.EditCurve.length > 0)
                {
                    tweener.EditCurve.RemoveKey(0);
                }
            }

            for (var i = 0; i < curve.length; i++)
            {
                tweener.EditCurve.AddKey(curve[i]);
            }

            switch (curveMode)
            {
                case EaseCurveMode.TimePosition:
                    tweener.Curve = tweener.EditCurve;
                    break;
                case EaseCurveMode.TimeVelocity:
                    tweener.Curve = AnimationCurveUtil.ConvertTv2Tp(tweener.EditCurve);
                    break;
                case EaseCurveMode.TimeAcceleration:
                    tweener.Curve = AnimationCurveUtil.ConvertTa2Tp(tweener.EditCurve);
                    break;
            }

            return tweener;
        }


        public static TTweener SetSpace<TTweener>(this TTweener tweener, SpaceMode spaceMode) where TTweener : Tweener
        {
            if (!tweener.SupportSpace) return tweener;
            tweener.Space = spaceMode;
            return tweener;
        }

        #endregion

        #region Set Animation Property

        public static TTweener SetOwner<TTweener>(this TTweener tweener, object owner) where TTweener : Tweener
        {
            tweener.Animation.SetOwner(owner);
            return tweener;
        }

        public static TTweener SetIdentifier<TTweener>(this TTweener tweener, string identifier) where TTweener : Tweener
        {
            tweener.Animation.SetIdentifier(identifier);
            return tweener;
        }

        public static TTweener SetAnimationDuration<TTweener>(this TTweener tweener, float duration) where TTweener : Tweener
        {
            tweener.Animation.SetDuration(duration);
            return tweener;
        }

        public static TTweener SetAnimationDelay<TTweener>(this TTweener tweener, float delay) where TTweener : Tweener
        {
            tweener.Animation.SetDelay(delay);
            return tweener;
        }
        public static TTweener SetBackward<TTweener>(this TTweener tweener, bool backward) where TTweener : Tweener
        {
            tweener.Animation.Backward = backward;
            return tweener;
        }

        public static TTweener SetPlayMode<TTweener>(this TTweener tweener, PlayMode playMode) where TTweener : Tweener
        {
            tweener.Animation.SetPlayMode(playMode);
            return tweener;
        }

        public static TTweener SetPlayCount<TTweener>(this TTweener tweener, int playCount) where TTweener : Tweener
        {
            tweener.Animation.SetPlayCount(playCount);
            return tweener;
        }

        public static TTweener SetPlayOnce<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation.SetPlayOnce();
            return tweener;
        }

        public static TTweener SetLoopCount<TTweener>(this TTweener tweener, int playCount) where TTweener : Tweener
        {
            tweener.Animation.SetLoopCount(playCount);
            return tweener;
        }

        public static TTweener SetLoopUnlimited<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation.SetLoopUnlimited();
            return tweener;
        }

        public static TTweener SetPingPongCount<TTweener>(this TTweener tweener, int playCount) where TTweener : Tweener
        {
            tweener.Animation.SetPingPongCount(playCount);
            return tweener;
        }

        public static TTweener SetPingPongUnlimited<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation.SetPingPongUnlimited();
            return tweener;
        }

        public static TTweener SetAutoPlay<TTweener>(this TTweener tweener, AutoPlayMode autoPlay) where TTweener : Tweener
        {
            tweener.Animation.SetAutoPlay(autoPlay);
            return tweener;
        }

        public static TTweener SetUpdateMode<TTweener>(this TTweener tweener, UpdateMode updateMode) where TTweener : Tweener
        {
            tweener.Animation.SetUpdateMode(updateMode);
            return tweener;
        }

        public static TTweener SetInterval<TTweener>(this TTweener tweener, float interval) where TTweener : Tweener
        {
            tweener.Animation.SetInterval(interval);
            return tweener;
        }

        public static TTweener SetInterval<TTweener>(this TTweener tweener, float interval, float interval2) where TTweener : Tweener
        {
            tweener.Animation.SetInterval(interval, interval2);
            return tweener;
        }

        public static TTweener SetTimeMode<TTweener>(this TTweener tweener, TimeMode timeMode) where TTweener : Tweener
        {
            tweener.Animation.SetTimeMode(timeMode);
            return tweener;
        }

        public static TTweener SetSelfScale<TTweener>(this TTweener tweener, float selfScale) where TTweener : Tweener
        {
            tweener.Animation.SetSelfScale(selfScale);
            return tweener;
        }

        public static TTweener SetAutoKill<TTweener>(this TTweener tweener, bool autoKill) where TTweener : Tweener
        {
            tweener.Animation.SetAutoKill(autoKill);
            return tweener;
        }

        public static TTweener SetPreSample<TTweener>(this TTweener tweener, PrepareSampleMode prepareSampleMode) where TTweener : Tweener
        {
            tweener.Animation.SetPrepareSample(prepareSampleMode);
            return tweener;
        }

        public static TTweener SetSpeedBased<TTweener>(this TTweener tweener, bool speedBased = true) where TTweener : Tweener
        {
            tweener.Animation.SetSpeedBased(speedBased);
            return tweener;
        }

        #endregion

        #region Play / Pasue / Resume / Stop

        public static TTweener Play<TTweener>(this TTweener tweener, bool forward = true) where TTweener : Tweener
        {
            return Play(tweener, 0f, forward);
        }

        public static TTweener Play<TTweener>(this TTweener tweener, float initNormalizedProgress, bool forward = true) where TTweener : Tweener
        {
            if (tweener.Animation == null)
            {
                var tweenData = UTween.CreateTweenAnimation();
                tweenData.AddTweener(tweener);
            }

            tweener.Animation?.Play(initNormalizedProgress, forward);
            return tweener;
        }

        public static TTweener Pause<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation?.Pause();
            return tweener;
        }

        public static TTweener Resume<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation?.Resume();
            return tweener;
        }

        public static TTweener Stop<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation?.Stop();
            return tweener;
        }

        public static TTweener PlayForward<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation?.PlayForward();
            return tweener;
        }

        public static TTweener PlayBackward<TTweener>(this TTweener tweener) where TTweener : Tweener
        {
            tweener.Animation?.PlayBackward();
            return tweener;
        }

        #endregion

        #region Add / Remove Tweener

        public static TTweener AddTweener<TTweener>(this TTweener tweener, Tweener otherTweener) where TTweener : Tweener
        {
            tweener.Animation.AddTweener(otherTweener);
            return tweener;
        }

        public static TTweener RemoveTweener<TTweener>(this TTweener tweener, Tweener otherTweener) where TTweener : Tweener
        {
            tweener.Animation.RemoveTweener(otherTweener);
            return tweener;
        }

        #endregion

        #region Append / Join / Insert

        public static TTweener Append<TTweener>(this TTweener tweener, Tweener otherTweener) where TTweener : Tweener
        {
            tweener.Animation.Append(otherTweener);
            return tweener;
        }

        public static TTweener AppendInterval<TTweener>(this TTweener tweener, float interval) where TTweener : Tweener
        {
            tweener.Animation.AppendInterval(interval);
            return tweener;
        }

        public static TTweener Join<TTweener>(this TTweener tweener, Tweener otherTweener) where TTweener : Tweener
        {
            tweener.Animation.Join(otherTweener);
            return tweener;
        }

        public static TTweener Insert<TTweener>(this TTweener tweener, float delay, Tweener otherTweener) where TTweener : Tweener
        {
            tweener.Animation.Insert(delay, otherTweener);
            return tweener;
        }

        #endregion

        #region Set Callback

        public static TTweener SetStopCondition<TTweener>(this TTweener tweener, Func<bool> stopCondition) where TTweener : Tweener
        {
            tweener.Animation.SetStopCondition(stopCondition);
            return tweener;
        }

        public static TTweener SetOnPlay<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnPlay.SetListener(onPlay);
            return tweener;
        }

        public static TTweener AddOnPlay<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnPlay.AddListener(onPlay);
            return tweener;
        }

        public static TTweener SetOnUpdate<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnUpdate.SetListener(onPlay);
            return tweener;
        }

        public static TTweener AddOnUpdate<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnUpdate.AddListener(onPlay);
            return tweener;
        }

        public static TTweener SetOnLoopStart<TTweener>(this TTweener tweener, Action onLoopStart) where TTweener : Tweener
        {
            tweener.Animation.OnLoopStart.SetListener(onLoopStart);
            return tweener;
        }

        public static TTweener AddOnLoopStart<TTweener>(this TTweener tweener, Action onLoopStart) where TTweener : Tweener
        {
            tweener.Animation.OnLoopStart.AddListener(onLoopStart);
            return tweener;
        }

        public static TTweener SetOnLoopEnd<TTweener>(this TTweener tweener, Action onLoopEnd) where TTweener : Tweener
        {
            tweener.Animation.OnLoopEnd.SetListener(onLoopEnd);
            return tweener;
        }

        public static TTweener AddOnLoopEnd<TTweener>(this TTweener tweener, Action onLoopEnd) where TTweener : Tweener
        {
            tweener.Animation.OnLoopEnd.AddListener(onLoopEnd);
            return tweener;
        }

        public static TTweener SetOnPause<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnPause.SetListener(onPlay);
            return tweener;
        }

        public static TTweener AddOnPause<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnPause.AddListener(onPlay);
            return tweener;
        }

        public static TTweener SetOnResume<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnResume.SetListener(onPlay);
            return tweener;
        }

        public static TTweener AddOnResume<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnResume.AddListener(onPlay);
            return tweener;
        }

        public static TTweener SetOnStop<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnStop.SetListener(onPlay);
            return tweener;
        }

        public static TTweener AddOnStop<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnStop.AddListener(onPlay);
            return tweener;
        }

        public static TTweener SetOnComplete<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnComplete.SetListener(onPlay);
            return tweener;
        }

        public static TTweener AddOnComplete<TTweener>(this TTweener tweener, Action onPlay) where TTweener : Tweener
        {
            tweener.Animation.OnComplete.AddListener(onPlay);
            return tweener;
        }

        #endregion
    }
}