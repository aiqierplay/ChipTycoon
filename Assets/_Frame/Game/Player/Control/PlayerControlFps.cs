using System;
using Aya.Extension;
using Aya.Maths;
using UnityEngine;

public class PlayerControlFps : PlayerControl
{
    public override PlayerControlMode ControlMode => PlayerControlMode.Fps;

    public Vector2 RotateDirection = new Vector3(1f, 1f);
    public Vector3 RotateSpeed;
    public Vector3 RotateMin;
    public Vector3 RotateMax;

    public Transform TargetTrans => RendererTrans;

    [NonSerialized] public bool IsDown;
    [NonSerialized] public Vector3 StartDownPos;
    [NonSerialized] public Vector3 StartRotate;
    [NonSerialized] public Vector3 LastDownPos;

    public override void InitComponent()
    {
        base.InitComponent();
        IsDown = false;
        TargetTrans.ResetLocal();
    }

    public override void UpdateImpl(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsDown) return;
            IsDown = true;
            StartDownPos = LastDownPos = Input.mousePosition;
            StartRotate = TargetTrans.localEulerAngles;
            if (StartRotate.x > 180f)
            {
                StartRotate.x -= 360f;
            }

            if (StartRotate.y > 180f)
            {
                StartRotate.y -= 360f;
            }

            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!IsDown) return;
            IsDown = false;
        }

        if (IsDown)
        {
            var last = LastDownPos;
            var current = Input.mousePosition;
            LastDownPos = current;
            var offset = current - last;

            var rotateX = RotateSpeed.x * offset.x * deltaTime * RotateDirection.x;
            var rotateY = RotateSpeed.y * -offset.y * deltaTime * RotateDirection.y;

            StartRotate.x += rotateY;
            StartRotate.y += rotateX;

            var clampX = Mathf.Clamp(StartRotate.x, RotateMin.x, RotateMax.x);
            var clampY = Mathf.Clamp(StartRotate.y, RotateMin.y, RotateMax.y);
            var clampZ = Mathf.Clamp(StartRotate.z, RotateMin.z, RotateMax.z);
            StartRotate = new Vector3(clampX, clampY, clampZ);

            TargetTrans.localEulerAngles = StartRotate;
        }
    }
}
