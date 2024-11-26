using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Aya.TweenPro;
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

    public void OnTouchStart(Vector3 pos)
    {
        World.Character.OnTouchStart(pos);
    }

    public void OnTouch(Vector3 pos)
    {
        World.Character.OnTouch(pos);
    }

    public void OnTouchEnd(Vector3 pos)
    {
        World.Character.OnTouchEnd(pos);
    }

    public void Retry()
    {
        Level.LevelStart();
    }
}
