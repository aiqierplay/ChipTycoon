using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ItemBreakMode
{
    All = 0,
    Partial = 1,
}

public abstract class ItemBreakable<T> : ItemBase<T> where T : EntityBase
{
    [FoldoutGroup("Breakable")] public bool EnableBreak;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public ItemBreakMode BreakMode;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public BreakableConfigData BreakableConfig;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public Transform NormalTrans;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public Transform BrokenTrans;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public GameEffect BrokenEffect;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public GameObject BrokenTotalFx;
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public GameObject BrokenNodeFx;

    // 爆炸准备，仅完整爆炸时生效
    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public bool EnablePrepareExplode;
    public bool ShowPrepareExplodeParam => EnableBreak && EnablePrepareExplode;
    [FoldoutGroup("Breakable"), ShowIf(nameof(ShowPrepareExplodeParam))] public float PrepareDelay = 1f;
    [FoldoutGroup("Breakable"), ShowIf(nameof(ShowPrepareExplodeParam))] public GameEffect PrepareEffect;
    [FoldoutGroup("Breakable"), ShowIf(nameof(ShowPrepareExplodeParam))] public UTweenPlayerReference PrepareTween;
    [FoldoutGroup("Breakable"), ShowIf(nameof(ShowPrepareExplodeParam))] public GameObject PrepareFx;

    [FoldoutGroup("Breakable"), ShowIf(nameof(EnableBreak))] public List<Transform> BrokenList;

    [NonSerialized] public bool IsCached = false;
    [NonSerialized] public List<BreakableData> CacheBrokenDataList = new List<BreakableData>();
    [NonSerialized] public List<BreakableData> CurrentBrokenDataList = new List<BreakableData>();

    public int Count => CacheBrokenDataList.Count;
    public int RemainCount => CurrentBrokenDataList.Count;
    public bool IsFull => CurrentBrokenDataList.Count == CacheBrokenDataList.Count;
    public bool IsBroken => CurrentBrokenDataList.Count == 0;
    public float BreakProgress => 1f - CurrentBrokenDataList.Count * 1f / CacheBrokenDataList.Count;

    protected override void Awake()
    {
        base.Awake();
        CacheBreakableDataList();
    }

    public virtual void CacheBreakableDataList()
    {
        if (IsCached) return;
        CacheBrokenDataList.Clear();
        foreach (var brokenItem in BrokenList)
        {
            var data = new BreakableData(this, brokenItem, BreakableConfig);
            CacheBrokenDataList.Add(data);
        }

        IsCached = true;
    }

    public override void Init()
    {
        base.Init();

        if (!EnableBreak) return;
        CacheBreakableDataList();

        for (var i = 0; i < BrokenList.Count; i++)
        {
            var data = CacheBrokenDataList[i];
            data.Reset();
        }

        if (NormalTrans != null)
        {
            SetNormal(true);
            SetBroken(false);
        }
        else
        {
            SetBroken(true);
        }

        CurrentBrokenDataList.Clear();
        CurrentBrokenDataList.AddRange(CacheBrokenDataList);
    }

    [FoldoutGroup("Breakable"), Button("Auto Cache"), ShowIf(nameof(EnableBreak))]
    public void AutoCache()
    {
        BrokenList = BrokenTrans.GetComponentsInChildren<MeshRenderer>(true).ToList(m => m.transform);
        foreach (var item in BrokenList)
        {
            if (BreakableConfig.UseRigidbody) item.GetOrAddComponent<Rigidbody>();
            var col = item.GetComponentInChildren<Collider>();
            if (col == null)
            {
                var mesh = item.GetOrAddComponent<MeshCollider>();
                mesh.convex = true;
            }
        }
    }

    public override void OnTargetEffect(T target)
    {
        switch (BreakMode)
        {
            case ItemBreakMode.All:
                Explode();
                break;
            case ItemBreakMode.Partial:
                var range = target.Collider.bounds.size.magnitude;
                Explode(target.Position, range);
                break;
        }

        if (CurrentBrokenDataList.IsNullOrEmpty())
        {
            Active = false;
        }
    }

    public virtual void SetNormal(bool show)
    {
        if (NormalTrans != null)
        {
            NormalTrans.gameObject.SetActive(show);
        }
    }

    public virtual void SetBroken(bool show)
    {
        if (BrokenTrans != null)
        {
            BrokenTrans.gameObject.SetActive(show);
        }

        for (var i = 0; i < CacheBrokenDataList.Count; i++)
        {
            var data = CacheBrokenDataList[i];
            data.Transform.gameObject.SetActive(show);
        }
    }

    public virtual void Explode()
    {
        if (!EnableBreak) return;
        if (IsBroken) return;

        if (EnablePrepareExplode)
        {
            if (PrepareEffect != null) PrepareEffect.Play();
            PrepareTween.Play();
            SpawnFx(PrepareFx, CurrentLevel.Trans, Position);
            this.ExecuteDelay(ExplodeAll, PrepareDelay);
        }
        else
        {
            ExplodeAll();
        }
    }

    protected virtual void ExplodeAll()
    {
        SetNormal(false);
        SetBroken(true);
        SpawnFx(BrokenTotalFx, CurrentLevel.Trans, Position);

        for (var i = 0; i < CurrentBrokenDataList.Count; i++)
        {
            var data = CurrentBrokenDataList[i];
            data.Explode(Position);
            SpawnFx(BrokenNodeFx, CurrentLevel.Trans, data.Transform.position);
        }

        CurrentBrokenDataList.Clear();

        OnBroken();
    }

    public virtual void Explode(float progress)
    {
        if (!EnableBreak) return;
        if (IsBroken) return;

        SetNormal(false);
        SetBroken(true);

        var count = Mathf.RoundToInt(CacheBrokenDataList.Count * progress);
        var brokenList = new List<BreakableData>();
        for (var i = 0; i < count && i < CurrentBrokenDataList.Count; i++)
        {
            var data = CurrentBrokenDataList[i];
            data.Explode(Position);
            SpawnFx(BrokenNodeFx, CurrentLevel.Trans, data.Transform.position);
            brokenList.Add(data);
        }

        foreach (var breakableData in brokenList)
        {
            CurrentBrokenDataList.Remove(breakableData);
        }

        OnBroken();
    }

    public virtual bool Explode(Vector3 position, float range)
    {
        if (!EnableBreak) return false;
        if (IsBroken) return false;

        var hit = false;
        var dataList = CurrentBrokenDataList.FindAll(d => (d.Transform.position - position).sqrMagnitude <= range * range);
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            data.Explode(Position);
            SpawnFx(BrokenNodeFx, CurrentLevel.Trans, data.Transform.position);
            CurrentBrokenDataList.Remove(data);
            hit = true;
        }

        OnBroken();
        return hit;
    }



    public virtual void Explode(BreakableData data)
    {
        if (!EnableBreak) return;
        if (IsBroken) return;

        if (!CurrentBrokenDataList.Contains(data)) return;
        data.Explode(Position);
        SpawnFx(BrokenNodeFx, CurrentLevel.Trans, data.Transform.position);
        CurrentBrokenDataList.Remove(data);
        OnBroken();
    }

    public virtual void OnBroken()
    {
        if (!IsBroken) return;
        if (BrokenEffect != null) BrokenEffect.Play();
    }
}
