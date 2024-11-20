using Aya.SDK;
using UnityEngine;

public class IAPManager
{
    public static IAPChannelBase Instance { get; private set; }

    static IAPManager()
    {
    }

    public static void Init()
    {
        SDKDebug.Log("IAP", "Init.");
        CreateSdkInstance();
    }

    private static void CreateSdkInstance()
    {

#if UNITY_ANDROID || UNITY_IOS

#if SuperSonic && SuperSonic_Stage_3
        SDKDebug.Log("IAP", "Init IAP SuperSonic.");
        Instance = new IAPSuperSonic();
#endif

#endif
        if (Instance == null)
        {
            SDKDebug.Log("IAP", "Init Editor.");
            Instance = new IAPEditor();
        }

        var setting = IAPSetting.Ins;
        if (setting == null)
        {
            Debug.LogError("IAPSetting not found!!!");
        }
        else
        {
            Instance.Init(setting);
        }
    }

}
