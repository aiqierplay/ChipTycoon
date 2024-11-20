using Aya.Particle;
using UnityEngine;

public abstract partial class EntityBase
{
    #region Fx

    public virtual ParticleSpawner SpawnFx(GameObject fxPrefab)
    {
        var trans = RendererTrans;
        if (trans == null) trans = Trans;
        return SpawnFx(fxPrefab, trans);
    }

    public virtual ParticleSpawner SpawnFx(GameObject fxPrefab, Transform parent)
    {
        if (parent == null) parent = RendererTrans;
        if (parent == null) parent = Trans;
        return SpawnFx(fxPrefab, parent, parent.position);
    }

    public virtual ParticleSpawner SpawnFx(GameObject fxPrefab, Transform parent, Vector3 position)
    {
        if (fxPrefab == null) return default;
        var particleSpawner = ParticleSpawner.Spawn(fxPrefab, parent, position);
        return particleSpawner;
    }

    public virtual ParticleSpawner SpawnUiFx(GameObject fxPrefab, RectTransform parent)
    {
        if (fxPrefab == null) return default;
        var particleSpawner = ParticleSpawner.Spawn(fxPrefab, parent, Vector3.zero);
        return particleSpawner;
    }

    public virtual ParticleSpawner SpawnUiFx(GameObject fxPrefab, RectTransform parent, Vector3 position)
    {
        if (fxPrefab == null) return default;
        var particleSpawner = ParticleSpawner.Spawn(fxPrefab, parent, position);
        return particleSpawner;
    }

    #endregion
}