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

    public void OnTouchStart(Vector3 pos)
    {
        switch (World.Mode)
        {
            case GameMode.Work:
                World.Character.OnTouchStart(pos);
                break;
            case GameMode.Digger:
                DiggerArea.Digger.OnTouchStart(pos);
                break;
        }
       
    }

    public void OnTouch(Vector3 pos)
    {
        switch (World.Mode)
        {
            case GameMode.Work:
                World.Character.OnTouch(pos);
                break;
            case GameMode.Digger:
                DiggerArea.Digger.OnTouch(pos);
                break;
        }
      
    }

    public void OnTouchEnd(Vector3 pos)
    {
        switch (World.Mode)
        {
            case GameMode.Work:
                World.Character.OnTouchEnd(pos);
                break;
            case GameMode.Digger:
                DiggerArea.Digger.OnTouchEnd(pos);
                break;
        }
    
    }

    public void Retry()
    {
        Level.LevelStart();
    }
}
