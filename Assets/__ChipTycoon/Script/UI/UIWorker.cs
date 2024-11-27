using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorker : UIPage<UIWorker>
{
    public override void Back()
    {
        base.Back();
        World.Character.EnableMove();
    }
}