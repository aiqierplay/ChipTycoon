using System;
using Aya.TweenPro;

[Serializable]
public class UTweenPlayerReference : CustomTypeReference<UTweenPlayer>
{
    public UTweenPlayerReference(UTweenPlayer value) : base(value)
    {
    }

    public static implicit operator UTweenPlayerReference(UTweenPlayer value) => new UTweenPlayerReference(value);

    public void Play()
    {
        if (Value == null) return;
        Value.Play();
    }

    public void Pause()
    {
        if (Value == null) return;
        Value.Pause();
    }

    public void Stop()
    {
        if (Value == null) return;
        Value.Stop();
    }

    public void Sample(float normalizedDuration)
    {
        if (Value == null) return;
        Value.Sample(normalizedDuration);
    }
}