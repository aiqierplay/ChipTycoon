using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDigger : UIPage<UIDigger>
{
    public GameObject SelectDigger;
    public GameObject SelectAbsorber;
    public GameObject ControlObj;

    public override void Show(params object[] args)
    {
        base.Show(args);
        Refresh();
        UIDigger.Ins.ControlObj.SetActive(true);
    }


    public override void Refresh(bool immediately = false)
    {
        base.Refresh(immediately);
        if (SelectDigger != null) SelectDigger.SetActive(World.DiggerArea.DiggerTool.Mode == DiggerToolMode.Digger);
        if (SelectAbsorber != null) SelectAbsorber.SetActive(World.DiggerArea.DiggerTool.Mode == DiggerToolMode.Absorber);
    }

    public void SwitchDigger()
    {
        World.DiggerArea.DiggerTool.SwitchTool(DiggerToolMode.Digger);
        Refresh();
    }

    public void SwitchAbsorber()
    {
        World.DiggerArea.DiggerTool.SwitchTool(DiggerToolMode.Absorber);
        Refresh();
    }

    public void Exit()
    {
        World.DiggerArea.EndDigger();
    }

    public void OnTouchStart(Vector3 pos)
    {
        DiggerArea.DiggerTool.OnTouchStart(pos);
    }

    public void OnTouch(Vector3 pos)
    {
        DiggerArea.DiggerTool.OnTouch(pos);
    }

    public void OnTouchEnd(Vector3 pos)
    {
        DiggerArea.DiggerTool.OnTouchEnd(pos);
    }
}
