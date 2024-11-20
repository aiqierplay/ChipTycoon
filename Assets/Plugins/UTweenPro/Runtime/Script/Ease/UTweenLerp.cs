
namespace Aya.TweenPro
{
    public static partial class UTween
    {
        public static float Lerp(int easeType, float from, float to, float delta)
        {
            var easeFunction = EaseType.FunctionDic[easeType];
            var result = easeFunction.Ease(from, to, delta);
            return result;
        }

        public static float Lerp(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.Linear].Ease(from, to, delta);
            return result;
        }

        public static float LerpCircular(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.Circular].Ease(from, to, delta);
            return result;
        }

        public static float LerpInQuad(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InQuad].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutQuad(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutQuad].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutQuad(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutQuad].Ease(from, to, delta);
            return result;
        }

        public static float LerpInCubic(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InCubic].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutCubic(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutCubic].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutCubic(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutCubic].Ease(from, to, delta);
            return result;
        }

        public static float LerpInQuart(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InQuart].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutQuart(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutQuart].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutQuart(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutQuart].Ease(from, to, delta);
            return result;
        }

        public static float LerpInQuint(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InQuint].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutQuint(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutQuint].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutQuint(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutQuint].Ease(from, to, delta);
            return result;
        }

        public static float LerpInSine(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InSine].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutSine(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutSine].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutSine(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutSine].Ease(from, to, delta);
            return result;
        }

        public static float LerpInExpo(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InExpo].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutExpo(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutExpo].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutExpo(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutExpo].Ease(from, to, delta);
            return result;
        }

        public static float LerpInCirc(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InCirc].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutCirc(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutCirc].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutCirc(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutCirc].Ease(from, to, delta);
            return result;
        }

        public static float LerpSpring(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.Spring].Ease(from, to, delta);
            return result;
        }

        public static float LerpInBack(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InBack].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutBack(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutBack].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutBack(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutBack].Ease(from, to, delta);
            return result;
        }

        public static float LerpPunch(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.Punch].Ease(from, to, delta);
            return result;
        }

        public static float LerpInBounce(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InBounce].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutBounce(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutBounce].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutBounce(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutBounce].Ease(from, to, delta);
            return result;
        }

        public static float LerpInElastic(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InElastic].Ease(from, to, delta);
            return result;
        }

        public static float LerpOutElastic(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.OutElastic].Ease(from, to, delta);
            return result;
        }

        public static float LerpInOutElastic(float from, float to, float delta)
        {
            var result = EaseType.FunctionDic[EaseType.InOutElastic].Ease(from, to, delta);
            return result;
        }

        // Misc

        public static float LerpSin(float from, float to, float delta, float strength)
        {
            var result = EaseType.FunctionDic[EaseType.Sin].Ease(from, to, delta, strength);
            return result;
        }

        public static float LerpCos(float from, float to, float delta, float strength)
        {
            var result = EaseType.FunctionDic[EaseType.Cos].Ease(from, to, delta, strength);
            return result;
        }

        public static float LerpFlash(float from, float to, float delta, float strength)
        {
            var result = EaseType.FunctionDic[EaseType.Flash].Ease(from, to, delta, strength);
            return result;
        }

        public static float LerpStep(float from, float to, float delta, float strength)
        {
            var result = EaseType.FunctionDic[EaseType.Step].Ease(from, to, delta, strength);
            return result;
        }

        public static float LerpParabola(float from, float to, float delta, float strength)
        {
            var result = EaseType.FunctionDic[EaseType.Parabola].Ease(from, to, delta, strength);
            return result;
        }
    }
}