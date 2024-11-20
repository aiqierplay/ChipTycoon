using System;
using Aya.Extension;
using TMPro;
using UnityEngine;

public class EntityProgress : EntityBase
{
    [Range(0, 1)]
    public float Progress = 0.5f;
    public float LerpSpeed = 5f;

    public Transform ProgressTrans;
    public TMP_Text TextValue;
    public TMP_Text TextMax;
    public TMP_Text TextProgress;

    [NonSerialized] public float Value;
    [NonSerialized] public float Max;

    public void SetProgress(float progress, bool immediately = false)
    {
        Progress = progress;
        Refresh(immediately);
    }

    public void SetValue(float value, float max, bool immediately = false)
    {
        Value = value;
        Max = max;
        Progress = Mathf.Clamp01(value / max);
        Refresh(immediately);
    }

    [NonSerialized] public bool IsInLerp;

    public void Refresh(bool immediately = false)
    {
        if (ProgressTrans != null)
        {
            if (immediately)
            {
                IsInLerp = false;
                ProgressTrans.SetLocalScaleX(Progress);
            }
            else
            {
                var from = ProgressTrans.localScale.x;
                var to = Progress;
                if (Math.Abs(from - to) > 1e-6f)
                {
                    IsInLerp = true;
                }
                else
                {
                    IsInLerp = false;
                }

                var value = Mathf.Lerp(from, to, DeltaTime * LerpSpeed);
                ProgressTrans.SetLocalScaleX(value);
            }
        }

        if (TextValue != null)
        {
            TextValue.text = Mathf.RoundToInt(Value).ToString();
        }

        if (TextMax != null)
        {
            TextMax.text = Mathf.RoundToInt(Max).ToString();
        }

        if (TextProgress != null)
        {
            TextProgress.text = (Progress * 100f).ToString("F0") + "%";
        }
    }

    public void LateUpdate()
    {
        if (IsInLerp) Refresh(false);
    }

    public void OnValidate()
    {
        Refresh();
    }
}