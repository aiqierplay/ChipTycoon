using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Work= 0,
    Digger = 1,
}

public class World : EntityBase
{
    public Transform WorkerTrans;

    [GetComponentInChildren, NonSerialized]
    public new DiggerArea DiggerArea;

    [GetComponentInChildren, NonSerialized]
    public List<BuildingBase> BuildingList;
    [GetComponentInChildren, NonSerialized]
    public List<FactoryBase> FactoryList;

    [NonSerialized] public Worker Character;
    [NonSerialized] public List<Worker> WorkerList = new List<Worker>();

    [NonSerialized] public GameMode Mode;

    public void Init()
    {
        Mode = GameMode.Work;
        if (DiggerArea != null) DiggerArea.Init();
        foreach (var building in BuildingList)
        {
            building.Init();
        }

        for (var index = 0; index < FactoryList.Count; index++)
        {
            var factory = FactoryList[index];
            factory.Init(index);
        }

        SpawnPlayer();
        SpawnWorker();
        SwitchCam(Mode);
    }

    public void SpawnPlayer()
    {
        Character = GamePool.Spawn(GeneralSetting.Ins.PlayerPrefab, WorkerTrans);
        Character.Init(WorkerType.Player);
    }

    public void SpawnWorker()
    {
        WorkerList.Clear();
    }

    public void SwitchCam(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Work:
                Camera.Switch("Game", Character.Trans);
                break;
            case GameMode.Digger:
                Camera.Switch("Digger", DiggerArea.Digger.Trans);
                break;
        }
    }

    public void EnterDigger()
    {
        Mode = GameMode.Digger;
        SwitchCam(GameMode.Digger);
        DiggerArea.Init();
        DiggerArea.StartDigger();
    }
}
