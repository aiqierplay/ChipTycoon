using System;
using System.Collections;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIPage<UIGame>
{
    [GetComponentInChildren, NonSerialized] public UITouchCanvas TouchCanvas;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        if (TouchCanvas != null) TouchCanvas.Hide();
    }

    public void Update()
    {

    }

    public void Retry()
    {
        Level.LevelStart();
    }
}
