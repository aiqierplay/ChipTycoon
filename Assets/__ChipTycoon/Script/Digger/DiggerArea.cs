using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.Utilities;

public class DiggerArea : EntityBase
{
    public UTweenPlayerReference TweenDiggerStart;
    public UTweenPlayerReference TweenDiggerEnd;

    [GetComponentInChildren, NonSerialized] public Digger Digger;

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

        Digger.SetActive(false);

        foreach (var dropTriggerBase in DropTriggerList)
        {
            dropTriggerBase.Init();
        }
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

        Digger.SetActive(true);
        Digger.Init();
    }

    public void EndDigger()
    {
        // UIGame.Ins.DiggerEnd();
        TweenDiggerEnd.Play();
        Digger.SetActive(false);
        World.Mode = GameMode.Work;
    }

    public void LateUpdate()
    {
        if (World.Mode != GameMode.Digger) return;
        var check = CheckCanContinue();
        if (!check)
        {
            EndDigger();
        }
    }

    public bool CheckCanContinue()
    {
        if (OreList.Count == 0) return false;
        foreach (var ore in OreList)
        {
            if (ore.CheckCanBreak()) return true;
        }

        return false;
    }
}
