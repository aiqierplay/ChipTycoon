using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Physical;
using Aya.Util;
using UnityEngine;

public class DiggableOre : DiggableBase
{
    public int RewardCoin;

    public override void OnEnterImpl(Digger digger)
    {
        if (RewardCoin <= 0) return;
        var prefab = GeneralSetting.Ins.DropCoinPrefab;
        var coin = GamePool.Spawn(prefab, CurrentLevel.Trans, Position + RandUtil.RandVector3(-0.05f, 0.05f));
        World.DropList.Add(coin);
        coin.Prefab = prefab;
        coin.Value = RewardCoin;
    }
}
