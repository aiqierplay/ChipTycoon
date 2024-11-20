using Aya.Util;
using System;

[Serializable]
public class IAPCoin : IAPProduction
{
    public int Value;

    public override void OnBuySuccess()
    {
        UIFlyIcon.Ins.Fly(UIFlyIcon.Coin, AppManager.Ins.Player.WorldToUiPosition(),  RandUtil.RandInt(10, 20), ()=>{}, () =>
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
