using System;

[Serializable]
public class PlayerRankCondition : ValueCompareCondition<Player>
{
    public override float GetCompareValue(Player target)
    {
        var player = target as Player;
        return player.State.Rank;
    }
}
