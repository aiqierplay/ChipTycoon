using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class GameEffectBase
{
    [TableColumnWidth(65, false)]
    [Tooltip("Only used in Item Mode")] 
    public GameEffectTriggerMode Mode = GameEffectTriggerMode.None;

    [TableColumnWidth(55, false)]
    [SuffixLabel("sec", Overlay = true)]
    public float Delay;

    [NonSerialized] public EntityBase Entity;
    [NonSerialized] public EntityBase Other;

    public virtual void Init(EntityBase entity, EntityBase other = null)
    {
        Entity = entity;
        Other = other;
    }

    public abstract void PlayImpl(EntityBase entity, EntityBase other = null);

    public virtual void Play(EntityBase entity, EntityBase other = null)
    {
        if (Delay < 1e-6f)
        {
            PlayImpl(entity, other);
        }
        else
        {
            entity.StartCoroutine(PlayCo(entity, other));
        }
    }

    protected IEnumerator PlayCo(EntityBase entity, EntityBase other = null)
    {
        var timer = 0f;
        while (timer <= Delay)
        {
            timer += entity.DeltaTime;
            yield return null;
        }

        PlayImpl(entity, other);
    }

    public abstract float GetDuration();
}