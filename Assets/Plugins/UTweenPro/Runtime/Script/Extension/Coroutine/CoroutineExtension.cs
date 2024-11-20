using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Aya.TweenPro
{
    public static partial class CoroutineExtension
    {
        #region TweenData
        
        public static IEnumerator WaitForPlaying(this TweenAnimation tweenAnimation)
        {
            return new WaitForPlaying(tweenAnimation);
        }

        public static IEnumerator WaitForStart(this TweenAnimation tweenAnimation)
        {
            return new WaitForStart(tweenAnimation);
        }

        public static IEnumerator WaitForComplete(this TweenAnimation tweenAnimation)
        {
            return new WaitForComplete(tweenAnimation);
        }

        public static IEnumerator WaitForDuration(this TweenAnimation tweenAnimation, float duration)
        {
            return new WaitForDuration(tweenAnimation, duration);
        }

        public static IEnumerator WaitForNormalizedDuration(this TweenAnimation tweenAnimation, float normalizedDuration)
        {
            return new WaitForNormalizedDuration(tweenAnimation, normalizedDuration);
        }

        public static IEnumerator WaitForElapsedLoops(this TweenAnimation tweenAnimation, int loop)
        {
            return new WaitForElapsedLoops(tweenAnimation, loop);
        }

        #endregion

        #region Tweener

        public static IEnumerator WaitForPlaying(this Tweener tweener)
        {
            return new WaitForPlaying(tweener.Animation);
        }

        public static IEnumerator WaitForStart(this Tweener tweener)
        {
            return new WaitForStart(tweener.Animation);
        }

        public static IEnumerator WaitForComplete(this Tweener tweener)
        {
            return new WaitForComplete(tweener.Animation);
        }

        public static IEnumerator WaitForDuration(this Tweener tweener, float duration)
        {
            return new WaitForDuration(tweener.Animation, duration);
        }

        public static IEnumerator WaitForNormalizedDuration(this Tweener tweener, float normalizedDuration)
        {
            return new WaitForNormalizedDuration(tweener.Animation, normalizedDuration);
        }

        public static IEnumerator WaitForElapsedLoops(this Tweener tweener, int loop)
        {
            return new WaitForElapsedLoops(tweener.Animation, loop);
        }

        #endregion
    }
}