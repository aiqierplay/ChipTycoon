using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Product : EntityBase
{
    [ValueDropdown(nameof(TypeGetter))]
    public string Type;

    [NonSerialized] public bool IsWorking;

    public IEnumerable TypeGetter() => ProductSetting.Ins.GetValueDropdownKeyList();
    
    public ProductTypeData TypeData => ProductSetting.Ins.DataDic[Type];
}