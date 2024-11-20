using System;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public enum UIPageSate
{
    Show = 0,
    Hide = 1,
}

public abstract class UIPage : UIBase
{
    [NonSerialized] public UIPageSate PageSate;

    [FoldoutGroup("Button")] public Button BtnClose;

    protected override void Awake()
    {
        base.Awake();
        RegisterButton();
    }

    public virtual void RegisterButton()
    {
        if (BtnClose != null)
        {
            BtnClose.onClick.AddListener(Back);
        }
    }

    // 从关闭切换到显示
    public override void Show(params object[] args)
    {
        base.Show(args);
        PageSate = UIPageSate.Show;
        Dispatch(UIEvent.ShowPage, this);
    }

    // 从被遮盖显示恢复到最上层显示
    public virtual void RestoreShow()
    {

    }

    public override void Hide()
    {
        base.Hide();
        PageSate = UIPageSate.Hide;
        Dispatch(UIEvent.HidePage, this);
    }

    public override void Refresh(bool immediately = false)
    {

    }

    public virtual void Back()
    {
        UI.Hide(this);
    }
}