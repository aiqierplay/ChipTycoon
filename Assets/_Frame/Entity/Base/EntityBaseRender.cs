using System;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class EntityBase
{
    [GetComponentInChildren, NonSerialized] public Renderer Renderer;
    [GetComponentInChildren, NonSerialized] public SkinnedMeshRenderer SkinnedMeshRenderer;

    public virtual void CacheRendererComponent()
    {
        Renderer = GetComponentInChildren<Renderer>();
        SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    [NonSerialized] public static Dictionary<object, MaterialPropertyBlock> MaterialPropertyBlockCacheDic = new Dictionary<object, MaterialPropertyBlock>();

    public static MaterialPropertyBlock GetMaterialPropertyBlock(Renderer render, int materialIndex = 0)
    {
        if (MaterialPropertyBlockCacheDic.TryGetValue(render, out var result)) return result;
        result = new MaterialPropertyBlock();
        render.GetPropertyBlock(result, materialIndex);
        MaterialPropertyBlockCacheDic.Add(render, result);
        return result;
    }
}
