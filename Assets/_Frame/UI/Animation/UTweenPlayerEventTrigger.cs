using Aya.TweenPro;
using Sirenix.OdinInspector;
using System;
using Aya.Events;
using UnityEngine;

[RequireComponent(typeof(UTweenPlayer))]
[HideMonoScript]
[Serializable]
public class UTweenPlayerEventTrigger : EntityBase
{
    public UTweenPlayer TweenPlayer;
    public GameEvent Event;

    protected override void OnEnable()
    {
        base.OnEnable();
        UEvent.Listen(Event, Play);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UEvent.Remove(Event, Play);
    }

    public void Play()
    {
        TweenPlayer.Play();
    }

    public void Reset()
    {
        TweenPlayer = GetComponent<UTweenPlayer>();
    }
}
