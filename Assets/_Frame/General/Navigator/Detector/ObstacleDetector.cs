using System;
using UnityEngine;

[Serializable]
public class ObstacleDetector : RayDetector
{
    public override float GetValueWithRay(EntityBase handler, Vector3 direction, RaycastHit? rayHitInfo)
    {
        if (rayHitInfo == null) return 1f;
        var distance = (handler.Position - rayHitInfo.Value.point).magnitude;
        var value = distance / CheckDistance;
        return value;
    }
}