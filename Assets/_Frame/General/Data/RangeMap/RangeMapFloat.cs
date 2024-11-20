using System;
using UnityEngine;

[Serializable]
public class RangeMapFloat : RangeMap<float, float>
{
    public override float Lerp(float srcValue)
    {
        var factor = (srcValue - SrcFrom) / (SrcTo - SrcFrom);
        factor = Curve.Evaluate(factor);
        var result = Mathf.Lerp(DstFrom, DstTo, factor);
        return result;
    }

    public override float ClampSrc(float srcValue)
    {
        return Mathf.Clamp(srcValue, SrcFrom, SrcTo);
    }

    public override float ClampDst(float dstValue)
    {
        return Mathf.Clamp(dstValue, DstFrom, DstTo);
    }
}