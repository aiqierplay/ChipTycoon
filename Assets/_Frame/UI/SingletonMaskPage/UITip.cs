using Aya.Pool;
using UnityEngine;

public class UITip : UIBase<UITip>
{
    public UITipItem DefaultTipPrefab;

    public EntityPool Pool => PoolManager.Ins["Tip"];

    public UITipItem ShowTip()
    {
        var prefab = DefaultTipPrefab;
        return ShowTipWithUiPos(prefab, Vector3.zero);
    }

    public UITipItem ShowTip(Vector3 uiPosition)
    {
        var prefab = DefaultTipPrefab;
        return ShowTip(prefab, uiPosition);
    }

    public UITipItem ShowTip(UITipItem tipPrefab, Vector3 uiPosition)
    {
        return ShowTipWithUiPos(tipPrefab, uiPosition);
    }

    public UITipItem ShowTipWithUiPos(UITipItem tipPrefab, Vector3 uiPosition)
    {
        var tip = Pool.Spawn(tipPrefab, transform, uiPosition);
        tip.Rect.anchoredPosition = uiPosition;
        tip.Show();
        return tip;
    }

}