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
        ColliderListenerEnter.Clear();
        ColliderListenerEnter.onTriggerEnter.Add<Digger>(OnEnter, LayerManager.Ins.Player);
    }

    public virtual bool CheckCanBreak()
    {
        if (IsBroken) return false;
        if (RequirePower <= 0) return true;
        return CurrentLevel.Info.Power >= RequirePower;
    }

    public virtual void OnEnterForce(Digger digger, bool force)
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

    public virtual void OnEnter(Digger digger)
    {
        OnEnterForce(digger, false);
    }

    public abstract void OnEnterImpl(Digger digger);
}
