using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlyIconItem : UIBase
{
    public Text Value;

    public override void Init()
    {
        base.Init();
        SetValue(null);
    }

    public void SetValue(string value)
    {
        if (Value == null) return;
        Value.text = value;
    }
}
