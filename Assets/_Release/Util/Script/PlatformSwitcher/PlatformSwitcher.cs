using System.Collections.Generic;
using Sirenix.OdinInspector;

public enum PlatformType
{
    Windows = 0,
    Android = 1,
    [LabelText("iOS")] iOS = 2,
}

public class PlatformSwitcher : EntityBase
{
    [TableList] public List<PlatformSwitcherData> DataList;

    protected override void Awake()
    {
        base.Awake();
        SwitchPlatform();
    }

    public void SwitchPlatform()
    {
#if UNITY_STANDALONE_WIN
        SwitchPlatform(PlatformType.Windows);
#elif UNITY_ANDROID
        SwitchPlatform(PlatformType.Android);
#elif UNITY_IOS
        SwitchPlatform(PlatformType.iOS);
#endif
    }

    public void SwitchPlatform(PlatformType platformType)
    {
        foreach (var data in DataList)
        {
            foreach (var target in data.TargetList)
            {
                target.SetActive(data.Platform == platformType);
            }
        }
    }
}
