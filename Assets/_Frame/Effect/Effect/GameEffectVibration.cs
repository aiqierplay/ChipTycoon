using System;
using Fishtail.PlayTheBall.Vibration;
using MoreMountains.NiceVibrations;

[Serializable]
public class GameEffectVibration : GameEffectBase
{
    public HapticTypes Type;

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        VibrationController.Instance.Impact(Type);
    }

    public override float GetDuration() => Delay;
}
