#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public abstract partial class Tweener
    {
        [NonSerialized] public SerializedProperty TweenerProperty;

        [TweenerProperty, NonSerialized] public SerializedProperty ActiveProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty DurationProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty DelayProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty HoldStartProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty HoldEndProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty EaseProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty StrengthProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CurveModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty EditCurveProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CurveProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty SpaceProperty;

        [TweenerProperty, NonSerialized] internal SerializedProperty FoldOutProperty = null;
        [TweenerProperty, NonSerialized] internal SerializedProperty DurationModeProperty = null;

        public Editor Editor => Animation.Editor;
        public Object EditorTarget => Animation.EditorTarget;
        public MonoBehaviour MonoBehaviour => Animation.MonoBehaviour;
        public GameObject GameObject => Animation.GameObject;
        public SerializedObject SerializedObject => Animation.SerializedObject;

        [NonSerialized] public int Index = -1;

        public bool CanMoveDown => Index < Animation.TweenerList.Count - 1;
        public bool CanMoveUp => Index > 0;

        public TweenerEditorData EditorData
        {
            get
            {
                if (_editorData == null)
                {
                    _editorData = TypeCaches.TweenerEditorDataDic[GetType()];
                }

                if (_editorData.Type == null)
                {
                    _editorData.Cache(GetType());
                }

                return _editorData;
            }
        }

        private TweenerEditorData _editorData;

        public bool IsProgressFull => Math.Abs(Duration - Animation.Duration) <= 1e-6f && Math.Abs(Delay) <= 1e-6f;

        public bool ShowProgressBar
        {
            get
            {
                if (Animation.SingleMode) return false;
                if (FoldOut) return true;
                if (!Active) return false;
                if (Animation.IsPlaying) return true;
                if (IsProgressFull && UTweenEditorSetting.Ins.HideFullSubProgress) return false;
                return true;
            }
        }

        private static Tweener _clipboard;

        public UTweenEaseCurveWindow EaseCurveWindow;

        public virtual void InitParam(TweenAnimation animation, MonoBehaviour target)
        {
            Animation = animation;
            Duration = Animation.Duration;
        }

        public virtual void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            Index = index;
            Animation = animation;
            TweenerProperty = tweenerProperty;

            TweenerPropertyAttribute.CacheProperty(this, TweenerProperty);
            EaseCurveWindow = new UTweenEaseCurveWindow(this);
        }

        public virtual void DrawTweener()
        {
            using (GUIFoldOut.Create(FoldOutProperty, DrawTweenerHeader, DrawProgressBarWithHoldBtn))
            {
                if (Animation.SpeedBased && !SupportSpeedBased)
                {
                    GUIUtil.DrawTipArea(UTweenEditorSetting.Ins.ErrorColor, "Not Support Speed Based!");
                }

                if (FoldOut)
                {
                    using (GUIEnableArea.Create(ActiveProperty.boolValue && !Animation.IsInProgress))
                    {
                        DrawDocumentation();

                        if (SupportTarget) DrawTarget();
                        if (SupportIndependentAxis) DrawIndependentAxis();
                        if (SupportFromTo)
                        {
                            DrawFromToValue();
                            ClampFromToValue();
                        }

                        DrawBody();
                        DrawDurationDelay();
                        DrawEaseCurve();
                        DrawAppend();

                        // if (Data.Mode == TweenEditorMode.Component && SupportOnUpdate && ShowEvent)
                        // {
                        //     DrawEvent();
                        // }
                    }
                }
            }

            DrawGroupReminder();
        }

        public virtual void DrawGroupReminder()
        {
            if (!UTweenEditorSetting.Ins.ShowGroupReminder) return;
            var color = EditorData.GroupData.Color;
            var width = UTweenEditorSetting.Ins.GroupReminderWidth;
            var headerHeight = EditorGUIUtility.singleLineHeight + (ShowProgressBar ? 10 : 6);
            var rect = GUILayoutUtility.GetLastRect();

            rect.x -= width + 2;
            rect.width = width;
            var headerRect = rect;
            headerRect.height = headerHeight;
            EditorGUI.DrawRect(headerRect, color);
            var bodyRect = rect;
            rect.y += headerHeight;
            rect.height -= headerHeight;
            EditorGUI.DrawRect(bodyRect, color * 0.6f);
        }

        public virtual void DrawTweenerHeader()
        {
            // Icon
            DrawHeaderIcon();
            GUILayout.Space(2);

            // Active Toggle
            ActiveProperty.boolValue = EditorGUILayout.Toggle(ActiveProperty.boolValue, GUILayout.Width(EditorStyle.CharacterWidth));
            GUILayout.Space(2);

            // Title
            using (GUIHorizontal.Create())
            {
                DrawTitle();
            }

            var btnTitleRect = GUILayoutUtility.GetLastRect();
            var btnTweenerName = GUI.Button(btnTitleRect, GUIContent.none, EditorStyles.label);
            if (btnTweenerName)
            {
                FoldOutProperty.boolValue = !FoldOutProperty.boolValue;
            }

            DrawHeaderOptionButton();

            // Documentation
            if (EditorData.Documentation != null)
            {
                var btnDocumentation = GUIUtil.DrawHeaderDocumentationButton();
                if (btnDocumentation)
                {
                    UTweenEditorSetting.Ins.ShowDocumentation = !UTweenEditorSetting.Ins.ShowDocumentation;
                }
            }

            // Menu Button
            var btnContextMenu = GUIUtil.DrawOptionMenuButton();
            if (btnContextMenu)
            {
                var menu = CreateContextMenu();
                menu.ShowAsContext();
            }
        }

        public virtual void DrawTitle()
        {
            using (GUIEnableArea.Create(Active, false))
            {
                var name = EditorData.Info.DisplayName;
                if (UTweenEditorSetting.Ins.ShowTargetTypeInInspector && EditorData.TargetType != null)
                {
                    name = EditorData.TargetType.Name + " " + name;
                }

                GUILayout.Label(name, EditorStyles.boldLabel);
            }
        }

        public virtual void DrawHeaderIcon()
        {

        }

        public virtual void DrawHeaderOptionButton()
        {

        }

        public virtual void DrawProgressBarWithHoldBtn()
        {
            if (!ShowProgressBar) return;
            var holdBtnWidth = 14;
            var progressBarHeight = 10f;
            using (GUIHorizontal.Create(GUILayout.Height(progressBarHeight)))
            {
                using (GUIEnableArea.Create(!IsProgressFull && Active))
                {
                    GUIUtil.DrawHoldProgressToggleButton(HoldStartProperty, "", "Hold Start", holdBtnWidth, progressBarHeight);
                }

                if (IsProgressFull)
                {
                    HoldStartProperty.boolValue = false;
                }

                GUILayout.Space(1);

                DrawProgressBar();

                GUILayout.Space(1);

                using (GUIEnableArea.Create(!IsProgressFull && Active))
                {
                    GUIUtil.DrawHoldProgressToggleButton(HoldEndProperty, "", "Hold End", holdBtnWidth, progressBarHeight);
                }

                if (IsProgressFull)
                {
                    HoldEndProperty.boolValue = false;
                }
            }

            GUILayout.Space(1);
        }

        public virtual void DrawProgressBar()
        {
            var progressBarHeight = 10f;
            using (GUIVertical.Create())
            {
                GUILayout.Space(4);
                using (GUIEnableArea.Create(Active, false))
                {
                    var normalized = Animation.EditorNormalizedProgress;
                    if (!Active) normalized = 0f;
                    GUIUtil.DrawFromToDraggableProgressBar(SerializedObject.targetObject, progressBarHeight - 5,
                        DurationFromNormalized, DurationToNormalized, normalized,
                        HoldStartProperty.boolValue, HoldEndProperty.boolValue,
                        (from, to) =>
                        {
                            if (Animation.IsPlaying) return;
                            DurationFromNormalized = from;
                            DurationToNormalized = to;
                        });
                }
            }
        }

        public virtual void DrawTarget()
        {

        }

        public virtual void DrawIndependentAxis()
        {

        }

        public virtual void DrawFromToValue()
        {

        }

        public virtual void ClampFromToValue()
        {

        }

        public virtual void DrawDurationDelay()
        {
            using (GUIHorizontal.Create())
            {
                if (Animation.SingleMode)
                {
                    DurationProperty.floatValue = Animation.Duration;
                    DelayProperty.floatValue = 0f;
                    return;
                }

                var durationChanged = false;
                var delayChanged = false;

                if (DurationMode == DurationMode.DurationDelay)
                {
                    using (var check = GUICheckChangeArea.Create())
                    {
                        DurationProperty.floatValue = (float) Math.Round(EditorGUILayout.FloatField(nameof(Duration), DurationProperty.floatValue), 3);
                        durationChanged = check.Changed;
                    }

                    using (var check = GUICheckChangeArea.Create())
                    {
                        DelayProperty.floatValue = (float) Math.Round(EditorGUILayout.FloatField(nameof(Delay), DelayProperty.floatValue), 3);
                        delayChanged = check.Changed;
                    }
                }

                if (DurationMode == DurationMode.FromTo)
                {
                    GUILayout.Label("Time", EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
                    using (GUILabelWidthArea.Create(EditorStyle.CharacterWidth))
                    {
                        using (var check = GUICheckChangeArea.Create())
                        {
                            DelayProperty.floatValue = (float) Math.Round(GUIUtil.DrawFloatProperty("F", DelayProperty.floatValue, true), 3);
                            delayChanged = check.Changed;
                        }

                        using (var check = GUICheckChangeArea.Create())
                        {
                            DurationProperty.floatValue = (float) Math.Round(GUIUtil.DrawFloatProperty("T", DelayProperty.floatValue + DurationProperty.floatValue, true) - DelayProperty.floatValue, 3);
                            durationChanged = check.Changed;
                        }
                    }
                }

                GUIUtil.DrawSelectEnumButton(DurationModeProperty, typeof(DurationMode));

                if (DurationProperty.floatValue <= 0) DurationProperty.floatValue = Animation.DurationProperty.floatValue;
                if (durationChanged)
                {
                    if (DurationProperty.floatValue <= 0) DurationProperty.floatValue = 1e-6f;
                    if (DurationProperty.floatValue + DelayProperty.floatValue > Animation.DurationProperty.floatValue)
                    {
                        DurationProperty.floatValue = Animation.DurationProperty.floatValue - DelayProperty.floatValue;
                    }
                }

                if (delayChanged)
                {
                    if (DelayProperty.floatValue < 0) DelayProperty.floatValue = 0;
                    else if (DurationProperty.floatValue + DelayProperty.floatValue > Animation.DurationProperty.floatValue)
                    {
                        DelayProperty.floatValue = Animation.DurationProperty.floatValue - DurationProperty.floatValue;
                    }
                }
            }
        }

        public virtual void DrawEaseCurve()
        {
            EaseCurveWindow.DrawEaseCurve();
        }

        public virtual void DrawDocumentation()
        {
            if (!UTweenEditorSetting.Ins.ShowDocumentation || EditorData.Documentation == null) return;
            using (GUIColorArea.Create(UTweenEditorSetting.Ins.DocumentationColor))
            {
                using (GUIGroup.Create())
                {
                    var documentation = EditorData.Documentation;
                    EditorGUILayout.LabelField("[?] " + documentation.Title, EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("  " + documentation.Document, EditorStyles.label);
                }
            }
        }

        public virtual void DrawBody()
        {
        }

        public virtual void DrawEvent()
        {
        }

        public virtual void DrawAppend()
        {
            if (SupportSpace)
            {
                GUIUtil.DrawToolbarEnum(SpaceProperty, nameof(Space), typeof(SpaceMode));
            }
        }

        #region Context Menu

        public void MoveUp()
        {
            if (!CanMoveUp) return;
            Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Move Up");
            (Animation.TweenerList[Index - 1], Animation.TweenerList[Index]) = (Animation.TweenerList[Index], Animation.TweenerList[Index - 1]);
        }

        public void MoveDown()
        {
            if (!CanMoveDown) return;
            Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Move Down");
            (Animation.TweenerList[Index + 1], Animation.TweenerList[Index]) = (Animation.TweenerList[Index], Animation.TweenerList[Index + 1]);
        }

        public virtual GenericMenu CreateContextMenu()
        {
            var menu = new GenericMenu();

            // Reset
            menu.AddItem(new GUIContent("Reset"), false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Reset Tweener");
                Reset();
            });

            // Remove
            menu.AddItem(new GUIContent("Remove Tweener"), false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Remove Tweener");
                Animation.TweenerList.RemoveAt(Index);
                OnRemoved();
            });
            menu.AddSeparator("");

            // Move Up / Move Down / Auto Sort
            menu.AddItem(CanMoveUp, "Move Up", false, MoveUp);

            menu.AddItem(CanMoveDown, "Move Down", false, MoveDown);

            if (Animation.TweenerList.Count > 1)
            {
                menu.AddItem(true, "Auto Sort", false, () => { Animation.AutoSortTweener(); });
            }

            menu.AddSeparator("");

            // Duplicate / Copy / Paste
            menu.AddItem("Duplicate", false, () =>
            {
                Undo.RecordObject(SerializedObject.targetObject, "Duplicate Tweener");
                var tweener = (Tweener) Activator.CreateInstance(this.GetType());
                EditorUtility.CopySerializedManagedFieldsOnly(this, tweener);
                Animation.AddTweener(tweener);
            });
            menu.AddItem("Copy Tweener", false, () =>
            {
                _clipboard = (Tweener) Activator.CreateInstance(GetType());
                EditorUtility.CopySerializedManagedFieldsOnly(this, _clipboard);
            });
            var canPasteAsNew = _clipboard != null;
            menu.AddItem(canPasteAsNew, "Paste Tweener As New", false, () =>
            {
                Undo.RecordObject(SerializedObject.targetObject, "Paste Tweener As New");
                var tweener = (Tweener) Activator.CreateInstance(_clipboard.GetType());
                EditorUtility.CopySerializedManagedFieldsOnly(_clipboard, tweener);
                Animation.AddTweener(tweener);
            });
            var canPasteValues = _clipboard != null && _clipboard.GetType() == GetType();
            menu.AddItem(canPasteValues, "Paste Tweener Values", false, () =>
            {
                Undo.RecordObject(SerializedObject.targetObject, "Paste Tweener Values");
                EditorUtility.CopySerializedManagedFieldsOnly(_clipboard, this);
            });
            menu.AddSeparator("");

            // Show / Hide Event
            // if (SupportOnUpdate && Data.Mode == TweenEditorMode.Component)
            // {
            //     menu.AddItem(ShowEvent ? "Hide Callback Event" : "Show Callback Event", false, () =>
            //     {
            //         Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Switch Event");
            //         ShowEvent = !ShowEvent;
            //         if (!ShowEvent) ResetCallback();
            //     });
            // }

            // Curve

            return menu;
        }

        #endregion

        public virtual void OnSceneGUI()
        {
        }
    }
}
#endif