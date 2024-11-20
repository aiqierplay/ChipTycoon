using System;
using Aya.Extension;
using UnityEngine;

[Serializable]
public class GameEffectAnimator : GameEffectBase
{
    public Animator Animator;
    public string Clip;
    public string Default;

    [NonSerialized] public EntityBase ParentEntity;

    public override void Init(EntityBase entity, EntityBase other = null)
    {
        base.Init(entity, other);
        if (ParentEntity == null && Animator != null)
        {
            ParentEntity = Animator.GetComponentInParent<EntityBase>();
        }

        PlayClip(Default);
    }

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        PlayClip(Clip);
    }

    public virtual void PlayClip(string clip)
    {
        if (string.IsNullOrEmpty(clip)) return;
        if (ParentEntity == null) return;
        ParentEntity.Play(clip);
    }

    public override float GetDuration()
    {
        if (Animator == null) return Delay;
        var clip = Animator.runtimeAnimatorController.animationClips.First(c => c.name == Clip);
        if (clip == null) return Delay;
        return clip.length + Delay;
    }
}