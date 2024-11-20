using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Reflection;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public abstract class ItemUpgradable<TTarget> : ItemBase<TTarget>
    where TTarget : EntityBase
{
    [Title("Upgrade")] [TypeReference(typeof(ConfigData))]
    public TypeReference UpgradeDataType;
    public Transform RenderPos;
    public TMP_Text TextLevel;
    public TMP_Text TextExp;
    public TMP_Text TextRequireExp;
    public Transform TransExpProgress;
    public GameObject UpgradeFx;
    public string PrefabProperty = "Prefab";
    public string ExpProperty = "RequireExp";

    [Title("Level")]
    public List<GameObject> LevelObjList;

    [NonSerialized] public ConfigData ConfigData;
    [NonSerialized] public int DataLevel;
    [NonSerialized] public int DataMaxLevel;
    [NonSerialized] public int Exp;
    [NonSerialized] public int RequireExp;

    [NonSerialized] public EntityBase RendererInstance;

    public float UpgradeProgress => Mathf.Clamp01(Exp * 1f / RequireExp);

    public virtual int GetDefaultDataLevel()
    {
        return 1;
    }

    public virtual Type GetUpgradeDataType()
    {
        return UpgradeDataType;
    }

    public virtual ConfigData GetUpgradeData()
    {
        var data = Config.GetData(GetUpgradeDataType(), DataLevel - 1);
        return data;
    }

    public override void Init()
    {
        base.Init();
        DataLevel = GetDefaultDataLevel();
        DataMaxLevel = Config.GetDataList(GetUpgradeDataType()).Count;
        RefreshData();

        RendererInstance = null;
        RefreshUpgradeRender();
    }

    public override void Refresh()
    {
        base.Refresh();

        if (Exp >= RequireExp)
        {
            Exp = RequireExp;
            RefreshUpgradeRender();

            DataLevel++;
            RefreshData();
            OnUpgrade();
            // this.ExecuteDelay(() =>
            // {
            //     RefreshUpgradeRender();
            // }, 0.1f);
        }
        else
        {
            RefreshUpgradeRender();
        }
    }

    public virtual void RefreshData()
    {
        if (DataLevel > DataMaxLevel) DataLevel = DataMaxLevel;
        ConfigData = GetUpgradeData();
        Exp = 0;
        RequireExp = (int)ConfigData.GetField(ExpProperty);
    }

    public virtual EntityBase GetPrefab()
    {
        var prefab = (EntityBase)ConfigData.GetField(PrefabProperty);
        if (prefab != null)
        {
            return prefab;
        }

        return null;
    }

    public virtual void RefreshUpgradeRender()
    {
        if (TextLevel != null) TextLevel.text = DataLevel.ToString();
        if (TextExp != null) TextExp.text = Exp.ToString();
        if (TextRequireExp != null) TextRequireExp.text = RequireExp.ToString();
        if (TransExpProgress != null) TransExpProgress.SetLocalScaleX(UpgradeProgress);

        if (RendererInstance != null)
        {
            GamePool.DeSpawn(RendererInstance);
        }

        var prefab = GetPrefab();
        if (prefab != null)
        {
            RendererInstance = GamePool.Spawn(prefab, RenderPos);
        }

        if (LevelObjList != null && LevelObjList.Count > 0)
        {
            var index = DataLevel - 1;
            index = Mathf.Clamp(index, 0, LevelObjList.Count - 1);
            for (var i = 0; i < LevelObjList.Count; i++)
            {
                var levelObj = LevelObjList[i];
                levelObj.SetActive(index == i);
            }
        }
    }

    public virtual void OnUpgrade()
    {
        SpawnFx(UpgradeFx);
    }
}
