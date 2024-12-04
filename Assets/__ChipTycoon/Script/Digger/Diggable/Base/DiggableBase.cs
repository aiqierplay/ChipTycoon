using Aya.Physical;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiggableBase : EntityBase
{
    public int RequirePower;
    public GameObject FxBreak;

    [GetComponentInChildren, NonSerialized] public ColliderListenerEnter ColliderListenerEnter;
    [NonSerialized] public bool IsBroken;

    public virtual void Init()
    {
        IsBroken = false;
        gameObject.SetActive(true);
        if (ColliderListenerEnter != null)
        {
            ColliderListenerEnter.Clear();
            ColliderListenerEnter.onTriggerEnter.Add<DiggerTool>(OnEnter, LayerManager.Ins.Player);
        }
    }

    public virtual bool CheckCanBreak()
    {
        if (IsBroken) return false;
        if (World.DiggerArea.DiggerTool.Mode != DiggerToolMode.Digger) return false;
        if (RequirePower <= 0) return true;
        return CurrentLevel.Info.Power >= RequirePower;
    }

    public virtual void OnEnterForce(DiggerTool digger, bool force)
    {
        if (!force && !CheckCanBreak()) return;
        if (!force)
        {
            CurrentLevel.Info.Power -= RequirePower;
        }
      
        SpawnFx(FxBreak, CurrentLevel.Trans, Position);
        OnEnterImpl(digger);
        gameObject.SetActive(false);
        IsBroken = true;
    }

    public virtual void OnEnter(DiggerTool digger)
    {
        OnEnterForce(digger, false);
    }

    public abstract void OnEnterImpl(DiggerTool digger);
}
