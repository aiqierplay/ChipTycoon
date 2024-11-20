using System.Collections;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;

public class UISceneScrollList : UIBase
{
    public SceneScrollList ScrollList;

    protected override void Awake()
    {
        base.Awake();

        UIEventListener.Get(gameObject).onDown += (go, data) => OnDown();
        UIEventListener.Get(gameObject).onUp += (go, data) => OnUp();
    }

    public override void Init()
    {
        base.Init();
    }

    public virtual void OnDown()
    {
        ScrollList.OnMouseDown();
    }

    public virtual void OnUp()
    {
        ScrollList.OnMouseUp();
    }

    public virtual void Update()
    {

    }
}
