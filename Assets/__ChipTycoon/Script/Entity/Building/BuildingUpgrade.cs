using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUpgrade : BuildingBase
{
    public override void OnEnterImpl(Worker worker)
    {
        UI.Show<UIUpgrade>();
        World.Character.DisableMove();
    }

    public override void OnExitImpl(Worker worker)
    {
       
    }

    public override void OnWorkImpl(Worker worker)
    {
       
    }
}
