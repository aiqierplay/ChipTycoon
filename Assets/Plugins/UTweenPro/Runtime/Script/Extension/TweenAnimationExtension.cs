using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class TweenAnimationExtension
    {
        #region Set Target

        public static TweenAnimation SetTarget<T>(this TweenAnimation tweenAnimation, T target) where T : UnityEngine.Object
        {
            foreach (var tweener in tweenAnimation.TweenerList)
            {
                if (tweener is Tweener<T> tempTweener)
                {
                    tempTweener.SetTarget(target);
                }
            }

            return tweenAnimation;
        }

        public static TweenAnimation SetTarget<T>(this TweenAnimation tweenAnimation, int index, T target) where T : UnityEngine.Object
        {
            var tweener = tweenAnimation.TweenerList[index];
            if (tweener is Tweener<T> tempTweener)
            {
                tempTweener.SetTarget(target);
            }

            return tweenAnimation;
        }

        public static TweenAnimation SetTarget<T>(this TweenAnimation tweenAnimation, params T[] targets) where T : UnityEngine.Object
        {
            var index = 0;
            foreach (var tweener in tweenAnimation.TweenerList)
            {
                if (tweener is Tweener<T> tempTweener)
                {
                    if (index >= targets.Length) break;
                    var target = targets[index];
                    tempTweener.SetTarget(target);
                    index++;
                }
            }

            return tweenAnimation;
        }


        #endregion

        #region Set Property

        public static TweenAnimation SetOwner(this TweenAnimation tweenAnimation, object owner)
        {
            tweenAnimation.Owner = owner;
            return tweenAnimation;
        }

        public static TweenAnimation SetIdentifier(this TweenAnimation tweenAnimation, string identifier)
        {
            tweenAnimation.Identifier = identifier;
            return tweenAnimation;
        }

        public static TweenAnimation SetDuration(this TweenAnimation tweenAnimation, float duration)
        {
            if (duration < 1e-6f) duration = 1e-6f;
            tweenAnimation.Duration = duration;
            return tweenAnimation;
        }

        public static TweenAnimation SetDelay(this TweenAnimation tweenAnimation, float delay)
        {
            if (delay < 0f) delay = 0f;
            tweenAnimation.Delay = delay;
            return tweenAnimation;
        }

        public static TweenAnimation SetBackward(this TweenAnimation tweenAnimation, bool backward)
        {
            tweenAnimation.Backward = backward;
            return tweenAnimation;
        }

        public static TweenAnimation SetPlayMode(this TweenAnimation tweenAnimation, PlayMode playMode)
        {
            tweenAnimation.PlayMode = playMode;
            if (playMode == PlayMode.Once) tweenAnimation.PlayCount = 1;
            return tweenAnimation;
        }

        public static TweenAnimation SetPlayCount(this TweenAnimation tweenAnimation, int playCount)
        {
            if (playCount < 0) playCount = 0;
            if (tweenAnimation.PlayMode == PlayMode.Once) playCount = 1;
            tweenAnimation.PlayCount = playCount;
            return tweenAnimation;
        }

        public static TweenAnimation SetPlayOnce(this TweenAnimation tweenAnimation)
        {
            tweenAnimation.PlayMode = PlayMode.Once;
            tweenAnimation.PlayCount = 1;
            return tweenAnimation;
        }

        public static TweenAnimation SetLoopCount(this TweenAnimation tweenAnimation, int playCount)
        {
            tweenAnimation.PlayMode = PlayMode.Loop;
            tweenAnimation.PlayCount = playCount;
            return tweenAnimation;
        }

        public static TweenAnimation SetLoopUnlimited(this TweenAnimation tweenAnimation)
        {
            tweenAnimation.PlayMode = PlayMode.Loop;
            tweenAnimation.PlayCount = -1;
            return tweenAnimation;
        }

        public static TweenAnimation SetPingPongCount(this TweenAnimation tweenAnimation, int playCount)
        {
            tweenAnimation.PlayMode = PlayMode.PingPong;
            tweenAnimation.PlayCount = playCount;
            return tweenAnimation;
        }

        public static TweenAnimation SetPingPongUnlimited(this TweenAnimation tweenAnimation)
        {
            tweenAnimation.PlayMode = PlayMode.PingPong;
            tweenAnimation.PlayCount = -1;
            return tweenAnimation;
        }

        public static TweenAnimation SetAutoPlay(this TweenAnimation tweenAnimation, AutoPlayMode autoPlay)
        {
            tweenAnimation.AutoPlay = autoPlay;
            return tweenAnimation;
        }

        public static TweenAnimation SetUpdateMode(this TweenAnimation tweenAnimation, UpdateMode updateMode)
        {
            tweenAnimation.UpdateMode = updateMode;
            return tweenAnimation;
        }

        public static TweenAnimation SetInterval(this TweenAnimation tweenAnimation, float interval)
        {
            if (interval < 0f) interval = 0f;
            tweenAnimation.Interval = interval;
            return tweenAnimation;
        }

        public static TweenAnimation SetInterval(this TweenAnimation tweenAnimation, float interval, float interval2)
        {
            if (interval < 0f) interval = 0f;
            tweenAnimation.Interval = interval;
            if (interval2 < 0f) interval2 = 0f;
            tweenAnimation.Interval2 = interval2;
            return tweenAnimation;
        }

        public static TweenAnimation SetTimeMode(this TweenAnimation tweenAnimation, TimeMode timeMode)
        {
            tweenAnimation.TimeMode = timeMode;
            return tweenAnimation;
        }

        public static TweenAnimation SetSelfScale(this TweenAnimation tweenAnimation, float selfScale)
        {
            if (selfScale < 1e-6f) selfScale = 1e-6f;
            tweenAnimation.SelfScale = selfScale;
            return tweenAnimation;
        }

        public static TweenAnimation SetAutoKill(this TweenAnimation tweenAnimation, bool autoKill)
        {
            tweenAnimation.AutoKill = autoKill;
            return tweenAnimation;
        }

        public static TweenAnimation SetPrepareSample(this TweenAnimation tweenAnimation, PrepareSampleMode prepareSampleMode)
        {
            tweenAnimation.PrepareSampleMode = prepareSampleMode;
            return tweenAnimation;
        }

        public static TweenAnimation SetSpeedBased(this TweenAnimation tweenAnimation, bool speedBased = true)
        {
            tweenAnimation.SpeedBased = speedBased;
            return tweenAnimation;
        }

        public static TweenAnimation AdaptDuration(this TweenAnimation tweenAnimation)
        {
            var duration = 1e-6f;
            foreach (var tweener in tweenAnimation.TweenerList)
            {
                if (tweener.TotalDuration > duration) duration = tweener.TotalDuration;
            }

            tweenAnimation.Duration = duration;
            return tweenAnimation;
        }

        #endregion

        #region Set Stats

        public static TweenAnimation SetProgress(this TweenAnimation tweenAnimation, float progress)
        {
            tweenAnimation.PlayTimer = progress;
            tweenAnimation.Update(0f);
            return tweenAnimation;
        }

        public static TweenAnimation SetNormalizedProgress(this TweenAnimation tweenAnimation, float normalizedProgress)
        {
            normalizedProgress = Mathf.Clamp01(normalizedProgress);
            var progress = tweenAnimation.RuntimeDuration * normalizedProgress;
            tweenAnimation.SetProgress(progress);
            return tweenAnimation;
        }

        #endregion

        #region Set Callback

        public static TweenAnimation SetStopCondition(this TweenAnimation tweenAnimation, Func<bool> stopCondition)
        {
            tweenAnimation.StopCondition = stopCondition;
            return tweenAnimation;
        }

        public static TweenAnimation SetOnPlay(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnPlay.SetListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnUpdate(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnUpdate.SetListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnLoopStart(this TweenAnimation tweenAnimation, Action onLoopStart)
        {
            tweenAnimation.OnLoopStart.SetListener(onLoopStart);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnLoopEnd(this TweenAnimation tweenAnimation, Action onLoopEnd)
        {
            tweenAnimation.OnLoopEnd.SetListener(onLoopEnd);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnPause(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnPause.SetListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnResume(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnResume.SetListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnStop(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnStop.SetListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation SetOnComplete(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnComplete.SetListener(onPlay);
            return tweenAnimation;
        }

        #endregion

        #region Add Callback

        public static TweenAnimation AddStopCondition(this TweenAnimation tweenAnimation, Func<bool> stopCondition)
        {
            tweenAnimation.StopCondition += stopCondition;
            return tweenAnimation;
        }

        public static TweenAnimation AddOnPlay(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnPlay.AddListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnUpdate(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnUpdate.AddListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnLoopStart(this TweenAnimation tweenAnimation, Action onLoopStart)
        {
            tweenAnimation.OnLoopStart.AddListener(onLoopStart);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnLoopEnd(this TweenAnimation tweenAnimation, Action onLoopEnd)
        {
            tweenAnimation.OnLoopEnd.AddListener(onLoopEnd);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnPause(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnPause.AddListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnResume(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnResume.AddListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnStop(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnStop.AddListener(onPlay);
            return tweenAnimation;
        }

        public static TweenAnimation AddOnComplete(this TweenAnimation tweenAnimation, Action onPlay)
        {
            tweenAnimation.OnComplete.AddListener(onPlay);
            return tweenAnimation;
        }

        #endregion

        #region Get Tweener

        public static TTweener GetTweener<TTweener>(this TweenAnimation tweenAnimation, Predicate<TTweener> predicate = null) where TTweener : Tweener
        {
            foreach (var tweener in tweenAnimation.TweenerList)
            {
                if (!(tweener is TTweener result)) continue;
                if (predicate == null) return result;
                if (predicate(result)) return result;
            }

            return default;
        }

        public static List<TTweener> GetTweeners<TTweener>(this TweenAnimation tweenAnimation, Predicate<TTweener> predicate = null) where TTweener : Tweener
        {
            var result = new List<TTweener>();
            foreach (var tweener in tweenAnimation.TweenerList)
            {
                if (!(tweener is TTweener temp)) continue;
                if (predicate == null) result.Add(temp);
                else if (predicate(temp)) result.Add(temp);
            }

            return result;
        }

        #endregion

        #region Append / Join / Insert

        public static TweenAnimation Append(this TweenAnimation tweenAnimation, Tweener tweener)
        {
            tweener.Delay = tweenAnimation.Duration;
            tweenAnimation.Duration += tweener.Duration;
            tweenAnimation.AddTweener(tweener);
            return tweenAnimation;
        }

        public static TweenAnimation AppendInterval(this TweenAnimation tweenAnimation, float interval)
        {
            tweenAnimation.Duration += interval;
            return tweenAnimation;
        }

        public static TweenAnimation Join(this TweenAnimation tweenAnimation, Tweener tweener)
        {
            Insert(tweenAnimation, 0f, tweener);
            return tweenAnimation;
        }

        public static TweenAnimation Insert(this TweenAnimation tweenAnimation, float delay, Tweener tweener)
        {
            tweener.Delay = delay;
            tweenAnimation.AddTweener(tweener);
            tweenAnimation.AdaptDuration();
            return tweenAnimation;
        }

        #endregion
    }
}