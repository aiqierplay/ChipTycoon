using Aya.Extension;
using UnityEngine;

public class UIKeyState : UIBase
{
    public GameObject[] Keys;

    private int _value;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ExecuteWhen(RefreshKey, () => Save != null);
    }

    public void Update()
    {
        var value = Save.Key.Value;
        if (_value == value) return;
        RefreshKey();
    }

    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        RefreshKey();
    }

    public void RefreshKey()
    {
        var value = Save.Key.Value;
        _value = value;
        for (var i = 0; i < Keys.Length; i++)
        {
            var key = Keys[i];
            key.gameObject.SetActive(i < value);
        }
    }
}