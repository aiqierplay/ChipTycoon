using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class BattleAnimator
{
    public float AttackDuration = 1f;
    public float DieDuration = 1f;
    public string IdleClip = "Idle";
    public int IdleCount = 0;
    public string RunClip = "Run";
    public string AttackClip = "Attack";
    // public string HitClip = "Hit";
    public string DieClip = "Die";

    [NonSerialized] public BattleEntity Battle;
    [NonSerialized] public EntityBase AnimatorHandler;

    public void Init(BattleEntity battle)
    {
        Battle = battle;
        AnimatorHandler = battle.AnimatorHandler;
        if (AnimatorHandler == null) AnimatorHandler = battle;
    }

    public bool IsIdle => AnimatorHandler.CurrentAnimationClip.StartsWith(IdleClip);

    public void Play(string animationClipName, bool immediately = false)
    {
        if (animationClipName == IdleClip && IdleCount > 0)
        {
            animationClipName = "Idle_" + RandUtil.RandInt(0, IdleCount).ToString("D2");
        }

        AnimatorHandler.Play(animationClipName, immediately);
    }
}