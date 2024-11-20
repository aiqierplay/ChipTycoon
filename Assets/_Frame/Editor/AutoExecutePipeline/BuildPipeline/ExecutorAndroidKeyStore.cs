#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEditor;
using Object = UnityEngine.Object;

[Serializable, HideMonoScript]
public class ExecutorAndroidKeyStore : BuildExecutorBase
{
    public Object Keystore;
    public string Password;
    public string AliasName;
    public string AliasPassword;

    public override void Execute()
    {
        var path = AssetDatabase.GetAssetPath(Keystore);
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = path;
        PlayerSettings.Android.keystorePass = Password;
        PlayerSettings.Android.keyaliasName = AliasName;
        PlayerSettings.Android.keyaliasPass = AliasPassword;
    }

    public override bool CanLoad => true;

    public override void Load()
    {
        Keystore = AssetDatabase.LoadAssetAtPath(PlayerSettings.Android.keystoreName, typeof(Object));
        Password = PlayerSettings.Android.keystorePass;
        AliasName = PlayerSettings.Android.keyaliasName;
        AliasPassword = PlayerSettings.Android.keyaliasPass;
    }
}
#endif