using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UIButton))]
public class UIButton : UIBase
{
    [GetComponent, NonSerialized] public Button Button;
    public UnityEvent OnClickEvent;

    protected override void Awake()
    {
        base.Awake();
        RegisterButton();
    }

    public virtual void RegisterButton()
    {
        UIListener.onClick += (go, data) => OnClick();
        UIListener.onDown += (go, data) => OnDown();
        UIListener.onUp += (go, data) => OnUp();
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
    }

    public override void SetInteractable(bool interactable)
    {
        base.SetInteractable(interactable);
        if (Button != null)
        {
            Button.interactable = interactable;
        }
    }

    public virtual void OnClick()
    {
        if (!CanInteractable()) return;
        OnClickImpl();
        OnClickEvent?.Invoke();
    }

    public virtual void OnClickImpl()
    {

    }

    public virtual void OnDown()
    {

    }

    public virtual void OnUp()
    {

    }
}
