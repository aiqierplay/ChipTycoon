using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Product : EntityBase
{
    [ValueDropdown(nameof(TypeGetter))]
    public string Type;

    public IEnumerable TypeGetter() => ProductSetting.Ins.GetValueDropdownKeyList();
}