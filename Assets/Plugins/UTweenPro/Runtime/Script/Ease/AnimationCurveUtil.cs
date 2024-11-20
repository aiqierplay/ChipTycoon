using UnityEngine;

namespace Aya.TweenPro
{
    public static class AnimationCurveUtil
    {
        public static readonly float SampleStep = 0.01f;

        // public static AnimationCurve ConvertTp2Tv(AnimationCurve tpCurve)
        // {
        //     var tvCurve = new AnimationCurve();
        //     for (var t = 0f; t < tpCurve.keys[tpCurve.length - 1].time; t += SampleStep)
        //     {
        //         var pos = tpCurve.Evaluate(t);
        //         var nextPos = tpCurve.Evaluate(t + SampleStep);
        //         var velocity = (nextPos - pos) / SampleStep;
        //         var key = new Keyframe(t, velocity);
        //         key.inTangent = key.outTangent = 1f;
        //         tvCurve.AddKey(key);
        //     }
        //
        //     return tvCurve;
        // }

        public static AnimationCurve ConvertTv2Tp(AnimationCurve tvCurve)
        {
            var tpCurve = new AnimationCurve();
            var pos = 0f;
            for (var t = 0f; t < tvCurve.keys[tvCurve.length - 1].time; t += SampleStep)
            {
                var velocity = tvCurve.Evaluate(t);
                pos += velocity * SampleStep;
                var key = new Keyframe(t, pos);
                key.inTangent = key.outTangent = 0f;
                tpCurve.AddKey(key);
            }

            tpCurve = ScaleCurve01(tpCurve);
            return tpCurve;
        }

        public static AnimationCurve ConvertTa2Tp(AnimationCurve taCurve)
        {
            var tpCurve = new AnimationCurve();
            var pos = 0f;
            var velocity = 0f;
            for (var t = 0f; t < taCurve.keys[taCurve.length - 1].time; t += SampleStep)
            {
                var acceleration = taCurve.Evaluate(t);
                velocity += acceleration * SampleStep;
                pos += velocity * SampleStep;
                var key = new Keyframe(t, pos);
                key.inTangent = key.outTangent = 0f;
                tpCurve.AddKey(key);
            }

            tpCurve = ScaleCurve01(tpCurve);
            return tpCurve;
        }

        public static AnimationCurve ScaleCurve01(AnimationCurve tpCurve)
        {
            var maxValue = 0f;
            for (var t = 0f; t <= 1f; t+= SampleStep)
            {
                var value = tpCurve.Evaluate(t);
                if (value > maxValue) maxValue = value;
            }

            if (Mathf.Abs(maxValue) > 1e-6f)
            {
                var scale = 1f / maxValue;
                for (var i = 0; i < tpCurve.keys.Length; i++)
                {
                    var key = tpCurve.keys[i];
                    key.value *= scale;
                    tpCurve.MoveKey(i, key);
                }
            }
            
            return tpCurve;
        }
    }
}