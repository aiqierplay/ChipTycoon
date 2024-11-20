using UnityEngine;
using Aya.SDK;

namespace Aya.AD
{
    public class ADManager
    {
        public static ADChannelBase Instance { get; private set; }

        static ADManager()
        {
        }

        public static void Init()
        {
            SDKDebug.Log("AD", "Init.");
            CreateSdkInstance();
        }

        private static void CreateSdkInstance()
        {

#if UNITY_ANDROID || UNITY_IOS
#if APPLOVIN
            SDKDebug.Log("AD", "Init AD AppLovin MAX.");
            Instance = new ADAppLovin();
#endif
#if TapNation
            SDKDebug.Log("AD", "Init AD TapNation.");
            Instance = new ADTapNation();
#endif
#if SuperSonic && (SuperSonic_Stage2 || SuperSonic_Stage3)
            SDKDebug.Log("AD", "Init AD SuperSonic.");
            Instance = new ADSuperSonic();
#endif
// #if CrazyLab
//             SDKDebug.Log("Analysis CrazyLab");
//             Instance = new AnalysicCrazyLab();
// #endif

#endif
            if (Instance == null)
            {
                SDKDebug.Log("AD", "Init Editor.");
                Instance = new ADEditor();
            }

            var setting = ADSetting.Ins;
            if (setting == null)
            {
                Debug.LogError("ADSetting not found!!!");
            }
            else
            {
                Instance.Init(setting);
            }
        }

    }
}
