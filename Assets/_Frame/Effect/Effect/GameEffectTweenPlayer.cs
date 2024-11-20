using System;
using Aya.TweenPro;

[Serializable]
public class GameEffectTweenPlayer : GameEffectBase
{
    public UTweenPlayer Tween;

    public override void Init(EntityBase entity, EntityBase other = null)
    {
        base.Init(entity, other);
        // Tween.Sample(0f);
    }

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        Tween.Play();
    }

    public override float GetDuration()
    {
        return Tween.Animation.Duration + Tween.Animation.Delay + Delay;
    }
}
