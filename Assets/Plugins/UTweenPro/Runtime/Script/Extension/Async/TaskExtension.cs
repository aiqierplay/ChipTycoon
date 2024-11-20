using System;
using System.Threading;
using System.Threading.Tasks;

namespace Aya.TweenPro
{
    public static partial class TaskExtension
    {
        #region TweenData
        
        public static async Task AsyncWaitForPlaying(this TweenAnimation tweenAnimation)
        {
            var task = WaitTask(() => !tweenAnimation.IsPlaying);
            await task;
        }

        public static async Task AsyncWaitForStart(this TweenAnimation tweenAnimation)
        {
            var task = WaitTask(() => !tweenAnimation.IsPlaying || (tweenAnimation.IsPlaying && tweenAnimation.IsDelaying));
            await task;
        }

        public static async Task AsyncWaitForComplete(this TweenAnimation tweenAnimation)
        {
            var task = WaitTask(() => !tweenAnimation.IsCompleted);
            await task;
        }

        public static async Task AsyncWaitForDuration(this TweenAnimation tweenAnimation, float duration)
        {
            var task = WaitTask(() => tweenAnimation.PlayTimer < duration);
            await task;
        }

        public static async Task AsyncWaitForNormalizedDuration(this TweenAnimation tweenAnimation, float normalizedDuration)
        {
            var task = WaitTask(() => tweenAnimation.RuntimeNormalizedProgress < normalizedDuration);
            await task;
        }

        public static async Task AsyncWaitForElapsedLoops(this TweenAnimation tweenAnimation, int loop)
        {
            var task = WaitTask(() => tweenAnimation.LoopCounter < loop);
            await task;
        }

        #endregion

        #region Tweener

        public static async Task AsyncWaitForPlaying(this Tweener tweener)
        {
            var task = tweener.Animation.AsyncWaitForPlaying();
            await task;
        }

        public static async Task AsyncWaitForStart(this Tweener tweener)
        {
            var task = tweener.Animation.AsyncWaitForStart();
            await task;
        }

        public static async Task AsyncWaitForComplete(this Tweener tweener)
        {
            var task = tweener.Animation.AsyncWaitForComplete();
            await task;
        }

        public static async Task AsyncWaitForDuration(this Tweener tweener, float duration)
        {
            var task = tweener.Animation.AsyncWaitForDuration(duration);
            await task;
        }

        public static async Task AsyncWaitForNormalizedDuration(this Tweener tweener, float normalizedDuration)
        {
            var task = tweener.Animation.AsyncWaitForNormalizedDuration(normalizedDuration);
            await task;
        }

        public static async Task AsyncWaitForElapsedLoops(this Tweener tweener, int loop)
        {
            var task = tweener.Animation.AsyncWaitForElapsedLoops(loop);
            await task;
        }

        #endregion

        #region Internal

        internal static Task WaitTask(Func<bool> keepWaiting)
        {
            var task = Task.Run(() =>
            {
                while (keepWaiting())
                {
                    Thread.Sleep(1);
                }
            });

            return task;
        } 

        #endregion
    }
}
