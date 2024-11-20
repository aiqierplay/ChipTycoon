using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Work= 0,
    Digger = 1,
}

public class World : EntityBase
{
    [GetComponentInChildren, NonSerialized]
    public new DiggerArea DiggerArea;

    [NonSerialized] public GameMode Mode;

    public void Init()
    {
        DiggerArea.Init();
    }
}
