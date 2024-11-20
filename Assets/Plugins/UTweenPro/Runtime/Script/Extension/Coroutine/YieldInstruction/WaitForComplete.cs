
namespace Aya.TweenPro
{
    public class WaitForComplete: TweenYieldInstruction
    {
        public WaitForComplete(TweenAnimation tweenAnimation) : base(tweenAnimation)
        {
        }

        public override bool keepWaiting => !Animation.IsCompleted;
    }
}