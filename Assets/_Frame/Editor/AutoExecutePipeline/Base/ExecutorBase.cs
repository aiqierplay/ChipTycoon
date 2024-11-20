#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEditor;

[Serializable]
public abstract class ExecutorBase
{
    [NonSerialized] public AutoExecutePipeline Executor;


    public virtual void Init(AutoExecutePipeline execute)
    {
        Executor = execute;
    }

    public virtual void Before()
    {

    }

    [Button("Execute"), ButtonGroup(nameof(ExecutorBase))]
    public abstract void Execute();

    public virtual bool CanUndo => false;

    [Button("Undo"), ButtonGroup(nameof(ExecutorBase)), ShowIf(nameof(CanUndo))]
    public virtual void Undo()
    {
    }

    public virtual bool CanLoad => false;

    [Button("Load"), ButtonGroup(nameof(ExecutorBase)), ShowIf(nameof(CanLoad))]
    public virtual void Load()
    {
    }

    public virtual float GetProgress() => 1f;

    public virtual void After()
    {

    }
}
#endif