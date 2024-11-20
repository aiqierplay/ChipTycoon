using Aya.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

public enum ItemShowUIMode
{
    Window = 0,
    SceneItem = 1,
}

public class ItemShowUI : ItemBase<Player>
{
    [BoxGroup("UI")] public ItemShowUIMode UIMode;
    [BoxGroup("UI"), ShowIf("UIMode", ItemShowUIMode.Window), TypeReference(typeof(UIPage))] public TypeReference WindowType;
    [BoxGroup("UI"), ShowIf("UIMode", ItemShowUIMode.SceneItem)] public UIFollowSceneObject SceneItemPrefab;
    [BoxGroup("UI"), ShowIf("UIMode", ItemShowUIMode.SceneItem)] public EntityBase SceneItemTarget;
    [BoxGroup("UI")] public bool AutoHideAfterExit = true;
    [BoxGroup("UI")] public string[] Args;

    public UIFollowSceneObject UISceneItemInstance { get; set; }

    private float _lastShowTime;
    private readonly float _showInterval = 1f;

    public override void OnTargetEffect(Player target)
    {
        if (!target.IsPlayer) return;

        var current = Time.realtimeSinceStartup;
        if (current - _lastShowTime < _showInterval) return;
        _lastShowTime = current;

        Show();
    }

    public virtual void Show()
    {
        switch (UIMode)
        {
            case ItemShowUIMode.Window:
                UI.Show(WindowType, Args as object[]);
                break;
            case ItemShowUIMode.SceneItem:
                UISceneItemInstance = UI.ShowItem(SceneItemPrefab, SceneItemTarget, Args as object[]);
                break;
        }
    }

    public override void OnTargetExit(Player target)
    {
        if (!target.IsPlayer) return;

        var current = Time.realtimeSinceStartup;
        _lastShowTime = current;

        if (!AutoHideAfterExit) return;
        Hide();
    }

    public virtual void Hide()
    {
        switch (UIMode)
        {
            case ItemShowUIMode.Window:
                UI.Hide(WindowType);
                break;
            case ItemShowUIMode.SceneItem:
                UI.Hide(UISceneItemInstance);
                break;
        }
    }
}
