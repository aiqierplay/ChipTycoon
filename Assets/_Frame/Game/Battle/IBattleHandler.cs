using System.Collections.Generic;

public interface IBattleHandler
{
    // public IEnumerable<BattleEntity> GetTargetList();
    // public BattleEntity FindTarget();
    // public bool Move(float deltaTime);
    public bool OnBeforeAttack(BattleEntity target);
    public void OnAfterAttack(BattleEntity target);
    public void OnHit(BattleEntity other, int power);
    public void OnDie();
}