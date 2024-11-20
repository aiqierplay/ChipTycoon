#if APPLOVIN
using Aya.AD;
using Aya.SDK;

public abstract class ADAppLovinSource : ADSource
{
    public ADAppLovinBanner Banner
    {
        get
        {
            var source = (ADManager.Instance.Banner as ADAppLovinLocationBanner);
            if (source == null) return null;
            var result = source.CurrentSource as ADAppLovinBanner;
            return result;
        }
    }
}

public class ADAppLovinLocationBanner : ADLocationBase<ADAppLovinBanner>
{
}

public class ADAppLovinLocationInterstitial : ADLocationBase<ADAppLovinInterstitial>
{
}

public class ADAppLovinLocationRewardedVideo : ADLocationBase<ADAppLovinRewardedVideo>
{
}

public class ADAppLovin : ADChannelBase<ADAppLovinLocationBanner, ADAppLovinLocationInterstitial, ADAppLovinLocationRewardedVideo>
{
    public override void Init(params object[] args)
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += _ => {
            OnInit(ADSetting.Ins);
            Load();
        };

        MaxSdk.InitializeSdk();
        MaxSdk.SetHasUserConsent(true);

        SDKDebug.Log("AppLovin Init");
    }

    public override void OnInit(params object[] args)
    {
        base.OnInit(args);
        SDKDebug.Log("AppLovin Init Success");
    }
}
#endif