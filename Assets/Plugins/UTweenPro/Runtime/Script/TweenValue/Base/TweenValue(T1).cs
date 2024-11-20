using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Aya.TweenPro
{
    public enum TweenValueMode
    {
        Value = 0,
        Current = 1,
        Random = 2,
        RandomEachLoop = 3,
        [HideInInspector] Func = 4,
    }

    [Serializable]
    public abstract partial class TweenValue<TValue> : ITweenerSubData
    {
        public TweenValueMode Mode;

        public TValue Value;

        public TValue RandomFrom;
        public TValue RandomTo;

        public Func<TValue> ValueGetter;

        public virtual bool SupportRandom => true;

        public bool GetValueOnPreSample => Mode == TweenValueMode.Value || Mode == TweenValueMode.Current || Mode == TweenValueMode.Random;
        public bool GetValueOnLoopStart => Mode == TweenValueMode.RandomEachLoop;

        public abstract TValue Random(TValue from, TValue to);

        public virtual TValue GetRandomValue() => Random(RandomFrom, RandomTo);

        public virtual TValue GetValue()
        {
            var mode = Mode;
            switch (mode)
            {
                case TweenValueMode.Value:
                    return Value;
                // Processing in Tweener
                case TweenValueMode.Current:
                    return Value;
                case TweenValueMode.Random:
                case TweenValueMode.RandomEachLoop:
                    return GetRandomValue();
                case TweenValueMode.Func:
                    return ValueGetter();
                default:
                    return default;
            }
        }

        public virtual void Reset()
        {
            Reset(default);
        }

        public virtual void Reset(TValue defaultValue)
        {
            Mode = TweenValueMode.Value;
            Value = defaultValue;
            RandomFrom = default;
            RandomTo = defaultValue;
            ValueGetter = null;
        }

        #region Static
        
        public static implicit operator TValue(TweenValue<TValue> value)
        {
            return value.Value;
        }

        public static void Switch(TweenValue<TValue> value1, TweenValue<TValue> value2)
        {
            (value1.Mode, value2.Mode) = (value2.Mode, value1.Mode);
            (value1.Value, value2.Value) = (value2.Value, value1.Value);
            (value1.RandomFrom, value2.RandomFrom) = (value2.RandomFrom, value1.RandomFrom);
            (value1.RandomTo, value2.RandomTo) = (value2.RandomTo, value1.RandomTo);
            (value1.ValueGetter, value2.ValueGetter) = (value2.ValueGetter, value1.ValueGetter);
        } 

        #endregion
    }

    #region Extension

    public abstract partial class TweenValue<TValue>
    {
        public virtual TweenValue<TValue> SetMode(TweenValueMode mode)
        {
            Mode = mode;
            return this;
        }

        public virtual TweenValue<TValue> SetValue(TValue value)
        {
            Mode = TweenValueMode.Value;
            Value = value;
            return this;
        }
    }

    #endregion

#if UNITY_EDITOR
    public abstract partial class TweenValue<TValue>
    {
        public SerializedProperty DataProperty { get; set; }

        [TweenerProperty, NonSerialized] public SerializedProperty ModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty ValueProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty RandomFromProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty RandomToProperty;

        [NonSerialized] internal bool AxisX;
        [NonSerialized] internal bool AxisY;
        [NonSerialized] internal bool AxisZ;
        [NonSerialized] internal bool AxisW;

        [NonSerialized] internal string AxisXName;
        [NonSerialized] internal string AxisYName;
        [NonSerialized] internal string AxisZName;
        [NonSerialized] internal string AxisWName;

        public virtual void DrawInspector()
        {
            switch (Mode)
            {
                case TweenValueMode.Value:
                    using (GUIHorizontal.Create())
                    {
                        DrawValueProperty(ValueProperty, DataProperty.name);
                        DrawValueModeButton();
                    }

                    break;
                case TweenValueMode.Current:
                    using (GUIHorizontal.Create())
                    {
                        GUIUtil.DrawLabel(DataProperty.name);
                        GUILayout.TextField("Current Value");
                        DrawValueModeButton();
                    }

                    break;
                case TweenValueMode.Random:
                case TweenValueMode.RandomEachLoop:
                    using (GUIHorizontal.Create())
                    {
                        using (GUIGroup.Create())
                        {
                            using (GUIVertical.Create())
                            {
                                DrawValueProperty(RandomFromProperty, DataProperty.name);
                                DrawValueProperty(RandomToProperty, " ");
                            }
                        }

                        DrawValueModeButton();
                    }

                    break;
                case TweenValueMode.Func:
                    using (GUIHorizontal.Create())
                    {
                        GUIUtil.DrawLabel(DataProperty.name);
                        using (GUIColorArea.Create(UTweenEditorSetting.Ins.ErrorColor))
                        {
                            GUILayout.TextField("Function Mode");
                        }
                       
                        DrawValueModeButton();
                    }

                    break;
            }
        }

        public abstract void DrawValueProperty(SerializedProperty property, string name);

        public virtual void DrawValueModeButton()
        {
            var btnName = "";
            switch (Mode)
            {
                case TweenValueMode.Value:
                    btnName = "V";
                    break;
                case TweenValueMode.Current:
                    btnName = "C";
                    break;
                case TweenValueMode.Random:
                    btnName = "R";
                    break;
                case TweenValueMode.RandomEachLoop:
                    btnName = "RL";
                    break;
                case TweenValueMode.Func:
                    btnName = "F";
                    break;
            }

            GUIUtil.DrawSelectEnumButton(ModeProperty, typeof(TweenValueMode), btnName);
        }
    }
#endif
}