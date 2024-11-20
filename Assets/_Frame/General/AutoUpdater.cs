using Aya.Timers;
using Sirenix.OdinInspector;
using System;
using Aya.Extension;
using UnityEngine.Events;

public class AutoUpdater : EntityBase
{
    public bool UpdateOnEnable = false;
    public bool AutoUpdate = false;
    [ShowIf(nameof(AutoUpdate))] public float UpdateInterval = 1f;
    [ShowIf(nameof(AutoUpdate))] public UnityEvent UpdateEvent;

    [NonSerialized] public Action UpdateAction;
    [NonSerialized] public TimerEvent UpdateTimer;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (UpdateOnEnable)
        {
            this.ExecuteEndOfFrame(UpdateMethod);
        }

        if (AutoUpdate)
        {
            var timerId = Timer.Ins.Interval(UpdateMethod, UpdateInterval, false);
            UpdateTimer = Timer.Ins.GetTimerEvent(timerId);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (AutoUpdate && UpdateTimer != null)
        {
            Timer.Ins.Stop(UpdateTimer.Key);
            UpdateTimer = null;
        }
    }


    public virtual void UpdateMethod()
    {
        UpdateAction?.Invoke();
        UpdateEvent?.Invoke();
    }
}
