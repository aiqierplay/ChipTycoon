using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "SDKSetting", menuName = "Setting/SDK Setting")]
public class SDKSetting : SettingBase<SDKSetting>
{
    public bool EventLog;
    public string BannerKey;
    public string InterstitialKey;
}
