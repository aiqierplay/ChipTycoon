using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DiggerToolData
{
    public DiggerToolMode Mode;
    public GameObject Target;
    public UTweenPlayerReference TweenWork;

    public void Active()
    {
        Target.SetActive(true);
    }

    public void DeActive()
    {
        Target.SetActive(false);
    }

    public void Start()
    {
        TweenWork.Play();
    }

    public void Stop()
    {
        TweenWork.Stop();
    }
}
