using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlatformSwitcherData
{
    [TableColumnWidth(70, false)] public PlatformType Platform;
    public List<GameObject> TargetList;
}
