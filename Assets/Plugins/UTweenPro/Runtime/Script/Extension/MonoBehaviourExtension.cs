using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.TweenPro
{
    public static partial class MonoBehaviourExtension
    {
        #region TweenAnimation
        
        public static UTweenPlayer GetTweenAnimation(this MonoBehaviour monoBehaviour)
        {
            var tweenAnimation = monoBehaviour.GetComponent<UTweenPlayer>();
            return tweenAnimation;
        }

        public static UTweenPlayer GetTweenAnimationInChildren(this MonoBehaviour monoBehaviour)
        {
            var tweenAnimation = monoBehaviour.GetComponentInChildren<UTweenPlayer>();
            return tweenAnimation;
        }

        public static UTweenPlayer GetTweenAnimation(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenAnimations = monoBehaviour.GetComponents<UTweenPlayer>();
            foreach (var tweenAnimation in tweenAnimations)
            {
                if (tweenAnimation.Animation.Identifier == identifier) return tweenAnimation;
            }

            return default;
        }

        public static UTweenPlayer GetTweenAnimationInChildren(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenAnimations = monoBehaviour.GetComponentsInChildren<UTweenPlayer>();
            foreach (var tweenAnimation in tweenAnimations)
            {
                if (tweenAnimation.Animation.Identifier == identifier) return tweenAnimation;
            }

            return default;
        }

        public static List<UTweenPlayer> GetTweenAnimations(this MonoBehaviour monoBehaviour)
        {
            var tweenAnimations = new List<UTweenPlayer>();
            var components = monoBehaviour.GetComponents<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                tweenAnimations.Add(tweenAnimation);
            }

            return tweenAnimations;
        }

        public static List<UTweenPlayer> GetTweenAnimationsInChildren(this MonoBehaviour monoBehaviour)
        {
            var tweenAnimations = new List<UTweenPlayer>();
            var components = monoBehaviour.GetComponentsInChildren<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                tweenAnimations.Add(tweenAnimation);
            }

            return tweenAnimations;
        }

        public static List<UTweenPlayer> GetTweenAnimations(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenAnimations = monoBehaviour.GetTweenAnimations(t => t.Animation.Identifier == identifier);
            return tweenAnimations;
        }

        public static List<UTweenPlayer> GetTweenAnimationsInChildren(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenAnimations = monoBehaviour.GetTweenAnimationsInChildren(t => t.Animation.Identifier == identifier);
            return tweenAnimations;
        }

        public static List<UTweenPlayer> GetTweenAnimations(this MonoBehaviour monoBehaviour, Predicate<UTweenPlayer> predicate)
        {
            var tweenAnimations = new List<UTweenPlayer>();
            var components = monoBehaviour.GetComponents<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                if (predicate == null || predicate(tweenAnimation)) tweenAnimations.Add(tweenAnimation);
            }

            return tweenAnimations;
        }

        public static List<UTweenPlayer> GetTweenAnimationsInChildren(this MonoBehaviour monoBehaviour, Predicate<UTweenPlayer> predicate)
        {
            var tweenAnimations = new List<UTweenPlayer>();
            var components = monoBehaviour.GetComponentsInChildren<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                if (predicate == null || predicate(tweenAnimation)) tweenAnimations.Add(tweenAnimation);
            }

            return tweenAnimations;
        }

        #endregion

        #region TweenData

        public static TweenAnimation GetTweenData(this MonoBehaviour monoBehaviour)
        {
            var tweenAnimation = monoBehaviour.GetComponent<UTweenPlayer>();
            if (tweenAnimation != null) return tweenAnimation.Animation;
            return default;
        }

        public static TweenAnimation GetTweenDataInChildren(this MonoBehaviour monoBehaviour)
        {
            var tweenAnimation = monoBehaviour.GetComponentInChildren<UTweenPlayer>();
            if (tweenAnimation != null) return tweenAnimation.Animation;
            return default;
        }

        public static TweenAnimation GetTweenData(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenAnimations = monoBehaviour.GetComponents<UTweenPlayer>();
            foreach (var tweenAnimation in tweenAnimations)
            {
                if (tweenAnimation.Animation.Identifier == identifier) return tweenAnimation.Animation;
            }

            return default;
        }

        public static TweenAnimation GetTweenDataInChildren(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenAnimations = monoBehaviour.GetComponentsInChildren<UTweenPlayer>();
            foreach (var tweenAnimation in tweenAnimations)
            {
                if (tweenAnimation.Animation.Identifier == identifier) return tweenAnimation.Animation;
            }

            return default;
        }

        public static List<TweenAnimation> GetTweenDatas(this MonoBehaviour monoBehaviour)
        {
            var tweenDatas = new List<TweenAnimation>();
            var components = monoBehaviour.GetComponents<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                tweenDatas.Add(tweenAnimation.Animation);
            }

            return tweenDatas;
        }

        public static List<TweenAnimation> GetTweenDatasInChildren(this MonoBehaviour monoBehaviour)
        {
            var tweenDatas = new List<TweenAnimation>();
            var components = monoBehaviour.GetComponentsInChildren<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                tweenDatas.Add(tweenAnimation.Animation);
            }

            return tweenDatas;
        }

        public static List<TweenAnimation> GetTweenDatas(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenDatas = monoBehaviour.GetTweenDatas(d => d.Identifier == identifier);
            return tweenDatas;
        }

        public static List<TweenAnimation> GetTweenDatasInChildren(this MonoBehaviour monoBehaviour, string identifier)
        {
            var tweenDatas = monoBehaviour.GetTweenDatasInChildren(d => d.Identifier == identifier);
            return tweenDatas;
        }

        public static List<TweenAnimation> GetTweenDatas(this MonoBehaviour monoBehaviour, Predicate<TweenAnimation> predicate)
        {
            var tweenDatas = new List<TweenAnimation>();
            var components = monoBehaviour.GetComponents<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                if (predicate == null || predicate(tweenAnimation.Animation)) tweenDatas.Add(tweenAnimation.Animation);
            }

            return tweenDatas;
        }

        public static List<TweenAnimation> GetTweenDatasInChildren(this MonoBehaviour monoBehaviour, Predicate<TweenAnimation> predicate)
        {
            var tweenDatas = new List<TweenAnimation>();
            var components = monoBehaviour.GetComponentsInChildren<UTweenPlayer>();
            foreach (var tweenAnimation in components)
            {
                if (predicate == null || predicate(tweenAnimation.Animation)) tweenDatas.Add(tweenAnimation.Animation);
            }

            return tweenDatas;
        }

        #endregion
    }
}
