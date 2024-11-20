
using System;
using UnityEngine;

public class GrassPlaceholder : EntityBase
{
    // public float Radius = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();
        LastPosition = Position;
        GrassManager.Ins.Register(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GrassManager.Ins.DeRegister(this);
    }

    [NonSerialized] public Vector3 LastPosition = Vector3.zero;

    public void LateUpdate()
    {
        var currentPosition = Position;
        if (LastPosition.Equals(currentPosition)) return;
        LastPosition = currentPosition;
        GrassManager.Ins.RequestRefreshPlaceHolder();
    }
}