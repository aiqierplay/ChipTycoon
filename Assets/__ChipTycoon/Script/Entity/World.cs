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


}
