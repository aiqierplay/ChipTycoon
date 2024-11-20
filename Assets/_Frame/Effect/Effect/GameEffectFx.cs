using System;
using System.Collections.Generic;
using Aya.Extension;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using Sirenix.OdinInspector;
using UnityEngine;
public enum GameEffectTriggerMode
{
    None = -1,
    Enter = 0,
    Effect = 1,
    Exit = 2,

    OnlyUseInItem = 9999,
}

public enum GameEffectTargetMode
{
    Self = 0,
    Other = 1,
    Scene = 2,
}

[Serializable]
public class GameEffectFx : GameEffectBase
{
    [TableColumnWidth(60, false)] public GameEffectTargetMode Target;

    [TableColumnWidth(40, false)] public bool Rand;

    [TableColumnWidth(50)] public List<GameObject> FxList;

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        EntityBase target = null;
        Transform targetTrans = null;
        var position = Vector3.zero;
        switch (Target)
        {
            case GameEffectTargetMode.Self:
                target = entity;
                position = entity.Position;
                break;
            case GameEffectTargetMode.Other:
                if (other != null)
                {
                    target = other;
                    position = other.Position;
                }

                break;
            case GameEffectTargetMode.Scene:
                if (LevelManager.Ins != null)
                {
                    target = LevelManager.Ins.Level;
                }
                else if (AppManager.Ins != null)
                {
                    target = AppManager.Ins;
                }

                position = entity.Position;
                targetTrans = target != null ? target.Trans : null;
                goto SetComplete;
        }

        if (target == null) return;
        targetTrans = target.Trans;
        if (target.RendererTrans != null)
        {
            targetTrans = target.RendererTrans;
            position = target.RendererTrans.position;
        }

        SetComplete:
        if (target == null) return;
        if (Rand)
        {
            target.SpawnFx(FxList.Random(), targetTrans, position);
        }
        else
        {
            FxList.ForEach(f => target.SpawnFx(f, targetTrans, position));
        }
    }

    public override float GetDuration()
    {
        var duration = FxList.ToList(fx => fx.GetParticleDuration()).Max();
        return duration + Delay;
    }
}