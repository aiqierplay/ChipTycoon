using System;
using Aya.TweenPro;
using UnityEngine;

[Serializable]
public class OcclusionTargetData
{
    public Transform Target;
    public Renderer Renderer;

    public bool EnableColorProperty;
    public string ColorProperty;
    public float NormalAlpha;
    public float CurrentAlpha;
    public TweenMaterialPropertyAlpha FadeTween;

    public Material OriginalMaterial;

    public bool IsFading => FadeTween != null;

    public OcclusionTargetData(Transform target, string colorProperty)
    {
        Target = target;
        Renderer = target.GetComponentInChildren<Renderer>();

        OriginalMaterial = Renderer.material;
        ColorProperty = colorProperty;
        EnableColorProperty = OriginalMaterial.HasProperty(ColorProperty);
        if (EnableColorProperty)
        {
            NormalAlpha = OriginalMaterial.GetColor(ColorProperty).a;
        }
        else
        {
            ColorProperty = null;
        }

        CurrentAlpha = 1f;
    }

    public void FadeAlpha(string key, float targetAlpha, float duration)
    {
        if (!EnableColorProperty) return;
        if (Math.Abs(targetAlpha - CurrentAlpha) < 1e-6f) return;
        if (IsFading) return;

        FadeTween = UTween.Alpha(Renderer, key, CurrentAlpha, targetAlpha, duration)
            .SetMaterialMode(TweenMaterialMode.Property)
            .SetOnStop(() =>
            {
                FadeTween = null;
                CurrentAlpha = targetAlpha;
            });
    }

    public void SwitchMaterial(bool isOcclusion, Material occlusionMaterial)
    {
        var mat = isOcclusion ? occlusionMaterial : OriginalMaterial;
        Renderer.material = mat;
    }

    public void SetRenderer(bool isOcclusion)
    {
        Renderer.enabled = !isOcclusion;
    }
}