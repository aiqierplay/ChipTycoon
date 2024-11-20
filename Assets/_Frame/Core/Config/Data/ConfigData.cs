using System;
using UnityEngine;

[Serializable]
public abstract class ConfigData
{
    public int Index => ID - 1;
    [DataFieldOrder(1)] public int ID;
    [DataFieldOrder(2)] public float Value;
    public string Description;
    public int IntValue => Mathf.RoundToInt(Value);

    [NonSerialized] public bool IsMaxLevel;

    public virtual bool CanUpgrade()
    {
        return true;
    }

    public virtual void Upgrade()
    {

    }

    public virtual void ForceUpgrade()
    {

    }
}