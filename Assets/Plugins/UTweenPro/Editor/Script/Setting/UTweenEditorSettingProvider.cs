#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public static class UTweenEditorSettingProvider
    {
        #region Project Setting

        [SettingsProvider]
        public static SettingsProvider GetEditorSetting()
        {
            var provider = AssetSettingsProvider.CreateProviderFromObject("Aya Game/UTween Pro Editor", UTweenEditorSetting.Ins);
            provider.keywords = SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(UTweenEditorSetting.Ins));
            return provider;
        }

        [SettingsProvider]
        public static SettingsProvider GetRuntimeSetting()
        {
            var provider = AssetSettingsProvider.CreateProviderFromObject("Aya Game/UTween Pro Runtime", UTweenSetting.Ins);
            provider.keywords = SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(UTweenSetting.Ins));
            return provider;
        } 

        #endregion
    }
}

#endif