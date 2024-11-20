using Aya.UI;
using UnityEngine;

public class UITouchCanvas : UIBase
{
    public RectTransform Circle;
    public RectTransform Handler;
    public float Radius;

    public bool Active { get; set; }

    protected override void Awake()
    {
        base.Awake();
        UIEventListener.Get(gameObject).onDown += (go, data) =>
        {
            if (!Player.State.EnableInput) return;
            ShowTip();
        };
        UIEventListener.Get(gameObject).onUp += (go, data) =>
        {
            HideTip();
        };
    }

    public void ShowTip()
    {
        Circle.gameObject.SetActive(true);
        Active = true;
        SetPosition(Input.mousePosition);
    }

    public void HideTip()
    {
        Circle.gameObject.SetActive(false);
        Active = false;
    }

    public void SetPosition(Vector2 position)
    {
        Circle.anchoredPosition = position / Screen.width * 1080f;
    }

    public void SetDirection(Vector3 direction)
    {
        Handler.anchoredPosition = direction.normalized * Radius;
    }
}
