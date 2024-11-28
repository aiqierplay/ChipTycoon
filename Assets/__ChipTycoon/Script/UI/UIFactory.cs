using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFactory : UIPage<UIFactory>
{
    public override void Back()
    {
        base.Back();
        World.Character.EnableMove();
    }
}
