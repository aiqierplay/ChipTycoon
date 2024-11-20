using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelPathFollower : EntityBase
{
    public bool ShowGizmos;
    [ShowIf(nameof(ShowGizmos))] public Color GizmosColor;

    [NonSerialized] public EntityBase Owner;
    [NonSerialized] public float Length;
    [NonSerialized] public int BlockIndex;

    [NonSerialized] public bool Finish;

    [NonSerialized] public float Distance;
    [NonSerialized] public float Factor;

    [NonSerialized] public List<int> BlockPathIndexes;

    [NonSerialized] public float BlockDistance;
    [NonSerialized] public float BlockFactor;

    public List<LevelBlock> CurrentBLockList;
    public LevelBlock CurrentBlock => CurrentBLockList[BlockIndex];
    public LevelPath CurrentPath => CurrentBlock.PathList[BlockPathIndex];
    public Vector2 TurnRange => CurrentPath.TurnRange;

    public int BlockPathIndex => BlockPathIndexes[BlockIndex];

    // Only used to first path
    [NonSerialized] public PathRoute Path;

    public void Init(EntityBase owner)
    {
        Owner = owner;
        Finish = false;
        Distance = 0f;
        Factor = 0f;
        Length = 0f;

        SwitchBlockList(CurrentLevel.RunnerBlockInsList);
    }

    public void SwitchBlockList(List<LevelBlock> blockList)
    {
        CurrentBLockList = blockList;

        BlockPathIndexes = new List<int>();
        foreach (var levelBlock in CurrentBLockList)
        {
            Length += levelBlock.Length;
            BlockPathIndexes.Add(0);
        }

        RefreshPathInfo();
        CachePath();
        EnterBlock(0);
    }

    public void SwitchBlockList(List<LevelBlock> blockList, float distance)
    {
        SwitchBlockList(blockList);
        Move(distance);
    }

    public void CachePath()
    {
        Path = new PathRoute
        {
            DefaultWidth = 20f
        };

        var step = 0.025f;
        for (var i = 0; i < CurrentBLockList.Count; i++)
        {
            var levelBlock = CurrentBLockList[i];
            if (i == 0)
            {
                var pos = levelBlock.Path.GetPositionByFactor(0);
                Path.Add(pos);
            }

            for (var j = step; j <= 1f; j += step)
            {
                var pos = levelBlock.Path.GetPositionByFactor(j);
                Path.Add(pos);
            }
        }
    }

    public float GetFactorByPosition()
    {
        return GetFactorByPosition(Position);
    }

    public float GetFactorByPosition(Vector3 position)
    {
        return Path.GetFactorByPos(position);
    }

    public void SwitchPath(int blockIndex, int pathIndex)
    {
        if (BlockPathIndexes[blockIndex] == pathIndex) return;
        BlockPathIndexes[blockIndex] = pathIndex;
        RefreshPathInfo();
    }


    public void RefreshPathInfo()
    {
        Length = 0f;
        for (var i = 0; i < CurrentBLockList.Count; i++)
        {
            var block = CurrentBLockList[i];
            var pathIndex = BlockPathIndexes[i];
            var path = block.PathList[pathIndex];
            Length += path.Length;
        }
    }

    public void RefreshFactor()
    {
        Factor = Distance / Length;
        BlockFactor = BlockDistance / CurrentPath.Length;
    }

    public Vector3 Move(float distance)
    {
        if (Finish) return CurrentPath.GetPositionByFactor(1f);
        Vector3 result;

        while (true)
        {
            var finish = false;
            var overDistance = 0f;
            (finish, result, overDistance) = CurrentPath.GetPositionByDistance(BlockDistance + distance);
            if (finish)
            {
                Distance += distance - overDistance;
                BlockDistance += distance - overDistance;
            }
            else
            {
                Distance += distance;
                BlockDistance += distance;
            }

            if (finish)
            {
                var enterResult = EnterBlock(BlockIndex + 1, overDistance);
                if (!enterResult)
                {
                    Finish = true;
                    break;
                }

                distance = 0f;
            }
            else
            {
                break;
            }
        }

        RefreshFactor();
        return result;
    }

    public bool EnterBlock(int index, float initDistance = 0f)
    {
        if (index >= CurrentBLockList.Count)
        {
            Finish = true;
            return false;
        }

        BlockIndex = index;
        EnterPath(initDistance);

        return true;
    }

    public virtual void EnterPath(float initMoveDistance = 0f)
    {
        BlockDistance = 0f;
        BlockFactor = 0f;
        Move(initMoveDistance);
    }

    public virtual void SetDistance(float distance)
    {
        Distance = 0f;
        Factor = 0f;
        BlockDistance = 0f;
        BlockFactor = 0f;
        BlockIndex = 0;
        Move(distance);
    }

#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        if (!ShowGizmos) return;
        if (Path == null) return;
        Gizmos.color = GizmosColor;
        for (var i = 0; i < Path.NodeList.Count - 1; i++)
        {
            var p1 = Path.NodeList[i];
            var p2 = Path.NodeList[i + 1];
            Gizmos.DrawLine(p1.Position, p2.Position);
            Gizmos.DrawLine(p1.Left, p1.Right);
        }
    }

#endif
}
