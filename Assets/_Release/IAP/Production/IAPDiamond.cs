using System;
using Aya.Util;

[Serializable]
public class IAPDiamond : IAPProduction
{
    public int Value;

    public override void OnBuySuccess()
    {
        UIFlyIcon.Ins.Fly(UIFlyIcon.Diamond, AppManager.Ins.Player.WorldToUiPosition(), RandUtil.RandInt(10, 20), () => { }, () =>
        {
            SaveManager.Ins.Coin.Value += Value;
        });
    }

    public override void OnBuyFail()
    {
        
    }

    public override void Enable()
    {
        
    }
}
