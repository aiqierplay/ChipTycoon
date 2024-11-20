using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum TranslucencyMode
{
    FadeAlpha = 0,
    SwitchMaterial = 1,
    ActiveRenderer = 2,
}

public class OcclusionTranslucency : EntityBase
{
    [BoxGroup]
    public TranslucencyMode Mode = TranslucencyMode.FadeAlpha;

    [BoxGroup("Fade Alpha"), ShowIf(nameof(Mode), TranslucencyMode.FadeAlpha)]
    public string ColorProperty = "_Color";

    [BoxGroup("Fade Alpha"), ShowIf(nameof(Mode), TranslucencyMode.FadeAlpha)]
    public float OcclusionAlpha = 0.5f;

    [BoxGroup("Fade Alpha"), ShowIf(nameof(Mode), TranslucencyMode.FadeAlpha)]
    public float FadeDuration = 0.15f;

    [BoxGroup("Switch Material"), ShowIf(nameof(Mode), TranslucencyMode.SwitchMaterial)]
    public Material OcclusionMaterial;

    [BoxGroup("Param")]
    public float CheckDistance = 100f;

    [BoxGroup("Param")]
    public LayerMask CheckLayer;

    [NonSerialized] public Transform CameraTrans;
    [NonSerialized] public static Dictionary<Transform, OcclusionTargetData> OcclusionDic = new Dictionary<Transform, OcclusionTargetData>();

    protected override void Awake()
    {
        base.Awake();
        CameraTrans = Camera.Camera.transform;
    }

    private readonly HashSet<Transform> _hitTargets = new HashSet<Transform>();

    public virtual void Update()
    {
        _hitTargets.Clear();
        var direction = Position - CameraTrans.position;
        var raycastHits = Physics.RaycastAll(CameraTrans.position, direction, CheckDistance, CheckLayer.value);
        if (raycastHits != null && raycastHits.Length > 0)
        {
            foreach (var hit in raycastHits)
            {
                var target = hit.transform;
                _hitTargets.Add(target);
                if (!OcclusionDic.TryGetValue(target, out var targetData))
                {
                    targetData = new OcclusionTargetData(target, ColorProperty);
                    OcclusionDic.Add(target, targetData);
                }

                HideTarget(targetData);
            }
        }

        foreach (var kv in OcclusionDic)
        {
            var target = kv.Key;
            if (_hitTargets.Contains(target)) continue;
            var targetData = kv.Value;
            ShowTarget(targetData);
        }
    }

    public virtual void ShowTarget(OcclusionTargetData targetData)
    {
        switch (Mode)
        {
            case TranslucencyMode.FadeAlpha:
                if (!targetData.IsFading)
                {
                    targetData.FadeAlpha(ColorProperty, targetData.NormalAlpha, FadeDuration);
                }

                break;
            case TranslucencyMode.SwitchMaterial:
                targetData.SwitchMaterial(false, OcclusionMaterial);
                break;
            case TranslucencyMode.ActiveRenderer:
                targetData.SetRenderer(false);
                break;
        }
    }

    public virtual void HideTarget(OcclusionTargetData targetData)
    {
        switch (Mode)
        {
            case TranslucencyMode.FadeAlpha:
                if (!targetData.IsFading)
                {
                    targetData.FadeAlpha(ColorProperty, OcclusionAlpha, FadeDuration);
                }

                break;
            case TranslucencyMode.SwitchMaterial:
                targetData.SwitchMaterial(true, OcclusionMaterial);
                break;
            case TranslucencyMode.ActiveRenderer:
                targetData.SetRenderer(true);
                break;
        }
    }
}