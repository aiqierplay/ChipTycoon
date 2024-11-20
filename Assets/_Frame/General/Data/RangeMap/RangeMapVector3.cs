using System;
using Aya.Maths;
using UnityEngine;

[Serializable]
public class RangeMapVector3 : RangeMap<float, Vector3>
{
    public override Vector3 Lerp(float srcValue)
    {
        var factor = (srcValue - SrcFrom) / (SrcTo - SrcFrom);
        factor = Curve.Evaluate(factor);
        var result = Vector3.Lerp(DstFrom, DstTo, factor);
        return result;
    }

    public override float ClampSrc(float srcValue)
    {
        return MathUtil.Clamp(srcValue, SrcFrom, SrcTo);
    }

    public override Vector3 ClampDst(Vector3 dstValue)
    {
        return MathUtil.Clamp(dstValue, DstFrom, DstTo);
    }
}