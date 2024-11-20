using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropBase : EntityBase
{
    public int Value;
    [NonSerialized] public DropBase Prefab;

    public abstract void Get();
}
