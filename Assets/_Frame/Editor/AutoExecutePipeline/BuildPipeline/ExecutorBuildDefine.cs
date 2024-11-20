#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEditor;

[Serializable, HideMonoScript]
public class ExecutorBuildDefine : BuildExecutorBase
{
    public List<string> AddDefineList;
    public List<string> RemoveDefineList;

    public override void Execute()
    {
        var defineList = GetDefineList(CurrentPlatform);
        foreach (var define in AddDefineList)
        {
            if (!defineList.Contains(define))
            {
                defineList.Add(define);
            }
        }

        foreach (var define in RemoveDefineList)
        {
            if (!defineList.Contains(define))
            {
                defineList.Remove(define);
            }
        }

        SetDefineList(CurrentPlatform, defineList);
    }
    
    // public override bool CanLoad => true;
    //
    // public override void Load()
    // {
    //     DefineList = GetDefineList(CurrentTarget);
    // }

    public List<string> GetDefineList(BuildTarget buildTarget)
    {
        var buildTargetGroup = ConvertBuildTarget(buildTarget);
        var defineList = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';').ToList();
        return defineList;
    }

    public void SetDefineList(BuildTarget buildTarget, List<string> defineList)
    {
        var defineStr = "";
        for (var i = 0; i < defineList.Count; i++)
        {
            var define = defineList[i];
            defineStr += define;
            if (i < defineList.Count - 1)
            {
                defineStr += ";";
            }
        }

        var buildTargetGroup = ConvertBuildTarget(buildTarget);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineStr);
    }
}
#endif