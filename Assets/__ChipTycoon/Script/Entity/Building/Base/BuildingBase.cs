using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Physical;
using UnityEngine;

public abstract class BuildingBase : EntityBase
{
    [GetComponentInChildren, NonSerialized]
    public ColliderListenerEnter ColliderListenerEnter;

    [GetComponentInChildren, NonSerialized]
    public ColliderListenerExit ColliderListenerExit;

    [NonSerialized] public List<Worker> WorkerList = new List<Worker>();

    public virtual void Init()
    {
        ColliderListenerEnter.Clear();
        ColliderListenerEnter.onTriggerEnter.Add<Worker>(OnEnter, LayerManager.Ins.Player);
        ColliderListenerExit.Clear();
        ColliderListenerExit.onTriggerExit.Add<Worker>(OnExit, LayerManager.Ins.Player);

        WorkerList.Clear();
    }

    public virtual void Refresh()
    {

    }

    public virtual void OnEnter(Worker worker)
    {
        worker.OnEnter(this);
        OnEnterImpl(worker);
    }

    public virtual void OnExit(Worker worker)
    {
        worker.OnExit(this);
        OnExitImpl(worker);
    }

    public virtual void OnWork(Worker worker)
    {
        OnWorkImpl(worker);
    }

    public abstract void OnEnterImpl(Worker worker);
    public abstract void OnExitImpl(Worker worker);
    public abstract void OnWorkImpl(Worker worker);
}
