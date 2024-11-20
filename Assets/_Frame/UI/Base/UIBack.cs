using System;
using Aya.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIBack : UIBase
{
    [GetComponent, NonSerialized] public Button Button;
    [GetComponent, NonSerialized] public Image Image;

    protected override void Awake()
    {
        base.Awake();
        if (Button != null)
        {
            Button.onClick.AddListener(Back);
        }
        else if (Image != null)
        {
            UIEventListener.Get(Image.gameObject).onClick += (go, data) =>
            {
                Back();
            };
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Escape))
        {
            Back();
        }
    }

    public void Back()
    {
        UI.Current.Back();
    }
}
