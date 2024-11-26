using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Async;
using TMPro;
using UnityEngine;

public class BuildingDigger : BuildingBase
{
    public TMP_Text TextCountDown;

    [NonSerialized] public Coroutine EnterCo;

    public override void Init()
    {
        base.Init();
        TextCountDown.text = "";
    }

    public override void OnEnterImpl(Worker worker)
    {
        EnterCo = StartCoroutine(EnterDiggerCo(worker));
    }

    public override void OnExitImpl(Worker worker)
    {
        TextCountDown.text = "";
        if (EnterCo != null)
        {
            StopCoroutine(EnterCo);
            EnterCo = null;
        }
    }

    public override void OnWorkImpl(Worker worker)
    {
      
    }

    public IEnumerator EnterDiggerCo(Worker worker)
    {
        for (var i = 3; i >= 1; i--)
        {
            TextCountDown.text = i.ToString();
            yield return YieldBuilder.WaitForSeconds(1f);
        }

        TextCountDown.text = "";
        worker.DisableMove();
        World.EnterDigger();
    }
}
