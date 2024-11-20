using System;

public abstract class CustomTrigger
{
    public TriggerCallback Callback = new TriggerCallback();

    [NonSerialized] public bool IsRunning;
    [NonSerialized] public float Timer;

    public abstract bool CanTrigger { get; }

    public virtual bool Start()
    {
        if (!CanTrigger) return false;
        IsRunning = true;
        Timer = 0f;
        return true;
    }

    public virtual void Stop()
    {
        IsRunning = false;
        Timer = 0f;
    }

    public virtual void Update(float deltaTime)
    {

    }

    public virtual void Trigger()
    {
        Callback.Invoke();
    }
}
