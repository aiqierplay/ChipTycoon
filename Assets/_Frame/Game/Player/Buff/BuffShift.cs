
using Aya.Extension;

public class BuffShift : BuffBase
{
    public float SpeedChange => Args[0].AsFloat();

    public override void StartImpl()
    {
        TargetPlayer.State.SpeedMultiply += SpeedChange;
    }

    public override void UpdateImpl(float deltaTime)
    {

    }

    public override void EndImpl()
    {
        TargetPlayer.State.SpeedMultiply -= SpeedChange;
    }
}
