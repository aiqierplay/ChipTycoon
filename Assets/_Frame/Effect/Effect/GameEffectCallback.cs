using System;
using UnityEngine.Events;

[Serializable]
public class GameEffectCallback : GameEffectBase
{
    public UnityEvent Event;

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        Event.Invoke();
    }

    public override float GetDuration()
    {
        return Delay;
    }
}
