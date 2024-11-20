using UnityEngine;

namespace Aya.TweenPro
{
    public abstract class TweenYieldInstruction : CustomYieldInstruction
    {
        public TweenAnimation Animation;

        protected TweenYieldInstruction(TweenAnimation tweenAnimation)
        {
            Animation = tweenAnimation;
        }
    }
}