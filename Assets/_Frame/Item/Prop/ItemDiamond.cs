using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Aya.Extension;
using Aya.Media.Audio;

public class ItemDiamond : ItemBase<Player>
{
    [BoxGroup("Diamond")] public int Value = 1;
    [BoxGroup("Diamond")] public GameObject Render;

    public override void OnTargetEffect(Player target)
    {
        if (!target.IsPlayer) return;
        UIFlyIcon.Ins.Fly(UIFlyIcon.Diamond, WorldToUiPosition(), Value, () =>
        {
            Save.Diamond.Value += 1;
        });
    }
}
