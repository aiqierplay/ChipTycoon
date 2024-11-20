using System;
using Aya.Data.Persistent;
using Aya.Events;
using Aya.Extension;
using Aya.Util;
using UnityEngine;

public class OfflineEarnings : EntityBase<OfflineEarnings>
{
    public float CoinPerMinutes = 10;
    public float MinMinutes = 60;
    public float MaxMinutes = 1440;

    [NonSerialized] public DateTime LaunchTime;
    [NonSerialized] public int TotalMinutes;

    protected override void Awake()
    {
        base.Awake();
        Log("Offline Launch", DateTime.UtcNow);
        LaunchTime = DateTime.UtcNow;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.ExecuteEndOfFrame(() =>
        {
            TotalMinutes = Mathf.RoundToInt((float)(LaunchTime - Save.LastExitTime).TotalMinutes);
            Log("Offline Time ", TotalMinutes);
        });
    }

    [Listen(GameEvent.Launch)]
    public void ShowOfflineEarnings()
    {
        this.ExecuteNextFrame(() =>
        {
            if (TotalMinutes > MinMinutes)
            {
                UI.Show<UIOfflineEarnings>();
            }
            else
            {
                Save.LastExitTime.DateTimeValue = DateTime.UtcNow;
            }
        });
    }

    public string GetTime()
    {
        if (TotalMinutes > MaxMinutes) TotalMinutes = (int)MaxMinutes;
        var minutes = TotalMinutes % 60;
        var hours = TotalMinutes / 60;
        var time = hours.ToString("D2") + minutes.ToString("D2");
        return time;
    }

    public float GetEarnValue()
    {
        if (TotalMinutes > MaxMinutes) TotalMinutes = (int)MaxMinutes;
        var result = TotalMinutes * CoinPerMinutes;
        Log("Value", TotalMinutes, result);
        return result;
    }

    public void GetEarn(float multiply = 1f)
    {
        if (UIOfflineEarnings.Ins.HasGetEarn) return;
        UIOfflineEarnings.Ins.HasGetEarn = true;

        var value = Mathf.RoundToInt(GetEarnValue() * multiply);
        Save.LastExitTime.DateTimeValue = DateTime.UtcNow;

        UIFlyIcon.Ins.Fly(UIFlyIcon.Coin, UIOfflineEarnings.Ins.Trans.position, RandUtil.RandInt(10, 20), () =>
        {

        }, () =>
        {
            Save.Coin.Value += value;
            UIOfflineEarnings.Ins.Back();
        });
    }

    public void OnApplicationFocus()
    {
        
    }

    public void OnApplicationQuit()
    {
        Log("Offline Exit", DateTime.UtcNow);
        Save.LastExitTime.DateTimeValue = DateTime.UtcNow;
        USave.Save();
    }
}
