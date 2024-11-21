using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : EntityBase
{
    public virtual void Init()
    {

    }

    public virtual void OnEnter(Character character)
    {
        character.OnEnter(this);
    }

    public virtual void OnExit(Character character)
    {
        character.OnExit(this);
    }

    public virtual void OnWork(Character character)
    {

    }
}
