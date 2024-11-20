using System;
using UnityEngine;

public class PlayerBuff : PlayerBase
{
    public BuffManager BuffManager { get; set; } = new BuffManager();

    public override void InitComponent()
    {
        BuffManager.Init(Self);
    }

    public void AddBuff<T>(float duration, params object[] args) where T : BuffBase
    {
        BuffManager.AddBuff<T>(duration, args);
    }

    public void AddBuff<T>(float duration, object[] args, GameObject[] assets, AnimationCurve[] curves = null) where T : BuffBase
    {
        BuffManager.AddBuff<T>(duration, args, assets, curves);
    }

    public void AddBuff(Type buffType, float duration, params object[] args)
    {
        BuffManager.AddBuff(buffType, duration, args);
    }

    public void AddBuff(Type buffType, float duration, object[] args, GameObject[] assets, AnimationCurve[] curves = null)
    {
        BuffManager.AddBuff(buffType, duration, args, assets, curves);
    }

    public bool HasBuff<T>() where T : BuffBase
    {
        return BuffManager.HasBuff<T>();
    }

    public bool HasBuff(Type buffType)
    {
        return BuffManager.HasBuff(buffType);
    }

    public T GetBuff<T>() where T : BuffBase
    {
        return BuffManager.GetBuff<T>();
    }

    public BuffBase GetBuff(Type buffType)
    {
        return BuffManager.GetBuff(buffType);
    }

    public void StopBuff<T>() where T : BuffBase
    {
        BuffManager.StopBuff<T>();
    }

    public void StopBuff(Type buffType)
    {
        BuffManager.StopBuff(buffType);
    }

    public void StopAll()
    {
        BuffManager.StopAll();
    }


    public void Update()
    {
        var deltaTime = DeltaTime;
        if (App.GamePhase != GamePhaseType.Gaming) return;
        BuffManager.Update(deltaTime);
    }
}