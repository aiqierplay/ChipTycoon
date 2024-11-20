using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    // TODO..
    // 启动自动播放问题
    // 自动计算时长

    [Tweener("Normalized Time", nameof(Renderer))]
    [Serializable]
    public partial class TweenParticleSystemNormalizedTime : TweenValueFloat<ParticleSystem>
    {
        public bool RandStart;
        public bool WithChildren;

        public override float Value
        {
            get => _value;
            set
            {
                _value = value;
                Playback(_value);
            }
        }

        private float _value;
        private bool _initRandomSeed;
        private uint _randomSeed;

        public override void PrepareSample()
        {
            base.PrepareSample();
            if (Application.isPlaying)
            {
                if (RandStart || !_initRandomSeed)
                {
                    _randomSeed = (uint)Random.Range(0f, 10000f);
                    _initRandomSeed = true;
                }
            }
        }

        public void Playback(float time)
        {
            Target.Pause(WithChildren);
            Target.Stop(WithChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
            Target.useAutoRandomSeed = RandStart;
            Target.randomSeed = _randomSeed;
            Target.Play(WithChildren);
            Target.Simulate(time, WithChildren, false, false);
            Target.Play(WithChildren);
        }

        public override void StopSample()
        {
            base.StopSample();
            Target.Pause(WithChildren);
            Target.Stop(WithChildren, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public override void Reset()
        {
            base.Reset();

            _initRandomSeed = false;
            RandStart = true;
            WithChildren = true;
        }
    }

#if UNITY_EDITOR

    public partial class TweenParticleSystemNormalizedTime : TweenValueFloat<ParticleSystem>
    {
        [TweenerProperty, NonSerialized] public SerializedProperty RandStartProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty WithChildrenProperty;

        public override void DrawAppend()
        {
            base.DrawAppend();

            using (GUIHorizontal.Create())
            {
                GUIUtil.DrawToggleButton(RandStartProperty);
                GUIUtil.DrawToggleButton(WithChildrenProperty);
            }
        }
    }

#endif

    #region Extension

    public static partial class UTween
    {
        public static TweenParticleSystemNormalizedTime NormalizedTime(ParticleSystem particleSystem, float to, float duration)
        {
            var tweener = Play<TweenParticleSystemNormalizedTime, ParticleSystem, float>(particleSystem, to, duration);
            return tweener;
        }

        public static TweenParticleSystemNormalizedTime NormalizedTime(ParticleSystem particleSystem, float from, float to, float duration)
        {
            var tweener = Play<TweenParticleSystemNormalizedTime, ParticleSystem, float>(particleSystem, from, to, duration);
            return tweener;
        }
    }

    public static partial class TweenParticleSystemExtension
    {
        public static TweenParticleSystemNormalizedTime TweenNormalizedTime(this ParticleSystem particleSystem, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(particleSystem, to, duration);
            return tweener;
        }

        public static TweenParticleSystemNormalizedTime TweenNormalizedTime(this ParticleSystem particleSystem, float from, float to, float duration)
        {
            var tweener = UTween.NormalizedTime(particleSystem, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
