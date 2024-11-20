using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIListItem<TData> : UIBase
{
    [BoxGroup("List Item")] public Image Icon;
    [BoxGroup("List Item")] public GameObject SelectTip;

    [GetComponentInChildren, NonSerialized] public Button Button;
    [NonSerialized] public UIList List;
    [NonSerialized] public int Index;
    [NonSerialized] public TData Data;

    public bool IsSelected => List.CurrentIndex == Index;

    protected override void Awake()
    {
        base.Awake();
        if (Button != null)
        {
            Button.onClick.AddListener(() => Select());
        }
    }

    public void Init(UIList list, int index, TData data)
    {
        List = list;
        Index = index;
        Data = data;
    }

    public virtual bool Select()
    {
        if (!CanSelect()) return false;
        List.CurrentIndex = Index;
        List.Refresh();
        return true;
    }

    public virtual bool CanSelect() => true;

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        if (SelectTip != null)
        {
            SelectTip.gameObject.SetActive(IsSelected);
        }
    }
}
