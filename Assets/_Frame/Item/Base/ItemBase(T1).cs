using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Physical;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ItemBase<TTarget> : ItemBase
    where TTarget : EntityBase
{
    [PropertyTooltip("条件列表" +
                     "\n\n在被触发时，依次检查列表中的条件，当且仅当条件全部满足时，道具效果才会被触发，否则不会触发功能且不计算触发次数。")]
    [TabGroup(ItemTabGroupMain, ItemTabCondition, SdfIconType.Funnel)]
    [SerializeReference]
    [TypeFilter(nameof(GetConditionTypeList))]
    public List<ConditionBase> Conditions = new List<ConditionBase>();

    public IEnumerable<Type> GetConditionTypeList()
    {
        var types = typeof(ConditionBase<TTarget>).GetSubTypes();
        return types;
    }

    public override Type TargetType => typeof(TTarget);

    [NonSerialized] public TTarget Target;

    public override void Init()
    {
        base.Init();
        Target = null;

        foreach (var guide in GuideList)
        {
            guide.Refresh();
        }
    }

    public override void CacheComponents()
    {
        base.CacheComponents();

        foreach (var col in ColliderList)
        {
            var listener = col.gameObject.GetComponent<ColliderListener>();
            if (listener == null) listener = col.gameObject.AddComponent<ColliderListener>();

            if (ClearTrigger) listener.Clear();
            if (ClearTrigger) listener.onTriggerEnter.Clear();
            listener.onTriggerEnter.Add<TTarget>(OnEnter, LayerMask);
            if (ClearTrigger) listener.onTriggerStay.Clear();
            listener.onTriggerStay.Add<TTarget>(OnStay, LayerMask);
            if (ClearTrigger) listener.onTriggerExit.Clear();
            listener.onTriggerExit.Add<TTarget>(OnExit, LayerMask);

            ColliderListeners.Add(listener);
        }
    }

    public virtual void OnEnter(TTarget target)
    {
        TryCatch(() =>
        {
            var canEnter = BeforeEnter(target);
            if (canEnter)
            {
                OnTargetEnter(target);
                ExecuteTriggerList(GameEffectTriggerMode.Enter, target);

                OnEffect(target);

                 AfterEnter();
            }
        }, "On Item Enter");
    }

    public virtual void OnStay(TTarget target)
    {
        if (EffectMode != ItemEffectMode.Stay) return;
        TryCatch(() =>
        {
            OnTargetStay(target);
            EffectIntervalTimer += DeltaTime;
            while (EffectIntervalTimer >= EffectInterval)
            {
                EffectIntervalTimer -= EffectInterval;
                OnEffect(target);
            }
        }, "On Item Stay");
    }

    public virtual void OnExit(TTarget target)
    {
        TryCatch(() =>
        {
            BeforeExit(target);
            OnTargetExit(target);
            ExecuteTriggerList(GameEffectTriggerMode.Exit, target);
        }, "On Item Exit");
        Target = null;

        if (DeSpawnMode == ItemDeSpawnMode.Exit)
        {
            Complete();
            DeSpawn();
        }
    }

    public virtual void OnEffect(TTarget target)
    {
        BeforeEffect(target);
        OnTargetEffect(target);
        ExecuteTriggerList(GameEffectTriggerMode.Effect, target);
    }

    public virtual bool BeforeEnter(TTarget target)
    {
        if (!Active) return false;

        if (Target == null)
        {
            Target = target;
        }

        if (Target == null) return false;

        // Condition
        foreach (var condition in Conditions)
        {
            if (condition is not ConditionBase<TTarget> conditionTemp) continue;
            var check = conditionTemp.CheckCondition(target);
            if (!check) return false;
        }

        // Effect Mode
        if (EffectMode == ItemEffectMode.Once && EffectCounter >= 1)
        {
            return false;
        }
        else if (EffectMode == ItemEffectMode.Count && EffectCount > 0)
        {
            if (EffectCounter >= EffectCount)
            {
                return false;
            }
        }

        // Camera
        if (FaceToTarget)
        {
            UTween.Forward(Trans, Target.Position - Position, FaceToDuration);
        }

        if (SwitchCamera)
        {
            if (!(target is Player playerTemp) || !playerTemp.IsAI)
            {
                Camera.Switch(EnterCamera, Target.RendererTrans);
            }
        }

        // Dock
        if (EnableDock && DockTrans != null)
        {
            var targetTrans = target.transform;
            UTween.Position(targetTrans, DockTrans.position, DockDuration);
            UTween.EulerAngles(targetTrans, DockTrans.eulerAngles, DockDuration);
            UTween.Scale(targetTrans, DockTrans.localScale, DockDuration);
        }

        // DeActive Render
        if (DeActiveRender && RendererTrans != null)
        {
            RendererTrans.gameObject.SetActive(false);
        }

        // Guide
        foreach (var guide in GuideList)
        {
            if (guide.IsComplete) continue;
            if (!guide.CheckShow()) continue;
            guide.Complete();
        }

        return true;
    }

    public virtual void AfterEnter()
    {
        if (DeSpawnMode == ItemDeSpawnMode.None)
        {
            if (EffectMode == ItemEffectMode.Once && EffectCounter >= 1)
            {
                Complete();
            }
            else if (EffectMode == ItemEffectMode.Count && EffectCount > 0)
            {
                if (EffectCounter >= EffectCount)
                {
                    Complete();
                }
            }
        }
        else if (DeSpawnMode == ItemDeSpawnMode.Effect)
        {
            Complete();
            DeSpawn();
            return;
        }
        else if (!Active && DeSpawnMode != ItemDeSpawnMode.None)
        {
            Complete();
            DeSpawn();
            return;
        }
    }

    public virtual void BeforeEffect(TTarget target)
    {
        EffectCounter++;
    }

    public abstract void OnTargetEffect(TTarget target);

    public virtual void OnTargetEnter(TTarget target)
    {

    }

    public virtual void OnTargetStay(TTarget target)
    {

    }

    public virtual void BeforeExit<T>(T target) where T : Component
    {
        // Camera
        if (SwitchCamera)
        {
            if (!(target is Player playerTemp) || !playerTemp.IsAI)
            {
                Camera.Switch(ExitCamera, Target.RendererTrans);
            }
        }
    }

    public virtual void OnTargetExit(TTarget target)
    {

    }
}
