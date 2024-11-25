using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDigger : BuildingBase
{
    public override void OnEnterImpl(Worker worker)
    {
        worker.DisableMove();
        World.EnterDigger();
    }

    public override void OnExitImpl(Worker worker)
    {
       
    }

    public override void OnWorkImpl(Worker worker)
    {
       
    }
}
