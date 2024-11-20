using System;
using TMPro;
using UnityEngine;

public class UIOfflineEarnings : UIPage<UIOfflineEarnings>
{
    public TMP_Text TextTime;
    public TMP_Text TextValue;

    [NonSerialized] public bool HasGetEarn;

    public override void Show(params object[] args)
    {
        base.Show(args);

        if (TextValue != null)
        {
            var value = OfflineEarnings.Ins.GetEarnValue();
            TextValue.text = Mathf.RoundToInt(value).ToString();
        }

        if (TextTime != null)
        {
            var time = OfflineEarnings.Ins.GetTime();
            TextTime.text = time;
        }

        HasGetEarn = false;
    }

    public void GetEarn()
    {
        OfflineEarnings.Ins.GetEarn();
    }
}
