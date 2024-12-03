using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Physical;
using UnityEngine;

public abstract class BuildingBase : EntityBase
{
    public ColliderListenerEnter ColliderListenerEnter;
    public ColliderListenerExit ColliderListenerExit;

    [NonSerialized] public List<Worker> WorkerList = new List<Worker>();

    public virtual void Init()
    {
        if (ColliderListenerEnter != null)
        {
            ColliderListenerEnter.Clear();
            ColliderListenerEnter.onTriggerEnter.Add<Worker>(OnEnter, LayerManager.Ins.Player);
        }

        if (ColliderListenerExit != null)
        {
            ColliderListenerExit.Clear();
            ColliderListenerExit.onTriggerExit.Add<Worker>(OnExit, LayerManager.Ins.Player);
        }

        WorkerList.Clear();
    }

    public virtual void Refresh()
    {

    }

    public virtual void OnEnter(Worker worker)
    {
        if (worker.Type != WorkerType.Player) return;
        worker.OnEnter(this);
        OnEnterImpl(worker);
    }

    public virtual void OnExit(Worker worker)
    {
        if (worker.Type != WorkerType.Player) return;
        worker.OnExit(this);
        OnExitImpl(worker);
    }

    public abstract void OnEnterImpl(Worker worker);
    public abstract void OnExitImpl(Worker worker);
}
