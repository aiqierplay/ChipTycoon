using System;
using Aya.Media.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class GameEffectAudio : GameEffectBase
{
    [TableColumnWidth(70, false)] public AudioGroupType Group = AudioGroupType.SE;
    public AudioClip Clip;

    public override void PlayImpl(EntityBase entity, EntityBase other = null)
    {
        if (Clip == null) return;
        Audio.Play(Group, Clip);
    }

    public override float GetDuration()
    {
        return Clip.length + Delay;
    }
}
