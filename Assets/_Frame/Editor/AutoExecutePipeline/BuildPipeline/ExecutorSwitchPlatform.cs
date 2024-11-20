#if UNITY_EDITOR
using System;
using UnityEditor;

[Serializable]
public class ExecutorSwitchPlatform : BuildExecutorBase
{
    public BuildTarget Platform;

    public override void Execute()
    {
        if (CurrentPlatform != Platform)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(ConvertBuildTarget(Platform), Platform);
        }
    }
}
#endif