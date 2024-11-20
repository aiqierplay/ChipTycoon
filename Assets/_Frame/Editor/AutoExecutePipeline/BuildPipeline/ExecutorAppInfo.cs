#if UNITY_EDITOR
using System;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEditor;

[Serializable, HideMonoScript]
public class ExecutorAppInfo : BuildExecutorBase
{
    public string ProduceName;
    public string CompanyName;
    public string PackageName;
    public string Version;
    public int BundleCode;

    public override void Execute()
    {
        PlayerSettings.productName = ProduceName;
        PlayerSettings.companyName = CompanyName;
        PlayerSettings.applicationIdentifier = PackageName;
        PlayerSettings.bundleVersion = Version;
        switch (CurrentPlatform)
        {
            case BuildTarget.Android:
                PlayerSettings.Android.bundleVersionCode = BundleCode;
                break;
            case BuildTarget.iOS:
                PlayerSettings.iOS.buildNumber = BundleCode.ToString();
                break;
        }
    }

    public override bool CanLoad => true;

    public override void Load()
    {
        ProduceName = PlayerSettings.productName;
        CompanyName = PlayerSettings.companyName;
        PackageName = PlayerSettings.applicationIdentifier;
        Version = PlayerSettings.bundleVersion;

        switch (CurrentPlatform)
        {
            case BuildTarget.Android:
                BundleCode = PlayerSettings.Android.bundleVersionCode;
                break;
            case BuildTarget.iOS:
                var build = PlayerSettings.iOS.buildNumber;
                if (string.IsNullOrEmpty(build)) BundleCode = 0;
                BundleCode = build.AsInt();
                break;
        }

    }
}
#endif