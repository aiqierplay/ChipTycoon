using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralSetting", menuName = "Setting/General Setting")]
public class GeneralSetting : SettingBase<GeneralSetting>
{
    [Title("Game")]
    public Worker PlayerPrefab;
    public Worker WorkerPrefab;

    public int DefaultCoin;
    public int DefaultDiamond;
    public int DefaultKey;
    public float LoseWaitDuration;
    public float WinWaitDuration;

    [Title("Item"), TableList] 
    public List<BuffItemData> BuffItemList;

    [NonSerialized] public Dictionary<Type, BuffItemData> BuffItemDic;

    public override void Init()
    {
        base.Init();
        BuffItemList.ForEach(i => i.Type = i.Prefab.GetComponent<ItemRvBuff>().Type);
        BuffItemDic = BuffItemList.ToDictionary(i => i.Type);
    }
}
