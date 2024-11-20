using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public abstract class ItemGambleValue<TTarget, TBullet> : ItemChangeValue<EntityBase>
    where TTarget : EntityBase
    where TBullet : EntityBase
{
    [Title("Gamble")]
    public GameObject DefaultRender;
    public TMP_Text GambleText;
    public int BulletValue = 1;
    public bool DeSpawnAfterEffect = true;

    public List<ItemBase> ActiveList;

    [NonSerialized] public int GambleTextIndex;
    [NonSerialized] public int BulletCounter;
    [NonSerialized] public string GambleStr;

    [NonSerialized] public bool IsGambleTextShowComplete;

    public override void Init()
    {
        base.Init();
        GambleTextIndex = 0;
        BulletCounter = 0;
        IsGambleTextShowComplete = false;

        this.ExecuteNextFrame(() =>
        {
            foreach (var item in ActiveList)
            {
                item.Active = false;
            }
        });

        Refresh();
    }

    public virtual void RefreshGamble()
    {
        if (!IsGambleTextShowComplete)
        {
            GambleStr = "";
            var charList = CurrentStrValue.ToCharArray();
            var index = charList.Length - 1;
            for (; index >= charList.Length - GambleTextIndex; index--)
            {
                try
                {
                    GambleStr = charList[index] + GambleStr;
                }
                catch
                {
                    Log(name, charList.Length, index, CurrentStrValue);
                }
            }

            for (; index >= 0; index--)
            {
                GambleStr = "?" + GambleStr;
            }

            GambleText.text = GambleStr;
        }
        else
        {
            GambleText.text = CurrentStrValue;
        }
    }

    public override void RefreshShowObj()
    {
        if (!IsGambleTextShowComplete)
        {
            if (DefaultRender != null) DefaultRender.SetActive(true);
            if (NegativeRender != null) NegativeRender.SetActive(false);
            if (PositiveRender != null) PositiveRender.SetActive(false);
        }
        else
        {
            if (DefaultRender != null) DefaultRender.SetActive(false);
            base.RefreshShowObj();
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        RefreshGamble();
    }

    public override float GetValue(EntityBase target)
    {
        return GetValue(target as TTarget);
    }

    public override void SetValue(EntityBase target, float value)
    {
        SetValue(target as TTarget, value);
    }

    public abstract float GetValue(TTarget target);
    public abstract void SetValue(TTarget target, float value);

    public override void OnTargetEffect(EntityBase target)
    {
        if (target is TTarget targetValue)
        {
            if (IsGambleTextShowComplete)
            {
                OnTargetEffectValue(targetValue);
                if (DeSpawnAfterEffect)
                {
                    Complete();
                    DeSpawn();
                }
            }
        }

        if (target is TBullet bullet)
        {
            GamePool.DeSpawn(bullet);
            if (!IsGambleTextShowComplete)
            {
                BulletCounter++;
                if (BulletCounter >= BulletValue)
                {
                    BulletCounter = 0;
                    GambleTextIndex++;
                }

                this.ExecuteNextFrame(() =>
                {
                    if (GambleTextIndex >= CurrentStrValue.Length)
                    {
                        IsGambleTextShowComplete = true;
                    }

                    Refresh();
                });
            }
            else
            {
                foreach (var item in ActiveList)
                {
                    item.Active = true;
                }
            }
        }
    }
}