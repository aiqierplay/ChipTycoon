using System;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public enum LoopMapMode
{
    Dynamic = 0,
    Static = 1,
}

public enum LoopMapDirection
{
    Forward = 0,
    Backward = 1,
}

public class LoopMap : EntityBase<LoopMap>
{
    public Transform MapTrans;
    public LoopMapMode Mode = LoopMapMode.Dynamic;
    public LoopMapDirection Direction = LoopMapDirection.Forward;
    public float CacheLength = 100f;

    public LoopMapBlock FirstBlock;
    public LoopMapBlock LastBlock;
    public List<LoopMapBlock> BlockPrefabList;

    [NonSerialized] public List<LoopMapBlock> BlockList = new List<LoopMapBlock>();
    [NonSerialized] public float CurrentCacheLength;
    [NonSerialized] public float CurrentCreateLength;
    [NonSerialized] public float CurrentMoveLength;

    public void Init()
    {
        BlockList.Clear();
        CurrentMoveLength = 0f;
        CurrentCreateLength = 0f;
        CurrentCacheLength = 0f;

        switch (Direction)
        {
            case LoopMapDirection.Forward:
                MapTrans.SetLocalEulerAnglesY(0f);
                break;
            case LoopMapDirection.Backward:
                MapTrans.SetLocalEulerAnglesY(180f);
                break;
        }

        if (CurrentCacheLength < CacheLength)
        {
            SpawnNextBlock();
        }
    }

    public void UpdateImpl(float moveSpeed, float deltaTime)
    {
        CurrentMoveLength += deltaTime * moveSpeed;
        switch (Mode)
        {
            case LoopMapMode.Dynamic:
                switch (Direction)
                {
                    case LoopMapDirection.Forward:
                        MapTrans.SetLocalPositionZ(CurrentMoveLength);
                        break;
                    case LoopMapDirection.Backward:
                        MapTrans.SetLocalPositionZ(-CurrentMoveLength);
                        break;
                }
                
                break;
            case LoopMapMode.Static:
                switch (Direction)
                {
                    case LoopMapDirection.Forward:
                        MapTrans.SetLocalPositionZ(-CurrentMoveLength);
                        break;
                    case LoopMapDirection.Backward:
                        MapTrans.SetLocalPositionZ(CurrentMoveLength);
                        break;
                }

                break;
        }
       
        while (CurrentCreateLength - CacheLength < CurrentMoveLength)
        {
            SpawnNextBlock();
        }
    }

    public void SpawnNextBlock(bool isEnd = false)
    {
        LoopMapBlock blockPrefab;
        if (BlockList.Count == 0 && FirstBlock != null)
        {
            blockPrefab = FirstBlock;
        }
        else if(isEnd && LastBlock != null)
        {
            blockPrefab = LastBlock;
        }
        else
        {
            blockPrefab = BlockPrefabList.Random();
        }

        var block = GamePool.Spawn(blockPrefab, MapTrans);
        block.Trans.SetLocalPositionZ(CurrentCreateLength);
        BlockList.Add(block);
        CurrentCreateLength += block.Length;
        CurrentCacheLength += block.Length;
        DeSpawnBlock();
    }

    public void DeSpawnBlock()
    {
        do
        {
            var firstBlock = BlockList.First();
            if (firstBlock == null) break;
            if (CurrentCacheLength - firstBlock.Length >= CacheLength)
            {
                CurrentCacheLength -= firstBlock.Length;
                BlockList.Remove(firstBlock);
                GamePool.DeSpawn(firstBlock);
            }
            else
            {
                break;
            }
        } while (true);
    }
}
