using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GameEffectActive : GameEffectBase
{
    public UnityEngine.Object Target;
    [TableColumnWidth(45, false)] public bool Active = true;

    public override void Init(EntityBase entity, EntityBase other = null)
    {
        base.Init(entity, other);
        SetTargetState(!Active);
    }

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        SetTargetState(Active);
    }

    public virtual void SetTargetState(bool active)
    {
        if (Target is GameObject gameObject)
        {
            gameObject.SetActive(active);
        }
        else if (Target is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.enabled = active;
        }
    }

    public override float GetDuration() => Delay;
}