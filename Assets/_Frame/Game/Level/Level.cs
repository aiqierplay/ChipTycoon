using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Sirenix.OdinInspector;

public class Level : EntityBase
{
    [BoxGroup("Level")] public int PlayerCount = 1;

    [NonSerialized] public List<ItemBase> ItemList = new List<ItemBase>();
    [NonSerialized, GetComponentInChildren] public List<EntityPropertyInitializer> LevelEntityPropertyList = new List<EntityPropertyInitializer>();
    [NonSerialized] public Dictionary<Type, List<ItemBase>> ItemDic = new Dictionary<Type, List<ItemBase>>();

    [NonSerialized] public int Index;
    [NonSerialized] public string SaveKey;

    [NonSerialized] public LevelData Data;
    [NonSerialized] public LevelSaveInfo Info;

    [GetComponentInChildren, NonSerialized]
    public DraggableCamTarget DraggableCamTarget;

    [GetComponentInChildren, NonSerialized]
    public new World World;

    public LevelSaveInfo NextInfo => LevelSaveInfo.GetInfo(Index + 1, false);


    public void Init(int levelIndex)
    {
        Index = levelIndex;
        SaveKey = $"{nameof(Level)}_{Index:D2}";

        Data = Config.GetData<LevelData>(levelIndex - 1);
        Info = LevelSaveInfo.GetInfo(levelIndex, false);

        // Runner
        // InitRunnerBlocks();

        // General
        // InitItem();
        // InitPlayer();

        World.Init();

        if (DraggableCamTarget != null) DraggableCamTarget.Init();
        Dispatch(GameEvent.LoadLevel);
    }

    public void Retry()
    {
        Info.ResetSave();
    }

    #region Item

    public void InitItem()
    {
        ItemList = transform.GetComponentsInChildren<ItemBase>(true).ToList();
        foreach (var item in ItemList)
        {
            item.Init();
        }

        ItemList = transform.GetComponentsInChildren<ItemBase>(true).ToList();

        ItemDic = new Dictionary<Type, List<ItemBase>>();
        foreach (var item in ItemList)
        {
            var itemType = item.GetType();
            if (!ItemDic.TryGetValue(itemType, out var itemList))
            {
                itemList = new List<ItemBase>();
                ItemDic.Add(itemType, itemList);
            }

            itemList.Add(item);
        }

        foreach (var propertyInitializer in LevelEntityPropertyList)
        {
            propertyInitializer.Init();
        }

        if (LevelEntityPropertyList.Count > 0)
        {
            foreach (var item in ItemList)
            {
                item.Refresh();
            }
        }
    }

    public void InitItemsRenderer()
    {
        ItemList.ForEach(item => item.InitRenderer());
    }

    public List<T> GetItems<T>() where T : ItemBase
    {
        if (!ItemDic.TryGetValue(typeof(T), out var list))
        {
            list = new List<ItemBase>();
            ItemDic.Add(typeof(T), list);
        }

        return list.ToList(i => i as T);
    }

    public T GetItem<T>() where T : ItemBase
    {
        var list = GetItems<T>();
        return list.First();
    }

    public void RemoveItem(ItemBase item)
    {
        var type = item.GetType();
        if (ItemList.Contains(item)) ItemList.Remove(item);
        if (ItemDic.TryGetValue(type, out var list))
        {
            if (list != null && list.Contains(item))
            {
                list.Remove(item);
            }
        }
    }

    #endregion

    #region Runner

    [FoldoutGroup("Runner")] public List<LevelBlock> RunnerBlockList;
    public List<LevelBlock> RunnerBlockInsList { get; set; } = new List<LevelBlock>();

    [ButtonGroup("Test"), Button("Test Create Runner Map")]
    public void InitRunnerBlocks()
    {
        DeSpawnRunnerBlocks();

        var currentPos = Vector3.zero;
        var currentForward = Vector3.forward;
        foreach (var levelBlockPrefab in RunnerBlockList)
        {
            var trans = Application.isPlaying ? GetChildTrans(nameof(LevelBlock)) : transform;
            var blockIns = Application.isPlaying ? GamePool.Spawn(levelBlockPrefab, transform) : Instantiate(levelBlockPrefab, trans);
            blockIns.transform.position = currentPos;
            blockIns.transform.forward = currentForward;
            blockIns.Init();

            currentPos = blockIns.EndPosition;
            currentForward = blockIns.EndForward;

            RunnerBlockInsList.Add(blockIns);
        }
    }

    [ButtonGroup("Test"), Button("Destroy Runner Map"), GUIColor(1f, 0.5f, 0.5f)]
    public void DeSpawnRunnerBlocks()
    {
        if (!Application.isPlaying)
        {
            RunnerBlockInsList = GetComponentsInChildren<LevelBlock>().ToList();
        }

        foreach (var levelBlock in RunnerBlockInsList)
        {
            if (Application.isPlaying)
            {
                GamePool.DeSpawn(levelBlock);
            }
            else
            {
                DestroyImmediate(levelBlock.gameObject);
            }
        }

        RunnerBlockInsList.Clear();
    }

    #endregion
}
