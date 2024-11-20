using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    public EntityBase Target;
    public Player TargetPlayer => Target as Player;

    public List<BuffBase> BuffList = new List<BuffBase>();
    public Dictionary<Type, BuffBase> BuffDic = new Dictionary<Type, BuffBase>();

    public void Init(EntityBase target)
    {
        Target = target;
        StopAll();
    }

    public void AddBuff<T>(float duration, params object[] args) where T : BuffBase
    {
        AddBuff(typeof(T), duration, args);
    }

    public void AddBuff<T>(float duration, object[] args, GameObject[] assets, AnimationCurve[] curves = null) where T : BuffBase
    {
        AddBuff(typeof(T), duration, args, assets, curves);
    }

    public void AddBuff(Type buffType, float duration, params object[] args)
    {
        AddBuff(buffType, duration, args, null);
    }

    public void AddBuff(Type buffType, float duration, object[] args, GameObject[] assets, AnimationCurve[] curves = null)
    {
        if (buffType == null) return;
        if (!BuffDic.TryGetValue(buffType, out var buff))
        {
            buff = Activator.CreateInstance(buffType) as BuffBase;
            if (buff == null) return;
            buff.Target = Target;
            BuffDic.Add(buffType, buff);
            BuffList.Add(buff);
        }

        if (buff.Active)
        {
            buff.Duration += duration;
        }
        else
        {
            buff.Start(duration, args, assets, curves);
        }
    }


    public bool HasBuff<T>() where T : BuffBase
    {
        var buffType = typeof(T);
        return HasBuff(buffType);
    }

    public bool HasBuff(Type buffType)
    {
        if (buffType == null) return false;
        if (BuffDic.TryGetValue(buffType, out var buff))
        {
            if (buff.Active) return true;
        }

        return false;
    }

    public T GetBuff<T>() where T : BuffBase
    {
        var buffType = typeof(T);
        var buff = GetBuff(buffType);
        return buff as T;
    }

    public BuffBase GetBuff(Type buffType)
    {
        if (buffType == null) return default;
        if (BuffDic.TryGetValue(buffType, out var buff))
        {
            if (buff.Active) return buff;
        }

        return default;
    }

    public void StopBuff<T>() where T : BuffBase
    {
        StopBuff(typeof(T));
    }

    public void StopBuff(Type buffType)
    {
        if (buffType == null) return;
        if (BuffDic.TryGetValue(buffType, out var buff))
        {
            if (!buff.Active) return;
            buff.End();
        }
    }

    public void StopAll()
    {
        foreach (var buff in BuffList)
        {
            buff.End();
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var buff in BuffList)
        {
            if (!buff.Active) continue;
            buff.Update(deltaTime);
        }
    }
}