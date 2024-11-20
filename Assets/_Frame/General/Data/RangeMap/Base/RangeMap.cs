using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class RangeMap<TSrc, TDst>
{
    [BoxGroup("Map/Src", ShowLabel = false), HorizontalGroup("Map/Src/Src"), LabelWidth(60)] public TSrc SrcFrom;
    [BoxGroup("Map/Src"), HorizontalGroup("Map/Src/Src"), LabelWidth(60)] public TSrc SrcTo;

    [BoxGroup("Map/Dst", ShowLabel = false), HorizontalGroup("Map/Dst/Dst"), LabelWidth(60)] public TDst DstFrom;
    [BoxGroup("Map/Dst"), HorizontalGroup("Map/Dst/Dst"), LabelWidth(60)] public TDst DstTo;

    [BoxGroup("Map", ShowLabel = false), LabelWidth(60)] public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

    public abstract TDst Lerp(TSrc srcValue);
    public abstract TSrc ClampSrc(TSrc srcValue);
    public abstract TDst ClampDst(TDst dstValue);
}