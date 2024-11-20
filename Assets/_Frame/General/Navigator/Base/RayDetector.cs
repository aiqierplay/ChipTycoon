using UnityEngine;

public abstract class RayDetector : Detector
{
    public LayerMask Layer;
    public float CheckDistance = 10f;

    public override float GetValueImpl(EntityBase handler, Vector3 direction)
    {
        var ray = new Ray(handler.Position, direction);
        if (Physics.Raycast(ray, out var hitInfo, CheckDistance, Layer.value))
        {
            return GetValueWithRay(handler, direction, hitInfo);
        }

        return GetValueWithRay(handler, direction, null);
    }

    public abstract float GetValueWithRay(EntityBase handler, Vector3 direction, RaycastHit? rayHitInfo);
}