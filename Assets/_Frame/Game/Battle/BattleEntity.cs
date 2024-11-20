using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Async;
using Aya.Extension;
using UnityEngine;

public class BattleEntity : EntityBase
{
    public BattleProperty Property;
    public new BattleAnimator Animator;
    public BattleEquipment Equipment;

    [NonSerialized] public BattleProperty BaseProperty = new BattleProperty();
    [NonSerialized] public bool Active;
    [NonSerialized] public int HpRuntime;
    [NonSerialized] public EntityBase AnimatorHandler;
    [NonSerialized] public BattleEntity Target;

    public float HpProgress => HpRuntime * 1f / Property.Hp;

    [NonSerialized] public List<BattleEntity> TargetList;

    // Replace Impl
    public Func<BattleEntity> FindTargetFunc;
    public Func<float, bool> MoveFunc;

    [NonSerialized] public IBattleHandler BattleHandler;

    public bool IsAlive => HpRuntime > 0;
    public bool IsDie => HpRuntime <= 0;

    protected override void Awake()
    {
        base.Awake();
        AnimatorHandler = this;
        FindTargetFunc = FindTargetDefault;
        MoveFunc = MoveDefault;
    }

    public virtual void Init(BattleProperty property = null, IBattleHandler battleHandler = null, EntityBase animatorHandler = null)
    {
        SetProperty(property);
        BattleHandler = battleHandler;
        HpRuntime = Property.Hp;
        Active = false;
        AnimatorHandler = animatorHandler;
        Animator.Init(this);
        Equipment.Init(this);
        Animator.Play(Animator.IdleClip);
        StartCoroutine(BattleCo());
    }

    public void SetProperty(BattleProperty property)
    {
        BaseProperty.Copy(property);
        Property.Copy(BaseProperty);
    }

    #region Default Impl

    public BattleEntity FindTargetDefault()
    {
        foreach (var target in TargetList)
        {
            if (target.IsDie) continue;
            return target;
        }

        return null;
    }

    public bool MoveDefault(float deltaTime)
    {
        if (IsDie) return false;
        var dis = Vector3.Distance(Position, Target.Position);
        var stopDistance = Property.Size + Target.Property.Size + Property.AttackRange;
        if (dis > stopDistance)
        {
            Animator.Play(Animator.RunClip);
            if (NavMeshAgent == null)
            {
                var forward = (Target.Position - Position).normalized;
                forward.y = 0f;
                Forward = Vector3.Lerp(Forward, forward, Property.RotateSpeed * deltaTime);
                Position += Forward * Property.RunSpeed * deltaTime;
            }
            else
            {
                NavMoveTo(Target.Position, Property.RunSpeed, stopDistance);
            }

            return false;
        }

        return true;
    }

    #endregion

    #region Enable / Disable

    public void EnableBattle()
    {
        Active = true;
        if (NavMeshAgent != null)
        {
            NavMeshAgent.enabled = true;
        }
    }

    public void DisableBattle()
    {
        Active = false;
        if (NavMeshAgent != null)
        {
            NavMeshAgent.enabled = false;
        }
    }

    #endregion

    #region Behaviour

    // [Button("Attack")]
    public void Attack()
    {
        Animator.Play(Animator.AttackClip);
    }

    public bool Attack(BattleEntity target)
    {
        Animator.Play(Animator.AttackClip);
        if (target == null) return false;
        if (BattleHandler != null)
        {
            var result = BattleHandler.OnBeforeAttack(target);
            if (!result) return false;
        }

        target.Damage(this, Property.Power);
        return true;
    }

    public void Damage(BattleEntity other, int power)
    {
        if (IsDie) return;
        HpRuntime -= power;
        // Play(HitClip);
        if (BattleHandler != null) BattleHandler.OnHit(other, power);
        if (HpRuntime <= 0)
        {
            HpRuntime = 0;
            Die();
        }
    }

    public void Die()
    {
        if (HpRuntime > 0) HpRuntime = 0;
        Animator.Play(Animator.DieClip);
        if (BattleHandler != null) BattleHandler.OnDie();
        if (NavMeshAgent != null && NavMeshAgent.enabled)
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.enabled = false;
        }

        this.ExecuteDelay(() =>
        {
            GamePool.DeSpawn(this);
        }, Animator.DieDuration);
    }

    #endregion

    #region Logic

    private float _attackTimer;

    public virtual IEnumerator BattleCo()
    {
        while (true)
        {
            while (!IsGaming)
            {
                yield return null;
            }

            while (!Active)
            {
                yield return null;
            }

            while (IsDie)
            {
                yield return null;
            }

            var deltaTime = DeltaTime;
            if (Target == null)
            {
                if (!Animator.IsIdle)
                {
                    Animator.Play(Animator.IdleClip);
                }

                Target = FindTargetFunc();
                _attackTimer = Property.AttackInterval;
            }
            else
            {
                var moveFinish = MoveFunc(deltaTime);
                if (moveFinish && Target != null)
                {
                    _attackTimer += deltaTime;
                    if (_attackTimer >= Property.AttackInterval)
                    {
                        _attackTimer = 0;
                        var attack = Attack(Target);
                        yield return YieldBuilder.WaitForSeconds(Animator.AttackDuration);
                        _attackTimer += Animator.AttackDuration;
                        if (Animator.AttackDuration < Property.AttackInterval)
                        {
                            Animator.Play(Animator.IdleClip);
                        }

                        if (attack)
                        {
                            if (BattleHandler != null) BattleHandler.OnAfterAttack(Target);
                        }
                    }

                    if (Target.IsDie)
                    {
                        // yield return YieldBuilder.WaitForSeconds(0.5f);
                        Target = null;
                    }
                }
            }

            yield return null;
        }
    }

    #endregion

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (Target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Position, Target.Position);
        }
    }
#endif
}
