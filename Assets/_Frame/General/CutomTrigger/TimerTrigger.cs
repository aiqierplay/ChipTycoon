using System;

[Serializable]
public class TimerTrigger : CustomTrigger
{
    public float Interval;
    public float Duration;
    public float Cooldown;
    
    [NonSerialized] public bool IsCooldown;
    [NonSerialized] public float IntervalTimer;

    public override bool CanTrigger => !IsRunning && !IsCooldown;

    public TimerTrigger(Action action, float interval, float duration = -1f, float cooldown = 0f)
    {
        Init(action, interval, duration, cooldown);
    }

    public void Init(Action action, float interval, float duration = -1f, float cooldown = 0f)
    {
        Callback.Action = action;
        Interval = interval;
        Duration = duration;
        Cooldown = cooldown;
    }

    public override bool Start()
    {
        var start = base.Start();
        if (!start) return false;
        IsCooldown = false;
        IntervalTimer = 0f;
        return true;
    }

    public override void Stop()
    {
        base.Stop();
        IsCooldown = true;
    }

    public override void Update(float deltaTime)
    {
        Timer += deltaTime;
        if (IsRunning)
        {
            IntervalTimer += deltaTime;
            if (IntervalTimer >= Interval)
            {
                Trigger();
                IntervalTimer = 0f;
            }

            if (Timer >= Duration && Duration > 0f)
            {
                Stop();
            }
        }
        else if (IsCooldown)
        {
            if (Timer >= Cooldown)
            {
                IsCooldown = false;
            }
        }
    }

}
