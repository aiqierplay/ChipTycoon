using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SettingBase<TSetting> : ScriptableObject where TSetting : SettingBase<TSetting>
{
    #region Ins / Load

    public static TSetting Ins
    {
        get
        {
            if (_instance == null) _instance = Load();
            return _instance;
        }
    }

    private static TSetting _instance;

    public static TSetting Load()
    {
        var fileName = typeof(TSetting).Name;
        var settingPrefab = Resources.Load<TSetting>($"Setting/{fileName}");
        if (settingPrefab == null)
        {
#if UNITY_ANDROID
            settingPrefab = Resources.Load<TSetting>($"Setting/Android/{fileName}");
#elif UNITY_IOS
            settingPrefab = Resources.Load<TSetting>($"Setting/iOS/{fileName}");
#endif
        }

        if (settingPrefab == null)
        {
            var abPath = ABTestSetting.Ins.GetValue(fileName);
            settingPrefab = Resources.Load<TSetting>($"Setting/{abPath}/{fileName}");
        }

        if (settingPrefab == null) return null;
        var setting = Instantiate(settingPrefab);
        setting.Init();
        return setting;
    }

#if UNITY_EDITOR
    [Button(SdfIconType.ArrowRepeat)]
    public void RefreshLoadEditor()
    {
        _instance = null;
        Load();
    }
#endif

    #endregion

    public virtual void Init()
    {

    }

    public TSetting GetInstance() => Ins;
}