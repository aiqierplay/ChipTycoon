using System.Collections;
using System.Collections.Generic;
using Aya.Util;
using UnityEngine;

public class DropCoin : DropBase
{
    public override void Get()
    {
        UIFlyIcon.Ins.Fly(UIFlyIcon.Coin, WorldToUiPosition(), 1, null, () =>
        {
            Save.Coin.Value += Value;
        });
    }
}
