using System;
using System.Globalization;
using Aya.Maths;
using UnityEngine.UI;

public class UICoinBigNumber : UIBase
{
    [GetComponentInChildren, NonSerialized] public Text Text;

    private decimal _lastCoin;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Refresh();
    }

    public void Update()
    {
        var coin = Save.Coin.Value;
        if (coin != _lastCoin)
        {
            Refresh();
        }
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        if (Save == null) return;
        var coin = Save.Coin.Value;
        _lastCoin = (int)coin;
        var numberStr = _lastCoin.ToString(CultureInfo.InvariantCulture);
        var value = BigNumber.Format(numberStr);
        Text.text = value;
    }
}