using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Spot Angle", nameof(Renderer))]
    [Serializable]
    public class TweenLightSpotAngle : TweenValueFloat<Light>
    {
        public override float Value
        {
            get => Target.spotAngle;
            set => Target.spotAngle = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenLightSpotAngle SpotAngle(Light light, float to, float duration)
        {
            var tweener = Play<TweenLightSpotAngle, Light, float>(light, to, duration);
            return tweener;
        }

        public static TweenLightSpotAngle SpotAngle(Light light, float from, float to, float duration)
        {
            var tweener = Play<TweenLightSpotAngle, Light, float>(light, from, to, duration);
            return tweener;
        }
    }

    public static partial class LightExtension
    {
        public static TweenLightSpotAngle TweenSpotAngle(this Light light, float to, float duration)
        {
            var tweener = UTween.SpotAngle(light, to, duration);
            return tweener;
        }

        public static TweenLightSpotAngle TweenSpotAngle(this Light light, float from, float to, float duration)
        {
            var tweener = UTween.SpotAngle(light, from, to, duration);
            return tweener;
        }
    }

    #endregion
}