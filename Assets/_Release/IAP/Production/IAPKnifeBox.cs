using System;
using UnityEngine;

[Serializable]
public class IAPKnifeBox : IAPProduction
{
    public int Count;

    public const string SaveKey = "IAP_KnifeBox_RemainCount";

    public static int RemainCount
    {
        get => PlayerPrefs.GetInt(SaveKey, 0);
        set
        {
            PlayerPrefs.SetInt(SaveKey, value);
            PlayerPrefs.Save();
        }
    }

    public override void Enable()
    {
        
    }

    public override void OnBuySuccess()
    {
        RemainCount += Count;
    }

    public override void OnBuyFail()
    {
       
    }
}
