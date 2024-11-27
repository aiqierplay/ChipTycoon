#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public abstract partial class Tweener<TTarget, TValue> : Tweener<TTarget>
    where TTarget : UnityEngine.Object
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty FromProperty;
        [TweenerProperty(true), NonSerialized] public SerializedProperty ToProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty AxisProperty;
        // [TweenerProperty, NonSerialized] public SerializedProperty OnUpdateEventProperty;

        [TweenerProperty, NonSerialized] internal SerializedProperty EnableAxisProperty = null;

        public virtual string AxisXName => nameof(AxisConstraint.X);
        public virtual string AxisYName => nameof(AxisConstraint.Y);
        public virtual string AxisZName => nameof(AxisConstraint.Z);
        public virtual string AxisWName => nameof(AxisConstraint.W);

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, animation, tweenerProperty);
        }

        public override void DrawHeaderOptionButton()
        {
            base.DrawHeaderOptionButton();
            // Independent Axis
            if (SupportIndependentAxis)
            {
                using (GUIEnableArea.Create(Active))
                {
                    GUIUtil.DrawHeaderAxisButton(EnableAxisProperty);
                }
            }
        }

        #region From To
       
        public override void DrawIndependentAxis()
        {
            if (!EnableAxis) return;
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(nameof(Axis), EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                {
                    var toggleAxises = new bool[4];
                    if (AxisCount >= 1) toggleAxises[0] = GUIUtil.DrawToggleButton(AxisXName, AxisX);
                    if (AxisCount >= 2) toggleAxises[1] = GUIUtil.DrawToggleButton(AxisYName, AxisY);
                    if (AxisCount >= 3) toggleAxises[2] = GUIUtil.DrawToggleButton(AxisZName, AxisZ);
                    if (AxisCount >= 4) toggleAxises[3] = GUIUtil.DrawToggleButton(AxisWName, AxisW);
                    var axis = 0;
                    for (var i = 0; i < 4; i++)
                    {
                        if (toggleAxises[i]) axis |= 2 << i;
                    }

                    AxisProperty.intValue = axis;
                }
            }
        }

        public override void DrawFromToValue()
        {
            CacheFromToPropertyAxisInfo(FromValueRef, ToValueRef);
            using (GUIVertical.Create())
            {
                using (GUIHorizontal.Create())
                {
                    FromValueRef.DrawInspector();
                    var fromContextMenuBtn = GUIUtil.DrawOptionMenuButton("Option");
                    if (fromContextMenuBtn)
                    {
                        var menu = CreateFromToContextMenu(FromValueRef);
                        menu.ShowAsContext();
                    }
                }

                using (GUIHorizontal.Create())
                {
                    ToValueRef.DrawInspector();
                    var toContextMenuBtn = GUIUtil.DrawOptionMenuButton("Option");
                    if (toContextMenuBtn)
                    {
                        var menu = CreateFromToContextMenu(ToValueRef);
                        menu.ShowAsContext();
                    }
                }
            }
        }

        public virtual GenericMenu CreateFromToContextMenu(TweenValue<TValue> fromToValue)
        {
            var menu = new GenericMenu();
            if (SupportFromTo)
            {
                menu.AddItem(new GUIContent("Reverse From - To"), false, () =>
                {
                    Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Reverse From - To");
                    ReverseFromTo();
                });

                // Current - From / To
                if (SupportSetCurrentValue)
                {
                    menu.AddSeparator("");
                    menu.AddItem(Target != null, "Set Current -> Value", false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Set Current -> Value");
                        fromToValue.Value = Value;
                    });

                    menu.AddItem(Target != null, "Set Value -> Current", false, () =>
                    {
                        Value = fromToValue.Value;
                    });
                }
            }

            return menu;
        }

        public virtual void CacheFromToPropertyAxisInfo(TweenValue<TValue> from, TweenValue<TValue> to)
        {
            from.AxisX = to.AxisX = AxisX;
            from.AxisY = to.AxisY = AxisY;
            from.AxisZ = to.AxisZ = AxisZ;
            from.AxisW = to.AxisW = AxisW;
            from.AxisXName = to.AxisXName = AxisXName;
            from.AxisYName = to.AxisYName = AxisYName;
            from.AxisZName = to.AxisZName = AxisZName;
            from.AxisWName = to.AxisWName = AxisWName;
        }

        public override void ClampFromToValue()
        {
            if (!SupportClampValue) return;
            if (RequireClampMin)
            {
                FromValueRef.Value = ClampMin(FromValueRef.Value);
                FromValueRef.RandomFrom = ClampMin(FromValueRef.RandomFrom);
                FromValueRef.RandomTo = ClampMin(FromValueRef.RandomTo);
                ToValueRef.Value = ClampMin(ToValueRef.Value);
                ToValueRef.RandomFrom = ClampMin(ToValueRef.RandomFrom);
                ToValueRef.RandomTo = ClampMin(ToValueRef.RandomTo);
            }

            if (RequireClampMax)
            {
                FromValueRef.Value = ClampMax(FromValueRef.Value);
                FromValueRef.RandomFrom = ClampMax(FromValueRef.RandomFrom);
                FromValueRef.RandomTo = ClampMax(FromValueRef.RandomTo);
                ToValueRef.Value = ClampMax(ToValueRef.Value);
                ToValueRef.RandomFrom = ClampMax(ToValueRef.RandomFrom);
                ToValueRef.RandomTo = ClampMax(ToValueRef.RandomTo);
            }
        }

        #endregion

        public override void DrawEvent()
        {

        }

        #region Context Menu

        public override GenericMenu CreateContextMenu()
        {
            var menu = base.CreateContextMenu();

            // Show / Hide Independent Axis
            if (SupportIndependentAxis)
            {
                menu.AddSeparator("");
                if (EnableAxis)
                {
                    menu.AddItem(new GUIContent("Disable Independent Axis"), false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Disable Independent Axis");
                        EnableAxisProperty.boolValue = !EnableAxisProperty.boolValue;
                        DisableIndependentAxis();
                        SerializedObject.ApplyModifiedProperties();
                    });
                }
                else
                {
                    menu.AddItem(new GUIContent("Enable Independent Axis"), false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Enable Independent Axis");
                        EnableAxisProperty.boolValue = !EnableAxisProperty.boolValue;
                        SerializedObject.ApplyModifiedProperties();
                    });
                }
            }

            // Reverse From To
            if (SupportFromTo)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Reverse From - To"), false, () =>
                {
                    Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Reverse From - To");
                    ReverseFromTo();
                });

                // Current - From / To
                if (SupportSetCurrentValue)
                {
                    menu.AddItem(Target != null, "Set Current -> From", false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Set Current -> From");
                        SetFrom(Value);
                    });
                    menu.AddItem(Target != null, "Set Current -> To", false, () =>
                    {
                        Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Set Current -> To");
                        SetTo(Value);
                    });
                    menu.AddItem(Target != null, "Set From -> Current", false, () =>
                    {
                        Value = GetFrom();
                    });
                    menu.AddItem(Target != null, "Set To -> Current", false, () =>
                    {
                        Value = GetTo();
                    });
                }
            }

            // Expand / Shrink 
            menu.AddSeparator("");
            menu.AddItem("Fold Out Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Fold Out Others");
                foreach (var tweener in Animation.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.FoldOut = true;
                }

                SerializedObject.ApplyModifiedProperties();
            });
            menu.AddItem("Fold In Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Fold In Others");
                foreach (var tweener in Animation.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.FoldOut = false;
                }

                SerializedObject.ApplyModifiedProperties();
            });

            // Active / DeActive
            menu.AddSeparator("");
            menu.AddItem("Active Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Active Others");
                foreach (var tweener in Animation.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.Active = true;
                }

                SerializedObject.ApplyModifiedProperties();
            });
            menu.AddItem("DeActive Others", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "DeActive Others");
                foreach (var tweener in Animation.TweenerList)
                {
                    if (tweener == this) continue;
                    tweener.Active = false;
                }

                SerializedObject.ApplyModifiedProperties();
            });

            return menu;
        }

        #endregion
    }
}
#endif