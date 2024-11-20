using System;
using UnityEngine.Events;

[Serializable]
public class TriggerCallback
{
    public Action Action;
    public UnityEvent Event;
    public GameEffect Effect;

    public virtual void Invoke()
    {
        Action?.Invoke();
        Event?.Invoke();
        if (Effect != null) Effect.Play();
    }
}