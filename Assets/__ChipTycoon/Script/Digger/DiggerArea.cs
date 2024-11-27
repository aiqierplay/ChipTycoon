using System;
using System.Collections.Generic;
using Aya.Extension;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class DiggerArea : EntityBase
{
    public UTweenPlayerReference TweenDiggerStart;
    public UTweenPlayerReference TweenDiggerEnd;

    [GetComponentInChildren(true), NonSerialized] public DiggerTool DiggerTool;

    [GetComponentInChildren(true), NonSerialized]
    public List<DiggableBase> DiggableList;

    [GetComponentInChildren(true), NonSerialized]
    public List<DropTriggerBase> DropTriggerList;

    [NonSerialized] public List<DiggableOre> OreList = new List<DiggableOre>();

    public void Init()
    {
        foreach (var diggable in DiggableList)
        {
            diggable.Init();
        }

        foreach (var dropTriggerBase in DropTriggerList)
        {
            dropTriggerBase.Init();
        }

        DiggerTool.Init();
    }

    public void StartDigger()
    {
        // UIGame.Ins.DiggerStart();
        TweenDiggerStart.Play();
        OreList.Clear();
        foreach (var diggable in DiggableList)
        {
            if (diggable.IsBroken) continue;
            if (diggable is DiggableOre ore) OreList.Add(ore);
        }

        DiggerTool.SetActive(true);
        DiggerTool.Init();
    }

    public void EndDigger()
    {
        // UIGame.Ins.DiggerEnd();
        TweenDiggerEnd.Play();
        DiggerTool.SetActive(false);
        World.Mode = GameMode.Work;
        World.SwitchCam(World.Mode);
        UIDigger.Ins.Back();
    }

    // public void LateUpdate()
    // {
    //     if (World.Mode != GameMode.Digger) return;
    //     var check = CheckCanContinue();
    //     if (!check)
    //     {
    //         EndDigger();
    //     }
    // }

    public bool CheckCanContinue()
    {
        if (OreList.Count == 0) return false;
        foreach (var ore in OreList)
        {
            var check = ore.CheckCanBreak();
            if (check) return true;
        }

        return false;
    }
}