using System;
using System.Globalization;
using Aya.Maths;
using UnityEngine.UI;

public class UIDiamondBigNumber : UIBase
{
    [GetComponentInChildren, NonSerialized] public Text Text;

    private decimal _lastDiamond;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Refresh();
    }

    public void Update()
    {
        var coin = Save.Diamond.Value;
        if (coin != _lastDiamond)
        {
            Refresh();
        }
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        if (Save == null) return;
        var coin = Save.Diamond.Value;
        _lastDiamond = (int)coin;
        var numberStr = _lastDiamond.ToString(CultureInfo.InvariantCulture);
        var value = BigNumber.Format(numberStr);
        Text.text = value;
    }
}