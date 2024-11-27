using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrade : UIPage<UIUpgrade>
{
    public override void Back()
    {
        base.Back();
        World.Character.EnableMove();
    }
}
