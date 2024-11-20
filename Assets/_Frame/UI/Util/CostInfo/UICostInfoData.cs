using Aya.Maths;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

[BoxGroup]
[Serializable]
public class UICostInfoData
{
    public GameObject ActiveRoot;
    public TMP_Text TextValue;
    public bool ShowBigNumber;

    [NonSerialized] public float LastValue = -1;
    [NonSerialized] public Func<float> ValueGetter;

    public void Refresh()
    {
        Refresh(ValueGetter);
    }

    public void Refresh(Func<float> valueGetter)
    {
        ValueGetter = valueGetter;
        if (ValueGetter == null) return;
        var value = ValueGetter();
        Refresh(value);
    }

    public void Refresh(float value)
    {
        if (Math.Abs(value - LastValue) < 1e-6f) return;
        LastValue = value;
        var show = value > 0;
        if (ActiveRoot != null)
        {
            ActiveRoot.SetActive(show);
        }

        if (show)
        {
            var displayValue = Mathf.RoundToInt(value).ToString();
            if (ShowBigNumber)
            {
                displayValue = BigNumber.Format(displayValue);
            }

            if (TextValue != null)
            {
                TextValue.text = displayValue;
            }
        }
        else
        {
            if (TextValue != null)
            {
                TextValue.text = "0";
            }
        }
    }
}