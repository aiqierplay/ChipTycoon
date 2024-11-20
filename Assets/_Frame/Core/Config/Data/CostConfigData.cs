using System;

[Serializable]
public abstract class CostConfigData : ConfigData
{
    [DataFieldOrder(1000)] public int CostCoin;
    [DataFieldOrder(1001)] public int CostDiamond;

    public virtual bool Enough => SaveManager.Ins.Coin >= CostCoin && SaveManager.Ins.Diamond >= CostDiamond;

    public override bool CanUpgrade()
    {
        return Enough;
    }

    public override void Upgrade()
    {
        SaveManager.Ins.Coin.Value -= CostCoin;
        SaveManager.Ins.Diamond.Value -= CostDiamond;
    }
}