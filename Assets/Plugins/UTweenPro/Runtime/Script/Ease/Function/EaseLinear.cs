using System.Runtime.CompilerServices;

namespace Aya.TweenPro
{
    public class EaseLinear : EaseFunction
    {
        public override int Type => EaseType.Linear;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float Ease(float from, float to, float delta)
        {
            var result = from + (to - from) * delta;
            return result;
        }
    }
}
