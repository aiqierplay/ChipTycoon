using UnityEngine;

public class UINotifyNode : UIBase
{
    public string Key;
    public bool DefaultState;

    public GameObject ShowObj;

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshState();
    }

    // [Listen(GameEvent.UpdateSaveState)]
    public void RefreshState()
    {
        var state = SaveState.GetBool(Key, DefaultState);
        ShowObj.SetActive(state);
    }
}