using System;
using System.Collections.Generic;
using System.Reflection;
using Aya.Events;
using Aya.Extension;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum UpgradeDataMode
{
    Default = 0,
    // 指定配置资源文件
    Asset = 1,
    // 指定配置资源路径
    Path = 2,
}

public enum UpgradeSaveMode
{
    // 默认 无存储 Key
    Default = 0,
    // 自定义存储附加 Key
    Custom = 1,
}

public class UIUpgradeButton : UIBase
{
    public UpgradeInfoReference UpgradeInfo;

    [BoxGroup("Info")] public TMP_Text Description;
    [BoxGroup("Info")] public TMP_Text LevelText;
    [BoxGroup("Info")] public Image LevelProgress;
    [BoxGroup("Info")] public GameObject MaxLevelTip;
    [BoxGroup("Info")] [TableList] public List<UIUpgradePropertyValueItem> ValueList;

    [BoxGroup("Button")] public Button UpgradeBtn;
    [BoxGroup("Button")] public UnityEvent OnUpgradeEvent;

    [BoxGroup("Fx")] public GameObject FxUpgrade;

    [BoxGroup("Param")] public List<GameEvent> TriggerRefreshEvent;
    [BoxGroup("Param")] public List<GameEvent> DispatchUpgradeEvent;

    [BoxGroup("Reward Video")] public string RvKey;
    [BoxGroup("Reward Video")] public Button RvUpgradeBtn;
    [BoxGroup("Reward Video")] public UnityEvent OnRvUpgradeEvent;

    [GetComponentInChildren, NonSerialized] public UICostInfo CostInfo;

    public int LevelIndex => UpgradeInfo.Data.ID;
    public bool IsMaxLevel => UpgradeInfo.Info.IsMaxLevel;
    public bool CanUpgrade => UpgradeInfo.Info.CanUpgrade && CheckCanUpgradeAction();
    public int MaxLevel => UpgradeInfo.Info.Count;

    public Func<bool> CheckCanUpgradeAction = () => true;

    protected override void Awake()
    {
        base.Awake();

        if (UpgradeBtn != null)
        {
            UpgradeBtn.onClick.AddListener(OnUpgradeClick);
        }

        if (RvUpgradeBtn != null)
        {
            RvUpgradeBtn.onClick.AddListener(OnUpgradeRvClick);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Upgrade == null) return;
        foreach (var refreshEvent in TriggerRefreshEvent)
        {
            UEvent.Listen(refreshEvent, RefreshData);
        }

        if (RefreshOnEnable) RefreshData();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var refreshEvent in TriggerRefreshEvent)
        {
            UEvent.Remove(refreshEvent, RefreshData);
        }
    }

    public virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(UpgradeInfo.Type)) return;
        var dataType = Assembly.GetExecutingAssembly().GetType(UpgradeInfo.Type);
        foreach (var item in ValueList)
        {
            if (item == null) continue;
            item.DataType = dataType;
        }
    }

    [Listen(GameEvent.Upgrade, GameEvent.LoadLevel)]
    public virtual void RefreshData()
    {
        if (UpgradeInfo == null) return;
        if (!UpgradeInfo.IsInit) UpgradeInfo.Init();
        Refresh();
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        if (UpgradeInfo == null || UpgradeInfo.Info == null) return;
        if (Description != null)
        {
            Description.text = UpgradeInfo.Data.Description;
        }

        if (LevelText != null)
        {
            LevelText.text = UpgradeInfo.Data.ID.ToString();
        }

        var previewData = UpgradeInfo.Info.Preview;
        var nextData = UpgradeInfo.Info.Next;
        foreach (var valueItem in ValueList)
        {
            if (valueItem.Preview != null)
            {
                if (previewData == null) valueItem.Preview.text = "";
                else valueItem.Preview.text = previewData.GetField(valueItem.Property).ToString();
            }

            if (valueItem.Current != null)
            {
                valueItem.Current.text = UpgradeInfo.Data.GetField(valueItem.Property).ToString();
            }

            if (valueItem.Next != null)
            {
                if (IsMaxLevel)
                {
                    valueItem.Next.text = "";
                }
                else
                {
                    if (nextData == null) valueItem.Next.text = "";
                    else valueItem.Next.text = nextData.GetField(valueItem.Property).ToString();
                }
            }
        }

        if (CostInfo != null)
        {
            CostInfo.Refresh(UpgradeInfo.Data.CostCoin, UpgradeInfo.Data.CostDiamond);
        }

        var progress = 0f;
        var showUpgradeBtn = !IsMaxLevel;
        progress = LevelIndex * 1f / MaxLevel;

        if (LevelProgress != null)
        {
            LevelProgress.fillAmount = progress;
        }

        if (UpgradeBtn != null)
        {
            UpgradeBtn.interactable = CanUpgrade;
            UpgradeBtn.gameObject.SetActive(showUpgradeBtn);
        }

        if (RvUpgradeBtn != null)
        {
            RvUpgradeBtn.interactable = SDKUtil.IsRewardVideoReady(RvKey);
            RvUpgradeBtn.gameObject.SetActive(showUpgradeBtn);
        }

        if (MaxLevelTip != null)
        {
            MaxLevelTip.SetActive(IsMaxLevel);
        }
    }

    public virtual void OnUpgradeClick()
    {
        UpgradeImpl(false);
        SDKUtil.Event("Upgrade ", "Type", UpgradeInfo.Type, "Level", LevelIndex);
    }

    public virtual void OnUpgradeRvClick()
    {
        SDKUtil.RewardVideo(RvKey, () =>
        {
            UpgradeImpl(true);
            SDKUtil.Event("Upgrade Rv", "Type", UpgradeInfo.Type, "Level", LevelIndex);
        });
    }

    public virtual void UpgradeImpl(bool isRv)
    {
        SpawnFx(FxUpgrade, UI.AlwaysTrans, Position);
        if (isRv)
        {
            UpgradeInfo.Info.ForceUpgrade();
            OnRvUpgradeEvent?.Invoke();
        }
        else
        {
            UpgradeInfo.Info.Upgrade();
            OnUpgradeEvent?.Invoke();
        }

        Dispatch(GameEvent.Upgrade);
        foreach (var gameEvent in DispatchUpgradeEvent)
        {
            Dispatch(gameEvent);
        }

        UpgradeInfo.IsInit = false;
        RefreshData();
    }
}