using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemPhysicObstacle : ItemBase<Player>
{
    public List<Rigidbody> ObstacleList;
    public bool CheckOffset = true;

    [NonSerialized] public List<TransformData> ObstacleDataList;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init()
    {
        base.Init();

        if (ObstacleDataList == null)
        {
            ObstacleDataList = new List<TransformData>();
            for (var i = 0; i < ObstacleList.Count; i++)
            {
                var obstacle = ObstacleList[i];
                var data = TransformData.Create(obstacle.transform);
                ObstacleDataList.Add(data);
            }
        }

        ResetPos();
    }

    public override void OnTargetEffect(Player target)
    {
        
    }

    public virtual void Update()
    {
        if (!Active) return;
        if (CheckOffset)
        {
            var offset = false;
            foreach (var data in ObstacleDataList)
            {
                if (data.IsDifferent(false))
                {
                    offset = true;
                    break;
                }
            }

            if (offset && Active)
            {
                Complete();
            }
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        ResetPos();
    }

    [Button("Reset Pos")]
    public void ResetPos()
    {
        for (var i = 0; i < ObstacleList.Count; i++)
        {
            var obstacle = ObstacleList[i];
            var data = ObstacleDataList[i];
            obstacle.ClearMomentum();
            obstacle.isKinematic = true;
            data.CopyTo(obstacle.transform, false);
            obstacle.isKinematic = false;
        }
    }
}
