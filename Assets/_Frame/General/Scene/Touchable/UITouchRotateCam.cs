using System;
using UnityEngine;

public class UITouchRotateCam : UITouchSlideArea
{
    public Vector2 RotateSpeed = new Vector2(1f, 1f);
    public bool ClampX;
    public bool ClampY;
    public Vector3 ClampMin;
    public Vector3 ClampMax;

    [NonSerialized] public Vector3 StartEulerAngles;

    public override void Init()
    {
        base.Init();
        StartEulerAngles = Vector3.zero;
    }

    public virtual Transform GetTargetTrans()
    {
        return Camera.Current.Camera.transform;;
    }

    public override void TouchStartImpl(Vector3 touchPoint)
    {
        base.TouchStartImpl(touchPoint);
        var target = GetTargetTrans();
        var eulerAngles = target.eulerAngles;
        StartEulerAngles = eulerAngles;
    }

    public override void OnSliding(Vector3 offset)
    {
        var rotateY = offset.x;
        var rotateX = offset.y;
        var target = GetTargetTrans();
        var eulerAngles = StartEulerAngles;
        eulerAngles += new Vector3(-rotateX * RotateSpeed.x, rotateY * RotateSpeed.y, 0f);
        if (ClampX)
        {
            eulerAngles.x = Mathf.Clamp(eulerAngles.x, ClampMin.x, ClampMax.x);
        }

        if (ClampY)
        {
            eulerAngles.y = Mathf.Clamp(eulerAngles.y, ClampMin.y, ClampMax.y);
        }

        target.eulerAngles = eulerAngles;
    }
}