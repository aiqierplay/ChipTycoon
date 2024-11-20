#if SuperSonic
using Aya.Async;
using SupersonicWisdomSDK;
using UnityEngine;

public class SuperSonicInit : MonoBehaviour
{
    public void Awake()
    {
        Init();
    }

    #region Init

    public void Init()
    {
        // Subscribe
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
#if SuperSonic && SuperSonic_Stage_3
        SupersonicWisdom.Api.AddOnIapRestorePurchaseInitializationListener(IAPSuperSonic.OnInit);
#endif
        // Then initialize
        SupersonicWisdom.Api.Initialize();
    }

    public void OnSupersonicWisdomReady()
    {
        // Start your game from this point
        UnityThread.ExecuteUpdate(() =>
        {
            AppManager.Ins.OnLoadComplete();
        });
    }

    #endregion
}
#endif
