using System;
using Aya.Data.Json;

[Serializable]
public abstract class UnLockableSaveInfoList<TInfo, TData> : SaveInfoList<TInfo>
    where TInfo : UnLockableSaveInfoList<TInfo, TData>, new()
    where TData : CostConfigData
{
    public bool UnLock;
    public decimal UnLockSpent;

    [JsonIgnore, NonSerialized] public TData Data;
    [JsonIgnore] public bool IsLock => !UnLock;
    [JsonIgnore] public bool IsUnLock => UnLock;

    [JsonIgnore] public decimal Cost => Data.CostCoin;
    [JsonIgnore] public decimal RemainCost => Cost - UnLockSpent;
    [JsonIgnore] public bool CanUnLock => IsLock && SaveManager.Ins.Coin >= Cost;

    public override void Init(int index, bool saveByLevel)
    {
        base.Init(index, saveByLevel);
        if (saveByLevel)
        {
            Data = ConfigManager.Ins.GetData<TData>(index, LevelManager.Ins.CurrentLevel.SaveKey);
        }
        else
        {
            Data = ConfigManager.Ins.GetData<TData>(index);
        }
    }

    public virtual void UnLockWithSpent(decimal unLockValue)
    {
        if (!CanUnLock) return;
        if (unLockValue >= SaveManager.Ins.Coin) unLockValue = SaveManager.Ins.Coin;
        if (unLockValue >= RemainCost) unLockValue = RemainCost;
        if (unLockValue <= 0) return;
        UnLockSpent += unLockValue;
        SaveManager.Ins.Coin.Value -= unLockValue;
        if (UnLockSpent >= Cost)
        {
            UnLock = true;
            Save();
        }
    }

    public virtual bool UnLockImmediately()
    {
        if (!CanUnLock) return false;
        UnLock = true;
        SaveManager.Ins.Coin.Value -= Data.CostCoin;
        Save();
        return true;
    }
}