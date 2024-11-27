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
        
    }
}
