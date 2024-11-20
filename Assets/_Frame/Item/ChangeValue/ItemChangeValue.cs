using System;
using System.Collections.Generic;
using Aya.Util;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public enum ItemValueChangeMode
{
    [LabelText("+")] Add = 0,
    [LabelText("-")] Reduce = 1,
    [LabelText("*")] Multiply = 2,
    [LabelText("/")] Divide = 3,
}

public enum ItemValueMode
{
    Value = 0,
    Random = 1,
}

public abstract class ItemChangeValue<TTarget> : ItemBase<TTarget>
    where TTarget : EntityBase
{
    [Title("Value")]
    [FoldoutGroup("Change Value"), LabelText("Change"), EnumToggleButtons] public ItemValueChangeMode ChangeMode;

    [FoldoutGroup("Change Value")] public List<string> ChangePrefix = new List<string>() { "+", "-", "x", "��" };

    [FoldoutGroup("Change Value"), LabelText("Mode"), EnumToggleButtons] public ItemValueMode ValueMode;
    [FoldoutGroup("Change Value"), ShowIf(nameof(ValueMode), ItemValueMode.Value)] public float Value = 1;
    [FoldoutGroup("Change Value"), ShowIf(nameof(ValueMode), ItemValueMode.Random)] public Vector2 RandomValue = new Vector2(0, 1);

    [FoldoutGroup("Change Value"), HorizontalGroup("Change Value/Min")] public bool ClampMin = true;
    [FoldoutGroup("Change Value"), EnableIf(nameof(ClampMin)), HideLabel, HorizontalGroup("Change Value/Min"), LabelWidth(0)] public float MinValue = 0f;
    [FoldoutGroup("Change Value"), HorizontalGroup("Change Value/Max")] public bool ClampMax = false;
    [FoldoutGroup("Change Value"), EnableIf(nameof(ClampMax)), HideLabel, HorizontalGroup("Change Value/Max"), LabelWidth(0)] public float MaxValue = 1f;

    [FoldoutGroup("Change Value")] public bool RoundToInt = true;

    [Title("Display")]
    [FoldoutGroup("Change Value")] public GameObject NegativeRender;
    [FoldoutGroup("Change Value")] public GameObject PositiveRender;
    [FoldoutGroup("Change Value")] public TMP_Text TextValue;
    [FoldoutGroup("Change Value")] public float ShowFactor = 1;
    [FoldoutGroup("Change Value")] public bool ShowToInt = false;
    [FoldoutGroup("Change Value"), HideIf(nameof(ShowToInt))] public int ShowDecimalPlaces = 2;

    public abstract float GetValue(TTarget target);
    public abstract void SetValue(TTarget target, float value);

    [NonSerialized] public float CurrentValue;
    [NonSerialized] public float CurrentRandomValue;
    [NonSerialized] public string CurrentStrValue;

    public override void Init()
    {
        base.Init();

        if (ValueMode == ItemValueMode.Random)
        {
            Value = 0f;
            CurrentRandomValue = RandUtil.RandFloat(RandomValue);
        }

        Refresh();
    }

    public virtual float GetValue()
    {
        var value = 0f;
        switch (ValueMode)
        {
            case ItemValueMode.Value:
                value = Value;
                break;
            case ItemValueMode.Random:
                value = CurrentRandomValue + Value;
                break;
        }

        return value;
    }

    public virtual float ProcessValue(float value)
    {
        if (ClampMin && value < MinValue) value = MinValue;
        if (ClampMax && value > MaxValue) value = MaxValue;
        if (RoundToInt) value = Mathf.RoundToInt(value);
        return value;
    }

    public virtual string GetStrValue()
    {
        string value;
        string prefix;
        if (ChangeMode == ItemValueChangeMode.Add)
        {
            if (CurrentValue >= 0)
            {
                prefix = ChangePrefix[0];
            }
            else
            {
                prefix = ChangePrefix[1];
            }
        }
        else if (ChangeMode == ItemValueChangeMode.Reduce)
        {
            if (CurrentValue >= 0)
            {
                prefix = ChangePrefix[1];
            }
            else
            {
                prefix = ChangePrefix[0];
            }
        }
        else
        {
            var modeIndex = (int)ChangeMode;
            prefix = ChangePrefix[modeIndex];
        }

        var showValue = Mathf.Abs(CurrentValue * ShowFactor);
        if (ShowToInt)
        {
            value = prefix + Mathf.RoundToInt(showValue);
        }
        else if (ShowDecimalPlaces > 0)
        {
            value = prefix + showValue.ToString("F" + ShowDecimalPlaces);
        }
        else
        {
            value = prefix + showValue;
        }
        return value;
    }

    public virtual void RefreshShowObj()
    {
        var isNegative = CurrentValue < 0 || (CurrentValue > 0 && (ChangeMode == ItemValueChangeMode.Reduce || ChangeMode == ItemValueChangeMode.Divide));
        if (NegativeRender != null) NegativeRender.gameObject.SetActive(isNegative);
        if (PositiveRender != null) PositiveRender.gameObject.SetActive(!isNegative);
    }

    public override void Refresh()
    {
        base.Refresh();
        CurrentValue = GetValue();
        CurrentStrValue = GetStrValue();
        if (TextValue != null)
        {
            TextValue.text = CurrentStrValue;
        }

        RefreshShowObj();
    }

    public override void OnTargetEffect(TTarget target)
    {
        OnTargetEffectValue(target);
    }

    public virtual void OnTargetEffectValue(TTarget target)
    {
        if (target == null) return;
        var originalValue = GetValue(target);
        var newValue = originalValue;
        switch (ChangeMode)
        {
            case ItemValueChangeMode.Add:
                newValue += CurrentValue;
                break;
            case ItemValueChangeMode.Reduce:
                newValue -= CurrentValue;
                break;
            case ItemValueChangeMode.Multiply:
                newValue *= CurrentValue;
                break;
            case ItemValueChangeMode.Divide:
                newValue /= CurrentValue;
                break;
        }

        if (newValue < originalValue && newValue < MinValue) OnNotEnough(target);
        newValue = ProcessValue(newValue);
        SetValue(target, newValue);
        if (newValue < originalValue) OnReduce(target);
        if (newValue > originalValue) OnAdd(target);
    }

    public virtual void OnAdd(TTarget target)
    {

    }

    public virtual void OnReduce(TTarget target)
    {

    }

    public virtual void OnNotEnough(TTarget target)
    {

    }
}
