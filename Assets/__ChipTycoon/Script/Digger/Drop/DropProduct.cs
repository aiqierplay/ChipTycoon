using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropProduct : DropBase
{
    [ValueDropdown(nameof(TypeGetter))]
    public string Type;

    public IEnumerable TypeGetter() => ProductSetting.Ins.GetValueDropdownKeyList();

    public override void GetImpl()
    {
        World.Factory01.Output.Add(1);
    }

    public override void DeSpawn()
    {
        World.DiggerArea.DropProductList.Remove(this);
        base.DeSpawn();
    }
}
