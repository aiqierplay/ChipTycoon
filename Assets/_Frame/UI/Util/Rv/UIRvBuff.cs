using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRvBuff : UIPage<UIRvBuff>
{
    public Image Icon;
    public Text Description;

    public ItemRvBuff Item { get; set; }

    public override void Show(params object[] args)
    {
        base.Show(args);
        Item = args[0] as ItemRvBuff;

        if (Item == null) return;
        
        var data = GeneralSetting.Ins.BuffItemDic[Item.Type];
        Icon.sprite = data.Icon;

        if (Description != null)
        {
            Description.text = data.Description;
        }
    }

    public void ShowRewardedVideo()
    {
        SDKUtil.RewardVideo("Use Buff " + Item.Type.Type.Name, UseBuff, Back, Back);
    }

    public void UseBuff()
    {
        Item.Target.Buff.AddBuff(Item.Type, Item.Duration, Item.Args);
        Back();
        Item.Complete();
        Item.DeSpawn();
    }
}
