﻿using System.Collections;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;

public class UIReady : UIPage<UIReady>
{
    public GameObject TouchTip;
    public GameObject TouchStartRunArea;
    public UIGameProgress GameProgress;

    private bool _clickStartRun;

    protected override void Awake()
    {
        base.Awake();
        UIEventListener.Get(TouchStartRunArea).onDown += (go, data) =>
        {
            if (!_clickStartRun)
            {
                _clickStartRun = true;
                App.Enter<GameStart>();
                if (Player != null) Player.Move.EnableMove();
                TouchTip.gameObject.SetActive(false);
            }
        };
    }

    public override void Show(params object[] args)
    {
        base.Show(args);

        _clickStartRun = false;
        TouchTip.gameObject.SetActive(true);

        GameProgress?.Init();
    }
}
