using Aya.Extension;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class UITipItem : UIBase
{
    public Image Icon;
    public Text Text;
    public float Duration;

    public UITip UiTip => UITip.Ins;

    protected override void Awake()
    {
        base.Awake();
    }

    public UITipItem Show()
    {
        UiTip.ExecuteDelay(() =>
        {
            UiTip.Pool.DeSpawn(this);
        }, Duration);

        return this;
    }

    public UITipItem SetIcon(Sprite icon)
    {
        if (Icon != null)
        {
            Icon.SetActive(icon != null);
            Icon.sprite = icon;
        }

        return this;
    }

    public UITipItem SetText(string text)
    {
        if (Text != null)
        {
            Text.text = text;
        }

        return this;
    }

    public UITipItem SetText(string text, Color color)
    {
        if (Text != null)
        {
            Text.text = text;
            Text.color = color;
        }

        return this;
    }
}