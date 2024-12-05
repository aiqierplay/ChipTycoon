using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Util;
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
    [NonSerialized] public List<DropProduct> DropProductList = new List<DropProduct>();

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

        DiggerTool.SwitchTool(DiggerToolMode.Digger);
        DiggerTool.RefreshLine();
    }

    public void LoadState()
    {
        if (CurrentLevel.Info.DiggableState.Count == 0)
        {
            CurrentLevel.Info.DiggableState = new List<int>();
            for (var i = 0; i < DiggableList.Count; i++)
            {
                CurrentLevel.Info.DiggableState.Add(1);
            }
        }

        for (var i = 0; i < DiggableList.Count; i++)
        {
            var diggable = DiggableList[i];
            var active = CurrentLevel.Info.DiggableState[i];
            diggable.IsBroken = active == 0 ? true : false;
            if (active == 0) diggable.SetActive(false);
        }

        for (var i = 0; i < CurrentLevel.Info.DropProductCount; i++)
        {
            DiggableOre.CreateDropProduct(Position + Vector3.down * RandUtil.RandFloat(5, 8) + new Vector3(RandUtil.RandFloat(-5f, 5f), 0f, 0f));
        }
    }

    public void SaveState()
    {
        for (var i = 0; i < DiggableList.Count; i++)
        {
            var diggable = DiggableList[i];
            CurrentLevel.Info.DiggableState[i] = diggable.IsBroken ? 0 : 1;
        }

        CurrentLevel.Info.DropProductCount = DropProductList.Count;
        CurrentLevel.Info.Save();
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
        SaveState();
        TweenDiggerEnd.Play();
        DiggerTool.SetActive(false);
        World.Mode = GameMode.Work;
        World.SwitchCam(World.Mode);
        World.Character.EnableMove();
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