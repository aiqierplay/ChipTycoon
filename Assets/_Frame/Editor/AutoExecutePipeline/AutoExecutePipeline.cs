#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Editor Tools/Auto Execute Pipeline", fileName = "AutoExecutePipeline")]
public class AutoExecutePipeline : ScriptableObject
{
    [SerializeReference] public List<ExecutorBase> ExecutorList = new List<ExecutorBase>();

    [Button(nameof(Execute))]
    public void Execute()
    {
        try
        {
            for (var i = 0; i < ExecutorList.Count; i++)
            {
                var buildPhase = ExecutorList[i];
                buildPhase.Init(this);
            }

            for (var i = 0; i < ExecutorList.Count; i++)
            {
                var buildPhase = ExecutorList[i];
                buildPhase.Before();
            }

            for (var i = 0; i < ExecutorList.Count; i++)
            {
                var buildPhase = ExecutorList[i];
                buildPhase.Execute();
            }

            for (var i = 0; i < ExecutorList.Count; i++)
            {
                var buildPhase = ExecutorList[i];
                buildPhase.After();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
}
#endif
