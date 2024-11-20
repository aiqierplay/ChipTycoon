/////////////////////////////////////////////////////////////////////////////
//
//  Script : YieldBuilder.cs
//  Info   : Yield 构建器，用于提供 Yield 对象并实现复用，减少 new yield 开销
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Aya.Async
{
    public static partial class YieldBuilder
    {
        #region Cache

        private static readonly WaitForEndOfFrame WaitForEndOfFrameCache = new WaitForEndOfFrame();
        private static readonly WaitForFixedUpdate WaitForFixedUpdateCache = new WaitForFixedUpdate();

        #endregion

        #region Async Task -> Coroutine

        public static IEnumerator AsyncTask(Action action)
        {
            var complete = false;
            AsyncTaskInternal(action, () => { complete = true; });
            while (!complete)
            {
                yield return null;
            }

            yield return null;
        }

        private static async void AsyncTaskInternal(Action action, Action onDone)
        {
            await Task.Run(action);
            onDone();
        }

        #endregion

        #region Wait Builder

        public static WaitForEndOfFrame WaitForEndOfFrame()
        {
            return WaitForEndOfFrameCache;
        }

        public static WaitForFixedUpdate WaitForFixedUpdate()
        {
            return WaitForFixedUpdateCache;
        }

        public static IEnumerator WaitForSeconds(float second, bool timeScale)
        {
            if (timeScale)
            {
                yield return WaitForSeconds(second);
            }
            else
            {
                yield return WaitForSecondsRealtime(second);
            }
        }

        public static IEnumerator WaitForSeconds(float second)
        {
            var timer = 0f;
            do
            {
                timer += Time.deltaTime;
                yield return null;
            } while (timer < second);
        }

        public static IEnumerator WaitForSecondsRealtime(float second)
        {
            var timer = 0f;
            do
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            } while (timer < second);
        }

        public static IEnumerator WaitForNextFrame()
        {
            yield return null;
        }

        public static IEnumerator WaitForFrames(int frames)
        {
            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }
        }

        public static IEnumerator WaitUntil(Func<bool> predicate)
        {
            while (!predicate())
            {
                yield return null;
            }
        }

        public static IEnumerator WaitWhile(Func<bool> predicate)
        {
            while (predicate())
            {
                yield return null;
            }
        }

        #endregion
    }
}
