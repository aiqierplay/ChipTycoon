using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Physical;
using Aya.Util;
using UnityEngine;

public class DiggableOre : DiggableBase
{
    public DropBase DropPrefab;
    public int RewardValue;

    public override void OnEnterImpl(DiggerTool digger)
    {
        if (RewardValue <= 0) return;
        var dropItem = GamePool.Spawn(DropPrefab, CurrentLevel.Trans, Position + RandUtil.RandVector3(-0.05f, 0.05f));
        World.DropList.Add(dropItem);
        dropItem.Init();
        dropItem.Prefab = DropPrefab;
        dropItem.Value = RewardValue;
    }
}
