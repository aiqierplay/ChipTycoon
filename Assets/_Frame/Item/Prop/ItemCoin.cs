using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemCoin : ItemBase<Player>
{
    [BoxGroup("Coin")] public int Value = 1;

    public override void OnTargetEffect(Player target)
    {
        if (!target.IsPlayer) return;
        UIFlyIcon.Ins.Fly(UIFlyIcon.Coin, WorldToUiPosition(), Value, () =>
        {
            Save.Coin.Value += 1;
        });
    }
}
