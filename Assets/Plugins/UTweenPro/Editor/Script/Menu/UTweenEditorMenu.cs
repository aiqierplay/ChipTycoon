#if UNITY_EDITOR
using UnityEditor;

namespace Aya.TweenPro
{
    public partial class UTweenEditorMenu
    {
        // [MenuItem("Tools/Aya Game/UTween Pro/Tweener Monitor", false, 0)]
        // public static void OpenTweenerMonitor()
        // {
        //
        // }

        [MenuItem("Tools/Aya Game/UTween Pro/Runtime Setting", false, 1000)]
        public static void OpenRuntimeSetting()
        {
            SettingsService.OpenProjectSettings("Aya Game/UTween Pro Runtime");
        }

        [MenuItem("Tools/Aya Game/UTween Pro/Editor Setting", false, 1001)]
        public static void OpenEditorSetting()
        {
            SettingsService.OpenProjectSettings("Aya Game/UTween Pro Editor");
        }
    }
}
#endif