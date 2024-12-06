using System;
using Aya.Physical;
using System.Collections;
using System.Collections.Generic;
using Aya.TweenPro;
using UnityEngine;

public class TriggerArea : EntityBase
{
    [GetComponentInChildren, NonSerialized] public ColliderListenerEnter Enter;
    [GetComponentInChildren, NonSerialized] public ColliderListenerExit Exit;

    public UTweenPlayer TweenEnter;
    public UTweenPlayer TweenExit;


    public virtual void Init()
    {
        if (Enter != null)
        {
            Enter.Clear();
        }

        if (Exit != null)
        {
            Exit.Clear();
        }
    }

    public void OnEnter()
    {
        if (TweenEnter != null) TweenEnter.Play();
    }

    public void OnExit()
    {
        if (TweenExit != null) TweenExit.Play();
    }
}
