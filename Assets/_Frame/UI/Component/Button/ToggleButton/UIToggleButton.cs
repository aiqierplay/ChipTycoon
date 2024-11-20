using System;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIToggleButton : UIBase
{
    [Title("State")]
    public bool DefaultState = true;

    [Title("Toggle Object")] 
    public GameObject On;
    public GameObject Off;

    [Title("Toggle Image")] 
    public Image ImgOn;
    public Image ImgOff;
    public float FadeDuration = 0.15f;

    [NonSerialized] public bool ToggleState;

    [NonSerialized] public UIToggleButtonGroup ToggleButtonGroup;
    [GetComponent, NonSerialized] public Button Button;

    protected override void Awake()
    {
        base.Awake();
        ToggleButtonGroup = null;
        ToggleState = DefaultState;
        Button.onClick.AddListener(OnClick);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshToggleState(true);
    }

    public virtual bool GetState()
    {
        return ToggleState;
    }

    public virtual void SetState(bool state, bool immediately = false)
    {
        if (state == ToggleState) return;
        ToggleState = state;
        Refresh(immediately);
    }

    public virtual void OnClick()
    {
        var state = !GetState();
        SetState(state);
        Refresh();
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        RefreshToggleState(immediately);
    }

    public virtual void RefreshToggleState(bool immediately = false)
    {
        var state = GetState();
        if (On != null) On.SetActive(state);
        if (Off != null) Off.SetActive(!state);

        if (ImgOn != null)
        {
            var targetAlpha = state ? 1f : 0f;
            if (immediately)
            {
                ImgOn.SetA(targetAlpha);
            }
            else
            {
                ImgOn.TweenAlpha(targetAlpha, FadeDuration);
            }
        }

        if (ImgOff != null)
        {
            var targetAlpha = state ? 0f : 1f;
            if (immediately)
            {
                ImgOff.SetA(targetAlpha);
            }
            else
            {
                ImgOff.TweenAlpha(targetAlpha, FadeDuration);
            }
        }

        if (ToggleButtonGroup != null && state)
        {
            ToggleButtonGroup.RefreshToggleGroup(this);
        }
    }
}
