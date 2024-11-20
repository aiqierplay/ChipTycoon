
namespace Aya.TweenPro
{
    public class WaitForDuration : TweenYieldInstruction
    {
        public float Duration;

        public WaitForDuration(TweenAnimation tweenAnimation, float duration) : base(tweenAnimation)
        {
            Duration = duration;
            if (Duration > tweenAnimation.Duration) Duration = tweenAnimation.Duration;
        }

        public override bool keepWaiting => Animation.PlayTimer < Duration;
    }
}