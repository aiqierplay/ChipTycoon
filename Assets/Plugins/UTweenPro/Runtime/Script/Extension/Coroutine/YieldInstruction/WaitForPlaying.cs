
namespace Aya.TweenPro
{
    public class WaitForPlaying : TweenYieldInstruction
    {
        public WaitForPlaying(TweenAnimation tweenAnimation) : base(tweenAnimation)
        {
        }

        public override bool keepWaiting => !Animation.IsPlaying;
    }
}