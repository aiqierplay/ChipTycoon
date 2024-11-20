
using UnityEngine;

namespace Aya.TweenPro
{
    public class WaitForNormalizedDuration : TweenYieldInstruction
    {
        public float NormalizedDuration;

        public WaitForNormalizedDuration(TweenAnimation tweenAnimation, float normalizedDuration) : base(tweenAnimation)
        {
            NormalizedDuration = Mathf.Clamp01(normalizedDuration);
        }

        public override bool keepWaiting => Animation.RuntimeNormalizedProgress < NormalizedDuration;
    }
}