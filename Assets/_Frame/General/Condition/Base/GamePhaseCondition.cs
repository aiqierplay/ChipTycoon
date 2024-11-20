using System;

[Serializable]
public class GamePhaseCondition : ConditionBase<EntityBase>
{
    public GamePhaseType GamePhase;

    public override bool CheckCondition(EntityBase target)
    {
        return (AppManager.Ins.GamePhase & GamePhase) > 0;
    }
}
