using System;
using Aya.Events;
using Fishtail.PlayTheBall.Vibration;
using MoreMountains.NiceVibrations;

public abstract partial class EntityBase : MonoListener
{
    #region Event

    public void Dispatch<T>(T eventType, params object[] args)
    {
        UEvent.Dispatch(eventType, args);
    }

    public void DispatchTo<T>(T eventType, object target, params object[] args)
    {
        UEvent.DispatchTo(eventType, target, args);
    }

    public void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args)
    {
        UEvent.DispatchTo(eventType, predicate, args);
    }

    public static void DispatchGroup<T>(T eventType, object group, params object[] args)
    {
        UEvent.DispatchGroup(eventType, group, args);
    }

    #endregion

    #region Vibrate

    public void Vibrate(HapticTypes vibrationType)
    {
        VibrationController.Instance.Impact(vibrationType);
    }

    #endregion
}
