using System;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class BuffItemData
{
    public Type Type { get; set; }
    public ItemBase Prefab;
    public Sprite Icon;
    public string Description;
}

public class ItemRvBuff : ItemBuff
{
    [BoxGroup("Item")] public float AutoDeActiveDuration = 5f;
    [BoxGroup("Item")] public float AutoHideDuration = 1f;

    private float _lastShowTime;
    private readonly float _showInterval = 1f;

    public override void OnTargetEffect(Player target)
    {
        // base.OnTargetEffect(target);

        if (!target.IsPlayer) return;

        var current = Time.realtimeSinceStartup;
        if (current - _lastShowTime < _showInterval) return;
        _lastShowTime = current;

        if (Save.LevelIndex <= 1)
        {
            target.Buff.AddBuff(Type, Duration, Args);
            Complete();
            DeSpawn();
            return;
        }

        UI.Show<UIRvBuff>(this);
    }

    public override void OnTargetExit(Player target)
    {
        base.OnTargetExit(target);
        if (!target.IsPlayer) return;

        var current = Time.realtimeSinceStartup;
        _lastShowTime = current;

        App.ExecuteDelay(() =>
        {
            UI.Hide<UIRvBuff>();
        }, AutoHideDuration);

        this.ExecuteDelay(() =>
        {
            Active = false;
        }, AutoDeActiveDuration);
    }
}
