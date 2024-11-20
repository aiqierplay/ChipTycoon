using Aya.Data.Persistent;
using UnityEngine;

public class UITutorialMask : UIBase
{
    public string Key;
    public GameObject Mask;

    protected sBool TutorialShown;

    protected override void Awake()
    {
        base.Awake();
        TutorialShown = new sBool(Key, false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Refresh();
    }

    public override void Refresh(bool immediately = false)
    {
        Mask.gameObject.SetActive(!TutorialShown);
        if (!TutorialShown)
        {
            TutorialShown.Value = true;
        }
    }
}
