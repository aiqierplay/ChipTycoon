using System;
using UnityEngine;

[Serializable]
public class PathDetector : RayDetector
{
    public override float GetValueWithRay(EntityBase handler, Vector3 direction, RaycastHit? rayHitInfo)
    {
        if (rayHitInfo == null) return 0f;
        var angle = Vector3.Angle(handler.Forward, direction);
        var value = Mathf.Lerp(1f, 0f, angle / 180f);
        return value;
    }
}
