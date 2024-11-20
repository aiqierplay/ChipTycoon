using System;
using System.Collections.Generic;
using Aya.Util;
using UnityEngine;

public class GrassNode : EntityBase
{
    public int MaterialIndex = 0;
    public string ExtrusionProperty = "_Extrusion";
    public string PlaceholderPositionsProperty = "_PlaceholderPositions";
    public string PlaceholderCountProperty = "_PlaceholderCount";
    public float ExtrusionRadius = 1f;
    public float ExtrusionPower = 2f;

    public bool RandRotate;

    [NonSerialized]
    public static Dictionary<object, MaterialPropertyBlock> RenderPropertyBlockDic = new Dictionary<object, MaterialPropertyBlock>();

    public MaterialPropertyBlock GetPropertyBlock(Renderer render)
    {
        if (RenderPropertyBlockDic.TryGetValue(render, out var result)) return result;
        result = new MaterialPropertyBlock();
        render.GetPropertyBlock(result, MaterialIndex);
        RenderPropertyBlockDic.Add(render, result);
        return result;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GrassManager.Ins.Register(this);
        if (RandRotate)
        {
            LocalEulerAngles = new Vector3(0, RandUtil.RandInt(0, 4) * 90f, 0f);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GrassManager.Ins.DeRegister(this);
    }

    // Old
    public virtual void Refresh()
    {
        var dis = float.MaxValue;
        foreach (var placeholder in GrassManager.Ins.PlaceholderList)
        {
            var disTemp = (placeholder.Position - Position).sqrMagnitude;
            if (disTemp < dis) dis = disTemp;
        }

        if (dis > ExtrusionRadius)
        {
            var extrusion = 0f;
            var value = extrusion * ExtrusionPower;
            var property = GetPropertyBlock(Renderer);
            property.SetFloat(ExtrusionProperty, value);
            Renderer.SetPropertyBlock(property, MaterialIndex);
        }
        else
        {
            var extrusion = (1f - dis / ExtrusionRadius);
            var value = extrusion * ExtrusionPower;
            var property = GetPropertyBlock(Renderer);
            property.SetFloat(ExtrusionProperty, value);
            Renderer.SetPropertyBlock(property, MaterialIndex);
        }
    }

    public virtual void Refresh(Vector4[] placeholderArray, int placeholderCount)
    {
        // var property = GetPropertyBlock(Renderer);
        // property.SetVectorArray(PlaceholderPositionsProperty, placeholderArray);
        // property.SetFloat(PlaceholderCountProperty, placeholderCount);
        // Renderer.SetPropertyBlock(property, MaterialIndex);

        Renderer.sharedMaterial.SetVectorArray(PlaceholderPositionsProperty, placeholderArray);
        Renderer.sharedMaterial.SetFloat(PlaceholderCountProperty, placeholderCount);
    }
}