using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemKey : ItemBase<Player>
{
    [BoxGroup("Key")] public int Value = 1;

    public override void OnTargetEffect(Player target)
    {
        if (!target.IsPlayer) return;
        UIFlyIcon.Ins.Fly(UIFlyIcon.Key, WorldToUiPosition(), Value, () =>
        {
            Save.Key.Value += 1;
        });
    }
}
