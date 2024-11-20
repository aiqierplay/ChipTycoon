using System;
using Aya.Physical;
using UnityEngine;

public class UIShootPoint : UIBase
{
    public LayerMask ShootLayer;

    [NonSerialized] public Shooter Shooter;

    public void Init(Shooter shooter)
    {
        Shooter = shooter;
    }

    public void LateUpdate()
    {
        if (Shooter == null) return;
        var (target, hitPos) = PhysicsUtil.Raycast<Transform>(Shooter.ShootPoint.position, Shooter.GetShootDirection(), 1000f, ShootLayer);
        if (target != null)
        {
            var uiPos = WorldToUiPosition(hitPos);
            Rect.anchoredPosition = uiPos;
        }
        else
        {
            Rect.anchoredPosition = Vector2.zero;
        }
    }
}
