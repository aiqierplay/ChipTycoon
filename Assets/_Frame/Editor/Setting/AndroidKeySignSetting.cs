#if UNITY_EDITOR
using Aya.UI.Markup;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AndroidKeySignSetting), menuName = "Editor Setting/Android Key Sign")]
public class AndroidKeySignSetting : EditorSettingBase<AndroidKeySignSetting>
{
    public Object Keystore;
    public string Password;
    public string AliasName;
    public string AliasPassword;

    [ReadOnly, GUIColor(nameof(GetActiveColor))] public bool Active;

    public Color GetActiveColor()
    {
        return Active ? Color.green : Color.red;
    }

    [ButtonGroup, GUIColor(0.5f, 1f, 0.5f), HideIf(nameof(Active))]
    public void ActiveSetting()
    {
        var settingList = LoadAllAsset();
        foreach (var setting in settingList)
        {
            setting.Active = setting == this;
        }

        AssetDatabase.SaveAssets();
    }

    [ButtonGroup, GUIColor(1f, 0.5f, 0.5f), ShowIf(nameof(Active))]
    public void DeActiveSetting()
    {
        Active = false;
        AssetDatabase.SaveAssets();
    }

    public void Load()
    {
        Keystore = AssetDatabase.LoadAssetAtPath(PlayerSettings.Android.keystoreName, typeof(Object));
        Password = PlayerSettings.Android.keystorePass;
        AliasName = PlayerSettings.Android.keyaliasName;
        AliasPassword = PlayerSettings.Android.keyaliasPass;
    }

    [ButtonGroup]
    public void Execute()
    {
        var path = AssetDatabase.GetAssetPath(Keystore);
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = path;
        PlayerSettings.Android.keystorePass = Password;
        PlayerSettings.Android.keyaliasName = AliasName;
        PlayerSettings.Android.keyaliasPass = AliasPassword;

        Debug.Log("[Android Key]".ToMarkup(Color.cyan) + "\t" + path);
    }

    [UnityEditor.Callbacks.PostProcessScene]
    public static void OnBuild()
    {
        var settingList = LoadAllAsset();
        var setting = settingList.Find(s => s.Active);
        if (setting == null) return;
        setting.Execute();
    }

    [UnityEditor.Callbacks.PostProcessBuild]
    public static void OnBuildComplete(BuildTarget buildTarget, string output)
    {

    }
}
#endif