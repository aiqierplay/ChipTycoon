using System;
using Sirenix.OdinInspector;

public enum GameEffectItemOperateMode
{
    Enable = 0,
    Disable = 1,
}

[Serializable]
public class GameEffectItem : GameEffectBase
{
    public ItemBase Target;
    [TableColumnWidth(75, false)] public GameEffectItemOperateMode Operate;

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        switch (Operate)
        {
            case GameEffectItemOperateMode.Enable:
                Target.Active = true;
                break;
            case GameEffectItemOperateMode.Disable:
                Target.Active = false;
                break;
        }
    }

    public override float GetDuration()
    {
        return Delay;
    }
}
