using System;
using Aya.Extension;
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
        Target.transform.ResetLocal();
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
