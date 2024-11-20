
namespace Aya.TweenPro
{
    public class WaitForElapsedLoops : TweenYieldInstruction
    {
        public int Loop;

        public WaitForElapsedLoops(TweenAnimation tweenAnimation, int loop) : base(tweenAnimation)
        {
            Loop = loop;
        }

        public override bool keepWaiting => Animation.LoopCounter < Loop;
    }
}