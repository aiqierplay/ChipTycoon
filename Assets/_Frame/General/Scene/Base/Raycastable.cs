using Aya.Physical;
using Aya.Util;
using System;
using UnityEngine;

public abstract class Raycastable : EntityBase
{
    public bool IgnoreUi = false;

    [NonSerialized] public LayerMask SelfLayer;
    [NonSerialized] public float RayCheckDistance = 1000f;

    protected override void Awake()
    {
        base.Awake();
        SelfLayer = LayerUtil.GetLayerMaskByIndex(gameObject.layer);
    }

    public virtual ValueTuple<Transform, Vector3> CheckRaycastTrans()
    {
        if (!IgnoreUi && IsPointerOverUiObject())
        {
            return (null, Vector3.zero);
        }

        var ray = Camera.Camera.ScreenPointToRay(Input.mousePosition);
        var result = PhysicsUtil.Raycast<Transform>(ray.origin, ray.direction, RayCheckDistance, SelfLayer);
        return result;
    }
}
