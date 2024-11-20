using System;

namespace Aya.TweenPro
{
    public static class UTweenCallback
    {
        public static Action<TweenAnimation> OnAddAnimation = delegate(TweenAnimation tweenAnimation) {  };
        public static Action<TweenAnimation> OnRemoveAnimation = delegate (TweenAnimation tweenAnimation) { };

        public static Action<Exception> OnException = UnityEngine.Debug.LogError;
    }
}
