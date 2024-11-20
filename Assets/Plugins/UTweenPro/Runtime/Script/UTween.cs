using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public static partial class UTween
    {
        internal static int InstanceIDCounter = 1;

        #region Create / Play Single

        public static TTweener Play<TTweener, TTarget>(TTarget target, float duration)
            where TTweener : Tweener<TTarget>, new()
            where TTarget : Object
        {
            var tweener = Create<TTweener>()
                .SetTarget(target)
                .SetDuration(duration)
                .Play();
            return tweener;
        }

        public static TTweener Play<TTweener, TTarget, TValue>(TTarget target, TValue to, float duration)
            where TTweener : Tweener<TTarget, TValue>, new()
            where TTarget : Object
        {
            var tweener = Create<TTweener>()
                .SetTarget(target)
                .SetDuration(duration)
                .SetCurrent2From()
                .SetTo(to)
                .Play() as TTweener;
            return tweener;
        }

        public static TTweener Play<TTweener, TTarget, TValue>(TTarget target, TValue from, TValue to, float duration)
            where TTweener : Tweener<TTarget, TValue>, new()
            where TTarget : Object
        {
            var tweener = Create<TTweener>()
                .SetTarget(target)
                .SetDuration(duration)
                .SetFrom(from)
                .SetTo(to)
                .Play() as TTweener;
            return tweener;
        }

        public static TTweener Create<TTweener>() where TTweener : Tweener, new()
        {
            var tweenAnimation = CreateTweenAnimation();
            var tweener = CreateWithoutAnimation<TTweener>();
            tweenAnimation.AddTweener(tweener);
            return tweener;
        }

        public static TTweener CreateWithoutAnimation<TTweener>() where TTweener : Tweener, new()
        {
            var tweener = UTweenPool.Spawn<TTweener>();
            tweener.Reset();
            return tweener;
        }

        public static TweenAnimation CreateTweenAnimation()
        {
            var tweenAnimation = UTweenPool.Spawn<TweenAnimation>();
            tweenAnimation.Reset();
            tweenAnimation.ControlMode = TweenControlMode.TweenManager;
            return tweenAnimation;
        }

        #endregion

        #region PreLoad

        public static void PreLoad<TTweener>() where TTweener : Tweener, new()
        {
            var tweenAnimation = UTweenPool.PreLoad<TweenAnimation>();
            tweenAnimation.Reset();
            var tweener = UTweenPool.PreLoad<TTweener>();
            tweener.Reset();
        }

        #endregion

        #region Get

        public static List<TweenAnimation> GetWithOwner(object owner)
        {
            var result = new List<TweenAnimation>();
            foreach (var tweenAnimation in UTweenManager.Ins.PlayingList)
            {
                if (tweenAnimation.Owner != owner) continue;
                result.Add(tweenAnimation);
            }

            return result;
        }

        #endregion

        #region Stop

        public static void StopAll()
        {
            foreach (var tweenAnimation in UTweenManager.Ins.PlayingList)
            {
                tweenAnimation.Stop();
            }
        }


        public static void StopWithOwner(object owner)
        {
            foreach (var tweenAnimation in UTweenManager.Ins.PlayingList)
            {
                if (tweenAnimation.Owner != owner) continue;
                tweenAnimation.Stop();
            }
        }

        public static void StopWithTarget<TTarget>(TTarget target) where TTarget : Object
        {
            foreach (var tweenAnimation in UTweenManager.Ins.PlayingList)
            {
                foreach (var tweener in tweenAnimation.TweenerList)
                {
                    if (tweener is Tweener<TTarget> tweenerTemp)
                    {
                        if (tweenerTemp.Target != target) continue;
                        tweenerTemp.Stop();
                        break;
                    }
                }
            }
        }

        public static void StopWithTarget<TTarget, TTweener>(TTarget target)
            where TTarget : Object
            where TTweener : Tweener<TTarget>
        {
            foreach (var tweenAnimation in UTweenManager.Ins.PlayingList)
            {
                foreach (var tweener in tweenAnimation.TweenerList)
                {
                    if (tweener is TTweener tweenerTemp)
                    {
                        if (tweenerTemp.Target != target) continue;
                        tweenerTemp.Stop();
                        break;
                    }
                }
            }
        }

        #endregion
    }
}