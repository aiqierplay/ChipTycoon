#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public partial class TweenAnimation
    {
        [NonSerialized] public TweenEditorMode Mode;
        [NonSerialized] public Editor Editor;
        [NonSerialized] public SerializedProperty TweenDataProperty;

        [TweenerProperty, NonSerialized] public SerializedProperty IdentifierProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty TweenerListProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty DurationProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty DelayProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty BackwardProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty PlayModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty PlayCountProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty AutoPlayProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty UpdateModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty IntervalProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty Interval2Property;
        [TweenerProperty, NonSerialized] public SerializedProperty TimeModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty SelfScaleProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty PrepareSampleModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty AutoKillProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty SpeedBasedProperty;

        [TweenerProperty, NonSerialized] public SerializedProperty OnStartProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnPlayProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnLoopStartProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnLoopEndProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnUpdateProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnPauseProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnResumeProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnStopProperty = null;
        [TweenerProperty, NonSerialized] public SerializedProperty OnCompleteProperty = null;

        [TweenerProperty, NonSerialized] internal SerializedProperty FoldOutProperty = null;
        [TweenerProperty, NonSerialized] internal SerializedProperty FoldOutCallbackProperty = null;
        [TweenerProperty, NonSerialized] internal SerializedProperty EnableIdentifierProperty = null;
        [TweenerProperty, NonSerialized] internal SerializedProperty EventTypeProperty = null;

        [NonSerialized] public float EditorNormalizedProgress;
        [NonSerialized] public bool PreviewSampled = false;
        [NonSerialized] public bool HasObjectChanged;

        public Object EditorTarget => Editor.target;
        public MonoBehaviour MonoBehaviour => EditorTarget as MonoBehaviour;
        public GameObject GameObject => EditorTarget as GameObject;
        public SerializedObject SerializedObject => Editor.serializedObject;

        public void InitEditor(TweenEditorMode mode, Editor editor)
        {
            Mode = mode;
            Editor = editor;
            PreviewSampled = false;

            TweenDataProperty = SerializedObject.FindProperty(nameof(Animation));
            TweenerPropertyAttribute.CacheProperty(this, TweenDataProperty);

            foreach (var tweener in TweenerList)
            {
                tweener.Index = -1;
            }

            if (Mode == TweenEditorMode.Component)
            {
                RecordObject();
            }
        }

        public virtual void OnInspectorGUI()
        {
            if (Editor == null) return;
            if (SerializedObject.isEditingMultipleObjects)
            {
                SerializedObject.ApplyModifiedProperties();
                return;
            }

            using (var check = GUICheckChangeArea.Create(Editor.target))
            {
                using (GUIWideMode.Create(true))
                {
                    using (GUILabelWidthArea.Create(EditorStyle.LabelWidth))
                    {
                        DrawProgressBar();
                        using (GUIEnableArea.Create(!IsInProgress))
                        {
                            DrawAnimation();
                            DrawTweenerList();
                            DrawAppend();
                            DrawCallback();
                        }
                    }
                }

                HasObjectChanged = check.Changed;
            }
        }

        #region Draw Progress Bar
      
        public virtual void DrawProgressBar()
        {
            if (Mode == TweenEditorMode.ScriptableObject) return;
            using (GUIGroup.Create())
            {
                using (GUIHorizontal.Create())
                {
                    var progressBarHeight = EditorGUIUtility.singleLineHeight;
                    var isInProgress = IsInProgress;
                    using (GUIColorArea.Create(UTweenEditorSetting.Ins.ProgressColor, isInProgress))
                    {
                        var btnContent = isInProgress ? EditorStyle.PlayButtonOn : EditorStyle.PlayButton;
                        var btnPlay = GUILayout.Button(btnContent, EditorStyles.miniButtonMid, GUILayout.Width(EditorGUIUtility.singleLineHeight));
                        if (btnPlay)
                        {
                            if (!isInProgress)
                            {
                                ControlMode = TweenControlMode.Component;
                                State = PlayState.None;
                                ResetCacheState();
                                Play();
                            }
                            else
                            {
                                Stop();
                            }
                        }
                    }

                    if (isInProgress)
                    {
                        EditorNormalizedProgress = RuntimeNormalizedProgress;
                    }

                    GUIUtil.DrawDraggableProgressBar(SerializedObject.targetObject, progressBarHeight, EditorNormalizedProgress,
                        value=>{},
                        value =>
                        {
                            Stop();
                            RestoreObject();
                        },
                        value =>
                        {
                            if (isInProgress) return;
                            EditorNormalizedProgress = value;
                            try
                            {
                                ResetCacheState();
                                Initialize(true);
                                Sample(EditorNormalizedProgress);
                            }
                            catch (Exception e)
                            {
                                UTweenCallback.OnException(e);
                            }
                        });

                    if (!string.IsNullOrEmpty(Identifier))
                    {
                        var rect = GUILayoutUtility.GetLastRect();
                        GUI.Label(rect, Identifier, EditorStyles.centeredGreyMiniLabel);
                    }

                    using (GUIEnableArea.Create(!IsInProgress))
                    {
                        var btnDirection = GUILayout.Button(Backward ? "←" : "→", EditorStyles.miniButtonMid, GUILayout.Width(EditorGUIUtility.singleLineHeight));
                        if (btnDirection)
                        {
                            BackwardProperty.boolValue = !BackwardProperty.boolValue;
                        }
                    }
                }
            }
        }

        #endregion

        #region Draw Animation Data

        private float _originalDuration;
        private bool _durationChanged;

        public void DrawAnimation()
        {
            _originalDuration = DurationProperty.floatValue;
            _durationChanged = false;

            using (GUIFoldOut.Create(FoldOutProperty, DrawAnimationHeader))
            {
                if (!FoldOut) return;

                // ID
                if (EnableIdentifier)
                {
                    EditorGUILayout.PropertyField(IdentifierProperty, new GUIContent("ID"));
                }

                using (GUIHorizontal.Create())
                {
                    using (var check = GUICheckChangeArea.Create())
                    {
                        var durationName = nameof(Duration);
                        if (SpeedBased) durationName = "Speed";
                        EditorGUILayout.PropertyField(DurationProperty, new GUIContent(durationName));

                        if (check.Changed)
                        {
                            _durationChanged = true;
                        }
                    }

                    EditorGUILayout.PropertyField(DelayProperty, new GUIContent(nameof(Delay)));
                }

                using (GUIHorizontal.Create())
                {
                    var playModeChanged = false;
                    using (var check = GUICheckChangeArea.Create())
                    {
                        EditorGUILayout.PropertyField(PlayModeProperty, new GUIContent("Play"));
                        playModeChanged = check.Changed;
                    }

                    if (playModeChanged)
                    {
                        if (PlayModeProperty.intValue == (int) PlayMode.Once)
                        {
                            PlayCountProperty.intValue = 1;
                        }
                        else if (PlayModeProperty.intValue == (int) PlayMode.Loop || PlayModeProperty.intValue == (int) PlayMode.PingPong)
                        {
                            PlayCountProperty.intValue = 0;
                        }
                    }

                    using (GUIEnableArea.Create(PlayMode != PlayMode.Once))
                    {
                        EditorGUILayout.PropertyField(PlayCountProperty, new GUIContent("Count"));
                    }
                }

                using (GUIHorizontal.Create())
                {
                    EditorGUILayout.PropertyField(UpdateModeProperty, new GUIContent("Update"));

                    if (PlayMode == PlayMode.PingPong)
                    {
                        var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.label);
                        var width = rect.width;
                        rect.width = EditorStyle.LabelWidth;
                        GUI.Label(rect, nameof(Interval), EditorStyles.label);
                        rect.x += EditorStyle.LabelWidth + 2f;
                        rect.width = (width - rect.width - 6f) / 2f;
                        IntervalProperty.floatValue = EditorGUI.FloatField(rect, IntervalProperty.floatValue);
                        if (Interval < 0f) IntervalProperty.floatValue = 0f;
                        rect.x += rect.width + 3f;
                        Interval2Property.floatValue = EditorGUI.FloatField(rect, Interval2Property.floatValue);
                        if (Interval2 < 0f) IntervalProperty.floatValue = 0f;
                    }
                    else
                    {
                        using (GUIEnableArea.Create(PlayMode != PlayMode.Once))
                        {
                            EditorGUILayout.PropertyField(IntervalProperty);
                        }
                    }
                }

                using (GUIHorizontal.Create())
                {
                    EditorGUILayout.PropertyField(AutoPlayProperty, new GUIContent("Auto"));
                    EditorGUILayout.PropertyField(PrepareSampleModeProperty, new GUIContent("Prepare"));
                }

                using (GUIHorizontal.Create())
                {
                    EditorGUILayout.PropertyField(TimeModeProperty, new GUIContent("Time"));
                    using (GUIColorArea.Create(UTweenEditorSetting.Ins.SpecialValueReminderColor, Mathf.Abs(SelfScaleProperty.floatValue -1f) > 1e-6))
                    {
                        EditorGUILayout.PropertyField(SelfScaleProperty, new GUIContent("Scale"));
                    }
                }

                using (GUIHorizontal.Create())
                {
                    using (GUIEnableArea.Create(SingleMode))
                    {
                        GUIUtil.DrawToggleButton(SpeedBasedProperty);
                        if (!SingleMode)
                        {
                            SpeedBasedProperty.boolValue = false;
                        }
                    }

                    GUIUtil.DrawToggleButton(AutoKillProperty);
                }
            }
        }

        public void DrawAnimationHeader()
        {
            // Header
            var btnTitle = GUILayout.Button(nameof(Animation), EditorStyles.boldLabel);
            var headerRect = GUILayoutUtility.GetLastRect();
            var info = "";
            if (!FoldOut)
            {
                if (AutoPlay != AutoPlayMode.None)
                {
                    info += "| " + AutoPlay + " ";
                }

                if (PlayMode != PlayMode.Once)
                {
                    info += "| " + PlayMode + " (" + PlayCount + ") ";
                }

                if (TimeMode != TimeMode.Normal)
                {
                    info += "| " + TimeMode + " ";
                    ;
                }

                if (Math.Abs(SelfScale - 1f) > 1e-6)
                {
                    info += "| " + SelfScale;
                }
            }

            var btnFlexibleInfo = GUILayout.Button(info, EditorStyles.label, GUILayout.MinWidth(0), GUILayout.MaxWidth(Screen.width));
            if (btnTitle || btnFlexibleInfo)
            {
                FoldOutProperty.boolValue = !FoldOutProperty.boolValue;
            }

            using (GUIEnableArea.Create(!IsInProgress))
            {
                // Identifier
                var btnIdentifier = GUIUtil.DrawHeaderIdentifierButton(EnableIdentifierProperty);
                if (btnIdentifier && !EnableIdentifierProperty.boolValue)
                {
                    IdentifierProperty.stringValue = "";
                }

                // Asset
                if (Mode == TweenEditorMode.Component)
                {
                    var btnImportAsset = GUIUtil.DrawImportPresetButton("Import Preset");
                    if (btnImportAsset)
                    {
                        var tweenPlayer = TweenPlayer;
                        var editor = Editor;
                        var assetList = Resources.LoadAll<UTweenAnimationPreset>("");
                        var dropRect = headerRect;
                        var menu = GUIMenu.CreateSearchableDropdownMenu("Import Preset",
                            assetList,
                            asset => asset.name,
                            asset => EditorIcon.ScriptObject,
                            asset =>
                            {
                                if (asset == null) return;
                                Undo.RegisterCompleteObjectUndo(tweenPlayer, "Import Preset");
                                tweenPlayer.ApplyAsset(asset);
                                tweenPlayer.Animation.TweenPlayer = tweenPlayer;
                                tweenPlayer.Animation.InitEditor(TweenEditorMode.Component, editor);
                                editor.serializedObject.ApplyModifiedProperties();
                            });
                        menu.Show(dropRect);
                    }
                }
               
                // Context Menu
                // var btnContextMenu = GUIUtil.DrawOptionMenuButton();
                // if (btnContextMenu)
                // {
                //     var menu = CreateContextMenu();
                //     menu.ShowAsContext();
                // }
            }
        }

        #endregion

        #region Draw Tweener List
       
        public virtual void DrawTweenerList()
        {
            if (TweenerList.Count == 0)
            {
                GUIUtil.DrawTipArea(UTweenEditorSetting.Ins.ErrorColor, "No Tweener");
                return;
            }

            for (var i = 0; i < TweenerList.Count; i++)
            {
                var tweener = TweenerList[i];
                if (tweener.Index != i || tweener.TweenerProperty == null || tweener.Animation == null)
                {
                    var tweenerProperty = TweenerListProperty.GetArrayElementAtIndex(i);
                    tweener.InitEditor(i, this, tweenerProperty);
                }

                // Sync tweeners duration and delay
                if (_durationChanged)
                {
                    var durationChangeRate = DurationProperty.floatValue / _originalDuration;
                    tweener.DelayProperty.floatValue *= durationChangeRate;
                    tweener.DurationProperty.floatValue *= durationChangeRate;
                }

                tweener.DrawTweener();
            }
        } 

        #endregion

        #region Draw Callback

        public virtual void DrawCallback()
        {
            if (Mode != TweenEditorMode.Component) return;
            if (!FoldOutCallback) return;
            using (GUIGroup.Create("Callback"))
            {
                var btnStyle = EditorStyles.miniButton;
                using (GUIHorizontal.Create())
                {
                    using (GUIVertical.Create())
                    {
                        if (GUILayout.Toggle(CallbackType == CallbackType.OnStart, nameof(CallbackType.OnStart), btnStyle))
                        {
                            OnStart.InitEditor(TweenDataProperty, nameof(OnStart));
                            CallbackType = CallbackType.OnStart;
                        }

                        if (GUILayout.Toggle(CallbackType == CallbackType.OnLoopStart, nameof(CallbackType.OnLoopStart), btnStyle))
                        {
                            OnLoopStart.InitEditor(TweenDataProperty, nameof(OnLoopStart));
                            CallbackType = CallbackType.OnLoopStart;
                        }

                        if (GUILayout.Toggle(CallbackType == CallbackType.OnResume, nameof(CallbackType.OnResume), btnStyle))
                        {
                            OnResume.InitEditor(TweenDataProperty, nameof(OnResume));
                            CallbackType = CallbackType.OnResume;
                        }
                    }

                    using (GUIVertical.Create())
                    {
                        if (GUILayout.Toggle(CallbackType == CallbackType.OnPlay, nameof(CallbackType.OnPlay), btnStyle))
                        {
                            OnPlay.InitEditor(TweenDataProperty, nameof(OnPlay));
                            CallbackType = CallbackType.OnPlay;
                        }

                        if (GUILayout.Toggle(CallbackType == CallbackType.OnLoopEnd, nameof(CallbackType.OnLoopEnd), btnStyle))
                        {
                            OnLoopEnd.InitEditor(TweenDataProperty, nameof(OnLoopEnd));
                            CallbackType = CallbackType.OnLoopEnd;
                        }

                        if (GUILayout.Toggle(CallbackType == CallbackType.OnStop, nameof(CallbackType.OnStop), btnStyle))
                        {
                            OnStop.InitEditor(TweenDataProperty, nameof(OnStop));
                            CallbackType = CallbackType.OnStop;
                        }
                    }

                    using (GUIVertical.Create())
                    {
                        if (GUILayout.Toggle(CallbackType == CallbackType.OnPause, nameof(CallbackType.OnPause), btnStyle))
                        {
                            OnPause.InitEditor(TweenDataProperty, nameof(OnPause));
                            CallbackType = CallbackType.OnPause;
                        }

                        if (GUILayout.Toggle(CallbackType == CallbackType.OnUpdate, nameof(CallbackType.OnUpdate), btnStyle))
                        {
                            OnUpdate.InitEditor(TweenDataProperty, nameof(OnUpdate));
                            CallbackType = CallbackType.OnUpdate;
                        }

                        if (GUILayout.Toggle(CallbackType == CallbackType.OnComplete, nameof(CallbackType.OnComplete), btnStyle))
                        {
                            OnComplete.InitEditor(TweenDataProperty, nameof(OnComplete));
                            CallbackType = CallbackType.OnComplete;
                        }
                    }
                }

                switch (CallbackType)
                {
                    case CallbackType.OnPlay:
                        OnPlay.DrawEvent(nameof(OnPlay));
                        break;
                    case CallbackType.OnStart:
                        OnStart.DrawEvent(nameof(OnStart));
                        break;
                    case CallbackType.OnUpdate:
                        OnUpdate.DrawEvent(nameof(OnUpdate));
                        break;
                    case CallbackType.OnLoopStart:
                        OnLoopStart.DrawEvent(nameof(OnLoopStart));
                        break;
                    case CallbackType.OnLoopEnd:
                        OnLoopEnd.DrawEvent(nameof(OnLoopEnd));
                        break;
                    case CallbackType.OnPause:
                        OnPause.DrawEvent(nameof(OnPause));
                        break;
                    case CallbackType.OnResume:
                        OnResume.DrawEvent(nameof(OnResume));
                        break;
                    case CallbackType.OnStop:
                        OnStop.DrawEvent(nameof(OnStop));
                        break;
                    case CallbackType.OnComplete:
                        OnComplete.DrawEvent(nameof(OnComplete));
                        break;
                }
            }
        }

        #endregion

        #region Draw Append

        public virtual void DrawAppend()
        {
            using (GUIGroup.Create())
            {
                using (GUIHorizontal.Create())
                {
                    // Add Tweener
                    var buttonRect = EditorGUILayout.GetControlRect();
                    var btnAddTweener = GUI.Button(buttonRect, "＋ Add Tweener");
                    if (btnAddTweener)
                    {
                        var menu = CreateTweenerMenu(tweenerType =>
                        {
                            var tweener = Activator.CreateInstance(tweenerType) as Tweener;
                            if (tweener == null) return;
                            Undo.RecordObject(SerializedObject.targetObject, "Add Tweener");
                            tweener.Reset();
                            tweener.InitParam(this, Mode == TweenEditorMode.Component ? MonoBehaviour : null);
                            TweenerList.Add(tweener);
                            tweener.OnAdded();
                        });
                        menu.Show(buttonRect);
                    }

                    // FoldOut Callback
                    var btnFoldOutCallback = GUIUtil.DrawCallbackButton(FoldOutCallbackProperty);
                    if (btnFoldOutCallback && !FoldOutCallbackProperty.boolValue)
                    {
                        ResetCallback();
                    }
                }
            }
        }

        public static SearchableDropdown CreateTweenerMenu(Action<Type> onClick = null)
        {
            var tweenerEditorDataDic = TypeCaches.TweenerEditorDataDic;
            var root = new SearchableDropdownItem($"Tweener ({tweenerEditorDataDic.Count})");
            var menu = new SearchableDropdown(root, item =>
            {
                var tweenerType = item.Value as Type;
                if (tweenerType == null) return;
                onClick?.Invoke(tweenerType);
            });

            foreach (var groupKv in UTweenEditorSetting.Ins.GroupDataDic)
            {
                var group = groupKv.Key;
                var groupItem = new SearchableDropdownItem(group)
                {
                    icon = groupKv.Value.Icon
                };

                root.AddChild(groupItem);

                var lastOrder = -1;
                Type lastType = null;
                foreach (var tweenerKv in tweenerEditorDataDic)
                {
                    var tweenerType = tweenerKv.Key;
                    var tweenerEditorData = tweenerKv.Value;

                    var tweenerGroup = tweenerEditorData.Info.Group;
                    if (tweenerGroup != group) continue;

                    var name = tweenerEditorData.Info.DisplayName;
                    if (UTweenEditorSetting.Ins.ShowTargetTypeInCreateMenu && tweenerEditorData.TargetType != null)
                    {
                        name = tweenerEditorData.TargetType.Name + " " + name;
                    }

                    var order = tweenerEditorData.Info.Order;
                    var targetType = tweenerEditorData.TargetType;
                    var item = new SearchableDropdownItem(name, tweenerType)
                    {
                        icon = EditorIcon.GetTweenerIcon(tweenerType)
                    };

                    var needSeparator = order - lastOrder >= 10 || (lastType != null && lastType != targetType);
                    if (needSeparator) groupItem.AddSeparator();
                    lastOrder = order;
                    lastType = targetType;

                    groupItem.AddChild(item);
                }
            }

            return menu;
        }

        #endregion

        #region Context Menu

        public virtual GenericMenu CreateContextMenu()
        {
            var menu = new GenericMenu();

            // Identifier
            menu.AddItem(EnableIdentifier ? "Disable Identifier" : "Enable Identifier", false, () =>
            {
                Undo.RegisterCompleteObjectUndo(SerializedObject.targetObject, "Switch Identifier");
                EnableIdentifier = !EnableIdentifier;
                if (!EnableIdentifier)
                {
                    Identifier = null;
                }
            });

            // Auto Sort
            if (TweenerList.Count > 1)
            {
                menu.AddItem(true, "Auto Sort", false, AutoSortTweener);
            }

            return menu;
        }

        public virtual void AutoSortTweener()
        {
            TweenerList.Sort((t1, t2) =>
            {
                if (Math.Abs(t1.Delay - t2.Delay) > 1e-3)
                {
                    return t1.Delay.CompareTo(t2.Delay);
                }
                else
                {
                    if (Math.Abs(t1.Duration - t2.Duration) > 1e-3)
                    {
                        return t1.Duration.CompareTo(t2.Duration);
                    }
                    else
                    {
                        if (t1.EditorData.Info.Group != t2.EditorData.Info.Group)
                        {
                            return string.Compare(t1.EditorData.Info.Group, t2.EditorData.Info.Group, StringComparison.Ordinal);
                        }
                        else
                        {
                            return string.Compare(t1.EditorData.Info.DisplayName, t2.EditorData.Info.DisplayName, StringComparison.Ordinal);
                        }
                    }
                }
            });
        }

        #endregion

        public virtual void OnSceneGUI()
        {
            if (TweenerList.Count == 0)
            {
                return;
            }

            for (var i = 0; i < TweenerList.Count; i++)
            {
                var tweener = TweenerList[i];
                tweener.OnSceneGUI();
            }
        }

    }
}
#endif