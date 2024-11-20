using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[LabelWidth(120)]
public class BreakableConfigData
{
    public bool UseRigidbody = true;
    public float ExplodeForce = 100;
    public Vector3 ExplodeCenter;
    public float ExplodeRange = 5;
    public bool AutoDeActive = true;
    [ShowIf(nameof(AutoDeActive))] public Vector2 DeActiveDelay = new Vector2(3f, 6f);
}