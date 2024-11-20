
namespace Aya.TweenPro
{
    public class WaitForStart : TweenYieldInstruction
    {
        public WaitForStart(TweenAnimation tweenAnimation) : base(tweenAnimation)
        {
        }

        public override bool keepWaiting => !Animation.IsPlaying || (Animation.IsPlaying && Animation.IsDelaying);
    }
}