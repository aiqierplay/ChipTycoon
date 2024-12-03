using Aya.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropCoinCopier : DropTriggerBase
{
    public int CopyCount;

    public override void OnEnterImpl(DropBase dropItem)
    {
        DiggerArea.StartCoroutine(ImplCo(dropItem));
    }

    public IEnumerator ImplCo(DropBase dropItem)
    {
        var prefab = dropItem.Prefab;
        for (var i = 0; i < CopyCount - 1; i++)
        {
            var coin = GamePool.Spawn(prefab, CurrentLevel.Trans, dropItem.Position);
            coin.Prefab = prefab;
            coin.Value = dropItem.Value;
            DropList.Add(coin);
            yield return null;
        }
    }
}
