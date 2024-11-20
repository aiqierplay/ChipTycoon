using System;
using Aya.Maths;
using UnityEngine;

[Serializable]
public class RangeMapVector2 : RangeMap<float, Vector2>
{
    public override Vector2 Lerp(float srcValue)
    {
        var factor = (srcValue - SrcFrom) / (SrcTo - SrcFrom);
        factor = Curve.Evaluate(factor);
        var result = Vector2.Lerp(DstFrom, DstTo, factor);
        return result;
    }

    public override float ClampSrc(float srcValue)
    {
        return MathUtil.Clamp(srcValue, SrcFrom, SrcTo);
    }

    public override Vector2 ClampDst(Vector2 dstValue)
    {
        return MathUtil.Clamp(dstValue, DstFrom, DstTo);
    }
}