using System;
using Aya.Extension;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class EntityHealth : EntityBase
{
    [Title("State")]
    public GameObject StateObj;
    public Gradient HpGradient;
    public Renderer GradientRender;
    public Transform HpBar;
    public TMP_Text TextHp;
    public TMP_Text TextHpMax;
    public bool DefaultShow = true;

    [Title("Fx")]
    public GameObject HitFx;
    public GameObject DieFx;

    [Title("Animation")]
    public string HitClip;
    public string DieClip;

    [NonSerialized] public EntityBase Owner;
    [NonSerialized] public int HpMax;
    [NonSerialized] public int HpRuntime;

    public Action OnRefresh = delegate { };
    public Action OnDamage = delegate { };
    public Action OnDie = delegate { };

    public bool IsAlive => HpRuntime > 0;
    public bool IsDie => HpRuntime <= 0;
    public bool IsFull => HpRuntime >= HpMax;

    public float HpProgress => HpRuntime * 1f / HpMax;

    public void Init(EntityBase owner, int hp)
    {
        Owner = owner;
        if (StateObj != null) StateObj.SetActive(DefaultShow);
        HpRuntime = HpMax = hp;
        if (DefaultShow)
        {
            RefreshState();
        }
    }

    public void SetHp(int hp, int hpMax)
    {
        HpRuntime = hp;
        HpMax = hpMax;
        RefreshState();
    }

    public virtual void RefreshState()
    {
        if (TextHp != null) TextHp.text = Mathf.RoundToInt(HpRuntime).ToString();
        if (TextHpMax != null) TextHpMax.text = Mathf.RoundToInt(HpMax).ToString();
        if (HpBar != null) HpBar.SetLocalScaleX(HpProgress);

        if (GradientRender != null)
        {
            var progress = HpRuntime * 1f / HpMax;
            GradientRender.material.color = HpGradient.Evaluate(progress);
        }

        OnRefresh?.Invoke();
    }

    public virtual void DoDamage(float power)
    {
        if (IsDie) return;
        if (StateObj != null) StateObj.SetActive(true);
        HpRuntime -= Mathf.RoundToInt(power);
        Owner.SpawnFx(HitFx);
        if (!string.IsNullOrEmpty(HitClip)) Owner.Play(HitClip);
        OnDamage?.Invoke();
        if (HpRuntime <= 0) Die();
        RefreshState();
    }

    public virtual void Die()
    {
        HpRuntime = 0;
        Owner.SpawnFx(DieFx);
        if (!string.IsNullOrEmpty(DieClip)) Owner.Play(DieClip);
        OnDie?.Invoke();
    }
}
