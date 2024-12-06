using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUpgrade : BuildingBase
{
    public override void OnEnterImpl(Worker worker)
    {
        if (UI.Current is not UIGame) return;
        UI.Show<UIUpgrade>();
        World.Character.DisableMove();
    }

    public override void OnExitImpl(Worker worker)
    {
       
    }
}
