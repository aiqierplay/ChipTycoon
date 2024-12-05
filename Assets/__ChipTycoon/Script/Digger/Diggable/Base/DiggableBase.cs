using Aya.Physical;
using System;
using Aya.Extension;
using Aya.TweenPro;
using Aya.Util;
using UnityEngine;

public abstract class DiggableBase : EntityBase
{
    public int RequirePower;
    public GameObject FxBreak;

    [GetComponentInChildren, NonSerialized] public ColliderListenerEnter ColliderListenerEnter;
    [NonSerialized] public bool IsBroken;
    [NonSerialized] public int Hp;


    public virtual void Init()
    {
        Hp = RequirePower;
        IsBroken = false;
        gameObject.SetActive(true);
        if (ColliderListenerEnter != null)
        {
            ColliderListenerEnter.Clear();
            ColliderListenerEnter.onTriggerEnter.Add<DiggerTool>(OnEnter, LayerManager.Ins.Player);
        }
    }

    public virtual bool CheckCanBreak()
    {
        if (IsBroken) return false;
        if (World.DiggerArea.DiggerTool.Mode != DiggerToolMode.Digger) return false;
        if (RequirePower <= 0) return true;
        return true;
    }

    public virtual void OnEnterForce(DiggerTool digger, bool force)
    {
        if (!force && !CheckCanBreak()) return;
        if (!force)
        {
            if (Hp > digger.Power)
            {
                Hp -= digger.Power;
                return;
            }
            else
            {
                Hp = 0;
            }
        }
        else
        {
            Hp = 0;
        }

        if (Hp == 0)
        {
            SpawnFx(FxBreak, CurrentLevel.Trans, Position);
            IsBroken = true;
            UTween.Shake(Trans, RandUtil.RandVector3(-0.15f, 0.15f), RandUtil.RandVector3(-5f, 5f), RandUtil.RandVector3(-0.1f, 0.1f), 3, 0.25f);
            this.ExecuteDelay(() =>
            {
                OnEnterImpl(digger);
                gameObject.SetActive(false);
            }, 0.3f);
        }
    }

    public virtual void OnEnter(DiggerTool digger)
    {
        OnEnterForce(digger, false);
    }

    public abstract void OnEnterImpl(DiggerTool digger);
}
