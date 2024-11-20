using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProductTypeData : SettingDataBase
{
    public string Name;
    public Sprite Icon;
    public Product Prefab;
    public int CostCoin;
}
