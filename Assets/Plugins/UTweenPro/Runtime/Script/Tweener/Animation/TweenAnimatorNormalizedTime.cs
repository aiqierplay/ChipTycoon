using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Normalized Time", nameof(Animation))]
    [Serializable]
    public partial class TweenAnimatorNormalizedTime : TweenValueFloat<Animator>
    {
        public int Layer;
        public string State;

        public override bool RequireClampMin => true;
        public override bool RequireClampMax => true;

        public override float Value
        {
            get
            {
                if (!Target.isActiveAndEnabled) return 0f;
                var stateInfo = Target.GetCurrentAnimatorStateInfo(Layer);
                if (stateInfo.IsName(State)) return stateInfo.normalizedTime;
                return 0f;
            }
            set
            {
                if (!Target.isActiveAndEnabled) return;
                Target.Play(State, Layer, value);
                Target.Update(0);
                Target.speed = 0f;
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

    public partial class TweenAnimatorNormalizedTime : TweenValueFloat<Animator>
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

    public partial class TweenAnimatorNormalizedTime : TweenValueFloat<Animator>
    {
        public TweenAnimatorNormalizedTime SetState(string stateMName)
        {
            State = stateMName;
            return this;
        }

        public TweenAnimatorNormalizedTime SetLayerIndex(int layerIndex)
        {
            Layer = layerIndex;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(0)
                .SetState(clipName);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime NormalizedTime(Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = Play<TweenAnimatorNormalizedTime, Animator, float>(animator, from, to, duration)
                .SetLayerIndex(layerIndex)
                .SetState(clipName);
            return tweener;
        }
    }

    public static partial class AnimatorExtension
    {
        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, string clipName, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, clipName, from, to, duration);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, int layerIndex, string clipName, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, layerIndex, clipName, to, duration);
            return tweener;
        }

        public static TweenAnimatorNormalizedTime TweenNormalizedTime(this Animator animator, int layerIndex, string clipName, float from, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(animator, layerIndex, clipName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}