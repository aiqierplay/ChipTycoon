
public class BuffInvincible : BuffBase
{
    public override void StartImpl()
    {
        TargetPlayer.State.IsInvincible = true;
    }

    public override void UpdateImpl(float deltaTime)
    {

    }

    public override void EndImpl()
    {
        TargetPlayer.State.IsInvincible = false;
    }
}
