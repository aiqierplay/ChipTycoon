using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Time", nameof(Animation))]
    [Serializable]
    public partial class TweenAnimatorTime : TweenValueFloat<Animator>
    {
        public int Layer;
        public string State;

        public override bool RequireClampMin => true;

        public override float Value
        {
            get => CurrentStateDuration;
            set
            {
                if (!Target.isActiveAndEnabled) return;
                var duration = CurrentStateDuration;
                var factor = value / duration;
                Target.Play(State, Layer, factor);
                Target.Update(0);
                Target.speed = 0f;
            }
        }

        public float CurrentStateDuration
        {
            get
            {
                if (!Target.isActiveAndEnabled) return 0f;
                var stateInfo = Target.GetCurrentAnimatorStateInfo(Layer);
                if (stateInfo.IsName(State)) return 0f;
                return stateInfo.length;
            }
        }

        public override void Reset()
        {
            base.Reset();
            State = null;
            Layer = 0;
        }
    }

#if UNITY_EDITOR

    public partial class TweenAnimatorTime : TweenValueFloat<Animator>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty StateProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty LayerProperty;

        public override void DrawTarget()
        {
            base.DrawTarget();

            if (Target == null)
            {
                StateProperty.stringValue = null;
                return;
            }

            GUIMenu.SelectAnimatorLayerMenu(Target, nameof(Layer), LayerProperty);
            GUIMenu.SelectAnimatorStateMenu(Target, LayerProperty.intValue, nameof(State), StateProperty);
        }
    }

#endif

    #region Extension

    public partial class TweenAnimatorTime : TweenValueFloat<Animator>
    {
        public TweenAnimatorTime SetState(string stateMName)
        {
            State = stateMName;
            return this;
        }

        public TweenAnimatorTime SetLayerIndex(int layerIndex)
        {
            Layer = layerIndex;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenAnimatorTime Time(Animator animator, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName);
            return tweener;
        }

        public static TweenAnimatorTime Time(Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName);
            return tweener;
        }

        public static TweenAnimatorTime Time(Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName);
            return tweener;
        }

        public static TweenAnimatorTime Time(Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName);
            return tweener;
        }
    }

    public static partial class AnimatorExtension
    {
        public static TweenAnimatorTime TweenTime(this Animator animator, string clipName, float to, float duration)
        {
            var tweener = UTween.Time(animator, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorTime TweenTime(this Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.Time(animator, clipName, from, to, duration);
            return tweener;
        }

        public static TweenAnimatorTime TweenTime(this Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = UTween.Time(animator, layerIndex, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorTime TweenTime(this Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.Time(animator, layerIndex, clipName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}