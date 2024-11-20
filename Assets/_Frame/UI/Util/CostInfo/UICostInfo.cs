using System;
using Sirenix.OdinInspector;

[HideMonoScript]
public class UICostInfo : UIBase
{
    public bool ShowSaveValue = true;
    public UICostInfoData Coin;
    public UICostInfoData Diamond;
    
    public void Refresh(Func<float> coinGetter, Func<float> diamondGetter)
    {
        Coin.Refresh(coinGetter);
        Diamond.Refresh(diamondGetter);
    }

    public void Refresh(float coin, float diamond)
    {
        Coin.Refresh(coin);
        Diamond.Refresh(diamond);
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        Coin.Refresh();
        Diamond.Refresh();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (ShowSaveValue)
        {
            Refresh(DefaultCoinGetter, DefaultDiamondGetter);
        }
    }

    public float DefaultCoinGetter()
    {
        if (Save == null) return 0f;
        return (float)Save.Coin.Value;
    }

    public float DefaultDiamondGetter()
    {
        if (Save == null) return 0f;
        return (float)Save.Diamond.Value;
    }
}