using System;
using System.Collections.Generic;
using Aya.Extension;

[Serializable]
public class GameEffectSubEffect : GameEffectBase
{
    public GameEffect Effect;

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        Effect.Play();
    }

    public override float GetDuration()
    {
        return Effect.Duration + Delay;
    }
}
