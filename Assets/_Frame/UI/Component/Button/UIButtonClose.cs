using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonClose : UIButton
{
    public override void OnClickImpl()
    {
        UI.Current.Back();
    }
}
