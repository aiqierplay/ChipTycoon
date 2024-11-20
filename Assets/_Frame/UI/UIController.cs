using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIController : EntityBase<UIController>
{
    public ABTestValueReference<RectTransform> RootAbTest;

    public new Camera Camera;
    public new Canvas Canvas;

    public RectTransform RootTrans;
    public RectTransform AlwaysTrans;

    protected override void Awake()
    {
        base.Awake();

        if (RootAbTest.EnableAbTest)
        {
            foreach (var data in RootAbTest.List)
            {
                data.RefValue.gameObject.SetActive(data.IsCurrentConfig);
            }

            RootTrans = RootAbTest.GetValue();
        }

        if (RootTrans == null) RootTrans = Rect;
        var allPageList = Trans.GetComponentsInChildren<UIPage>(true);
        foreach (var page in allPageList)
        {
            page.SetActive(false);
        }

        var pageList = RootTrans.GetComponentsInChildren<UIPage>(true);
        foreach (var page in pageList)
        {
            RegisterPage(page);
        }

        var allMaskPage = Trans.GetComponentsInChildren<UISingletonMaskPage>(true);
        foreach (var maskPage in allMaskPage)
        {
            maskPage.Init();
        }

        HideAllPage();
    }

    public void HideAll()
    {
        HideAllPage();
        HideAllItem();
    }

    #region Page

    [NonSerialized] public UIPage Current;
    [NonSerialized] public UIPage Last;

    [NonSerialized] public Dictionary<Type, UIPage> PageDic = new Dictionary<Type, UIPage>();
    [ReadOnly] public List<UIPage> PageStack = new List<UIPage>();

    public void RegisterPage(UIPage page)
    {
        page.InvokeMethod(nameof(Awake));
        PageDic.TryAdd(page.GetType(), page);
        page.SetActive(false);
    }

    #region Get Page

    public TPage GetPage<TPage>() where TPage : UIPage
    {
        var type = typeof(TPage);
        return GetPage(type) as TPage;
    }

    public UIPage GetPage(Type pageType)
    {
        if (PageDic.TryGetValue(pageType, out var window))
        {
            return window as UIPage;
        }

        window = (UIPage)RootTrans.GetComponentInChildren(pageType, true);
        if (window != null)
        {
            RegisterPage(window);
            return window;
        }

        window = (UIPage)Trans.GetComponentInChildren(pageType, true);
        if (window != null)
        {
            RegisterPage(window);
            return window;
        }

        window = PreLoad(pageType);
        return window;
    }

    #endregion

    #region PreLoad Page

    public TPage PreLoad<TPage>() where TPage : UIPage
    {
        var window = PreLoad(typeof(TPage)) as TPage;
        return window;
    }

    public UIPage PreLoad(Type pageType)
    {
        if (PageDic.TryGetValue(pageType, out var window)) return window;
        var path = "UI/" + pageType.Name;
        var prefab = Resources.Load<UIPage>(path);
        if (prefab == null) return default;
        var windowIns = UIPool.Spawn<UIPage>(prefab, RootTrans);
        windowIns.gameObject.SetActive(false);
        PageDic.Add(pageType, windowIns);
        return windowIns;
    }

    public void PreLoadAsync<TPage>() where TPage : UIPage
    {
        PreLoadAsync(typeof(TPage));
    }

    public void PreLoadAsync(Type pageType)
    {
        if (PageDic.TryGetValue(pageType, out var _)) return;
        var path = "UI/" + pageType.Name;
        var loadRequest = Resources.LoadAsync<UIPage>(path);
        this.ExecuteWhen(() =>
        {
            var prefab = loadRequest.asset as UIPage;
            if (prefab == null) return;
            var pageIns = UIPool.Spawn(prefab, RootTrans);
            pageIns.gameObject.SetActive(false);
            PageDic.Add(pageType, pageIns);
        }, () => loadRequest.isDone);
    }

    #endregion

    #region Show Page

    public void Show<TPage>(params object[] args) where TPage : UIPage
    {
        var type = typeof(TPage);
        Show(type, args);
    }

    public void ShowWithHideLast<TPage>(bool hideLast, params object[] args) where TPage : UIPage
    {
        var type = typeof(TPage);
        ShowWithHideLast(type, hideLast, args);
    }

    public void Show(Type windowType, params object[] args)
    {
        ShowWithHideLast(windowType, true, args);
    }

    public void ShowWithHideLast(Type pageType, bool hideLast, params object[] args)
    {
        var window = GetPage(pageType);
        if (window == null) return;
        ShowWithHideLast(window, hideLast, args);
    }

    public void Show(UIPage page, params object[] args)
    {
        ShowWithHideLast(page, true, args);
    }

    public void ShowWithHideLast(UIPage page, bool hideLast, params object[] args)
    {
        if (!PageStack.Contains(page))
        {
            if (PageStack.Count > 0 && hideLast)
            {
                PageStack.Last().Hide();
            }

            page.Show(args);
            PageStack.Add(page);
            Last = Current;
            Current = page;
        }
        else
        {
            var uiIndex = PageStack.IndexOf(page);
            for (var i = PageStack.Count - 1; i > uiIndex; i--)
            {
                var underUi = PageStack[i];
                Hide(underUi);
            }
        }
    }

    #endregion

    #region Hide Page

    public void Hide<TPage>() where TPage : UIPage
    {
        var type = typeof(TPage);
        Hide(type);
    }

    public void Hide(Type pageType)
    {
        var window = GetPage(pageType);
        if (window == null) return;
        Hide(window);
    }

    public void Hide(UIPage page)
    {
        if (PageStack.Contains(page))
        {
            page.Hide();
            PageStack.Remove(page);
        }
        else return;

        if (PageStack.Count <= 0) return;
        var lastUi = PageStack.Last();
        if (lastUi.PageSate == UIPageSate.Hide)
        {
            lastUi.Show();
        }
        else
        {
            lastUi.RestoreShow();
        }

        Last = Current;
        Current = lastUi;
    }

    public void HideAllPage()
    {
        for (var i = PageStack.Count - 1; i >= 0; i--)
        {
            var ui = PageStack[i];
            ui.Hide();
            PageStack.Remove(ui);
        }
    }

    #endregion

    #endregion

    #region UI Follow Scene Object

    [NonSerialized] public List<UIFollowSceneObject> SceneItemList = new List<UIFollowSceneObject>();

    public TSceneItem ShowItem<TSceneItem>(TSceneItem itemPrefab, EntityBase target, params object[] args)
        where TSceneItem : UIFollowSceneObject
    {
        var item = UIPool.Spawn(itemPrefab, Trans);
        SceneItemList.Add(item);
        item.Show(target, args);
        return item;
    }

    public void Hide<TSceneItem>(TSceneItem item)
        where TSceneItem : UIFollowSceneObject
    {
        SceneItemList.Remove(item);
        UIPool.DeSpawn(item);
    }

    public void HideAllItem(Predicate<UIFollowSceneObject> predicate = null)
    {
        for (var i = SceneItemList.Count - 1; i >= 0; i--)
        {
            var uiSceneItem = SceneItemList[i];
            if (predicate == null || predicate(uiSceneItem)) Hide(uiSceneItem);
        }
    }

    #endregion
}