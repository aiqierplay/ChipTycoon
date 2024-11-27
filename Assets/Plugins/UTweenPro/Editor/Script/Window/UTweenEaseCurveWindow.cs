#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Aya.TweenPro
{
    public class UTweenEaseCurveWindow : PopupWindowContent
    {
        public SerializedProperty EaseProperty;
        public SerializedProperty CurveModeProperty;
        public SerializedProperty EditCurveProperty;
        public SerializedProperty CurveProperty;
        public SerializedProperty StrengthProperty;

        public Vector2 WindowSize => new Vector2(400, 300);

        public bool IsCustomCurve => EaseProperty.intValue < 0;
        public EaseFunction CacheEaseFunction => EaseType.FunctionDic[EaseProperty.intValue];

        private string _searchEaseType;
        private Vector2 _curveScrollPos;

        public UTweenEaseCurveWindow(Tweener tweener)
        {
            EaseProperty = tweener.EaseProperty;
            CurveModeProperty = tweener.CurveModeProperty;
            EditCurveProperty = tweener.EditCurveProperty;
            CurveProperty = tweener.CurveProperty;
            StrengthProperty = tweener.StrengthProperty;
        }

        public void Init()
        {
            _searchEaseType = "";
            _curveScrollPos = Vector2.zero;
        }

        public override Vector2 GetWindowSize()
        {
            return WindowSize;
        }

        public override void OnGUI(Rect rect)
        {
            using (GUIRectArea.Create(rect))
            {
                GUILayout.Label(" Select Ease Curve", EditorStyles.boldLabel);
                GUILayout.Space(2);

                using (GUIHorizontal.Create())
                {
                    using (GUIGroup.Create(GUILayout.Width(125)))
                    {
                        DrawSelectEaseType();
                    }

                    using (GUIGroup.Create())
                    {
                        DrawCurvePreviewArea();
                    }
                }
            }
        }

        #region Editor Update

        internal double LastTimeSinceStartup = -1f;

        public override void OnOpen()
        {
            CachePreviewCurve();
            LastTimeSinceStartup = -1f;
            EditorApplication.update += EditorUpdate;
        }

        public override void OnClose()
        {
            EditorApplication.update -= EditorUpdate;
        }

        internal void EditorUpdate()
        {
            var currentTime = EditorApplication.timeSinceStartup;
            if (LastTimeSinceStartup < 0f)
            {
                LastTimeSinceStartup = currentTime;
            }

            var deltaTime = (float)(currentTime - LastTimeSinceStartup);
            LastTimeSinceStartup = currentTime;
            _previewTimer += deltaTime;
            if (_previewTimer >= 1f + _previewInterval)
            {
                _previewTimer = 0f;
            }

            editorWindow.Repaint();
        }

        #endregion

        #region Draw Select Ease Type

        public void DrawSelectEaseType()
        {
            GUILayout.Label(" Ease Type", EditorStyles.boldLabel);
            GUILayout.Space(2);

            _searchEaseType = EditorGUILayout.TextField(_searchEaseType, EditorStyles.toolbarSearchField);

            using (GUIScrollView.Create(ref _curveScrollPos, false, true))
            {
                var group = "";
                foreach (var kv in EaseType.FunctionInfoDic)
                {
                    var index = kv.Key;
                    var easeAttribute = kv.Value;
                    var easeFunction = EaseType.FunctionDic[index];
                    var displayName = "     " + easeAttribute.DisplayName;

                    // Search
                    if (!string.IsNullOrEmpty(_searchEaseType))
                    {
                        var searchName = _searchEaseType.ToLower().Replace(" ", "");
                        if (!string.IsNullOrEmpty(searchName))
                        {
                            var currentName = displayName.ToLower().Replace(" ", "");
                            if (!currentName.Contains(searchName)) continue;
                        }
                    }

                    var currentGroup = easeAttribute.Group;
                    if (currentGroup != group)
                    {
                        group = currentGroup;
                        var titleColor = new Color(0.8f, 0.8f, 0.8f);
                        using (GUIBackgroundColorArea.Create(titleColor))
                        {
                            GUILayout.Label(group, EditorStyle.ListSelectionButton);
                        }
                    }

                    var select = EaseProperty.intValue == index;
                    using (GUIBackgroundColorArea.Create(UTweenEditorSetting.Ins.EnableColor, select))
                    {
                        var btnSelect = GUILayout.Toggle(select, displayName, EditorStyle.ListSelectionButton);
                        if (btnSelect)
                        {
                            EaseProperty.intValue = index;
                            if (!easeFunction.SupportStrength)
                            {
                                StrengthProperty.floatValue = 0f;
                            }

                            EaseProperty.serializedObject.ApplyModifiedProperties();
                        }
                    }

                    var btnRect = GUILayoutUtility.GetLastRect();
                    var btnHover = btnRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseMove;
                    if (btnHover)
                    {
                        _cacheEaseCurve(easeFunction.Type);
                    }
                }
            }
        }

        #endregion

        #region Draw Curve Preview

        public void DrawCurvePreviewArea()
        {
            var currentEaseAttribute = EaseType.FunctionInfoDic[_cacheType];
            GUILayout.Label(" Curve Preview - " + currentEaseAttribute.DisplayName, EditorStyles.boldLabel);
            GUILayout.Space(2);

            if (IsCustomCurve)
            {
                var curveModeName = "";
                var curveMode = (EaseCurveMode)CurveModeProperty.intValue;
                if (curveMode != EaseCurveMode.TimePosition)
                {
                    switch (curveMode)
                    {
                        case EaseCurveMode.TimeVelocity:
                            curveModeName = "Time - Velocity";
                            break;
                        case EaseCurveMode.TimeAcceleration:
                            curveModeName = "Time - Acceleration";
                            break;
                    }

                    GUILayout.Label(curveModeName);
                    using (var check = GUICheckChangeArea.Create(editorWindow))
                    {
                        EditCurveProperty.animationCurveValue = EditorGUILayout.CurveField(EditCurveProperty.animationCurveValue);
                        if (check.Changed)
                        {
                            CachePreviewCurve();
                        }
                    }

                    GUILayout.Label("Time - Position");
                }
            }

            DrawCurvePreview(_cacheCurve);
        }

        public void CachePreviewCurve()
        {
            switch ((EaseCurveMode)CurveModeProperty.intValue)
            {
                case EaseCurveMode.TimePosition:
                    CurveProperty.animationCurveValue = EditCurveProperty.animationCurveValue;
                    break;
                case EaseCurveMode.TimeVelocity:
                    CurveProperty.animationCurveValue = AnimationCurveUtil.ConvertTv2Tp(EditCurveProperty.animationCurveValue);
                    break;
                case EaseCurveMode.TimeAcceleration:
                    CurveProperty.animationCurveValue = AnimationCurveUtil.ConvertTa2Tp(EditCurveProperty.animationCurveValue);
                    break;
            }

            _clearEaseCurvePreview();
            _cacheEaseCurve(EaseProperty.intValue);
        }

        public void DrawCurvePreview(AnimationCurve curve)
        {
            // Color
            var borderColor = Color.black;
            var outRangeBackColor = new Color(0.2f, 0.2f, 0.2f);
            var inRangeBackColor = new Color(0.3f, 0.3f, 0.3f);
            var currentLineColor = Color.cyan;
            var lineColor = Color.green;

            var inRangeProgressColor = new Color(0.3f, 0.65f, 0.3f);
            var outRangeProgressColor = new Color(0f, 0.35f, 0f);
            var currentRangeProgressColor = new Color(0f, 1f, 0f, 0.5f);
            var timeBlockColor = new Color(0.8f, 0.8f, 0.8f);
            var valueBlockColor = new Color(1f, 0.8f, 0.25f);

            var timeValueAreaWidth = 20f;
            var timeValueProgressBarWidth = 5f;
            var timeValueAreaSpace = 2f;
            var timeBlockSize = new Vector2(8, 20);
            var valueBlockSize = new Vector2(20, 8);

            using (GUIHorizontal.Create())
            {
                var contentRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));

                // Curve Size
                var curveRect = new Rect(contentRect.position, new Vector2(contentRect.width - timeValueAreaWidth - timeValueAreaSpace, contentRect.height - timeValueAreaWidth - timeValueAreaSpace));
                var startX = curveRect.x;
                var startY = curveRect.y;
                var width = curveRect.width;
                var height = curveRect.height;

                var curveMinValue = _curveMinValue;
                var curveMaxValue = _curveMaxValue;
                var height01 = height / (curveMaxValue - curveMinValue);
                var value0Pos = startY + height - (0f - curveMinValue) * height01;
                var value1Pos = startY + height - (1f - curveMinValue) * height01;
                var curveInRangeRect = new Rect(new Vector2(startX, value1Pos), new Vector2(width, value0Pos - value1Pos));

                // Current Value
                var currentTime = Mathf.Clamp01(_previewTimer);
                var currentValue = curve.Evaluate(currentTime);
                var currentPoint = GetPreviewCurePoint(new Vector2(currentTime, currentValue), curveRect, curveMinValue, curveMaxValue);

                EditorGUI.DrawRect(curveRect, outRangeBackColor);
                EditorGUI.DrawRect(curveInRangeRect, inRangeBackColor);

                // Curve
                for (var i = 0f; i < 1f - _previewStep; i += _previewStep)
                {
                    var point1 = new Vector2(i, curve.Evaluate(i));
                    var start = GetPreviewCurePoint(point1, curveRect, curveMinValue, curveMaxValue);

                    var point2 = new Vector2(i + _previewStep, curve.Evaluate(i + _previewStep));
                    var end = GetPreviewCurePoint(point2, curveRect, curveMinValue, curveMaxValue);

                    GUIUtil.DrawLine(start, end, lineColor, 3);
                }

                // Current Curve State
                GUIUtil.DrawLine(currentPoint, new Vector2(currentPoint.x, curveRect.y + curveRect.height), currentLineColor, 1.5f);
                GUIUtil.DrawLine(currentPoint, new Vector2(curveRect.x + curveRect.width, currentPoint.y), currentLineColor, 1.5f);

                GUIUtil.DrawBox(curveRect, borderColor);

                // Time
                var progressBarSpace = (timeValueAreaWidth - timeValueProgressBarWidth) / 2f + timeValueAreaSpace;
                var timeRect = new Rect(curveRect.x, curveRect.y + curveRect.height + progressBarSpace, curveRect.width, timeValueProgressBarWidth);
                EditorGUI.DrawRect(timeRect, outRangeProgressColor);
                var timeCurrentRect = new Rect(curveRect.x, curveRect.y + curveRect.height + progressBarSpace, currentPoint.x - curveRect.x, timeValueProgressBarWidth);
                EditorGUI.DrawRect(timeCurrentRect, currentRangeProgressColor);

                // Time Block
                var timeBlocPos = new Vector2(currentPoint.x - timeBlockSize.x / 2f, curveRect.y + curveRect.height + timeValueAreaSpace + timeValueAreaWidth / 2f - timeBlockSize.y / 2f);
                var timeBlockRect = new Rect(timeBlocPos, timeBlockSize);
                EditorGUI.DrawRect(timeBlockRect, timeBlockColor);
                GUIUtil.DrawBox(timeBlockRect, borderColor);

                // Value
                var valueRect = new Rect(curveRect.x + curveRect.width + progressBarSpace, curveRect.y, timeValueProgressBarWidth, curveRect.height);
                EditorGUI.DrawRect(valueRect, outRangeProgressColor);
                var valueRangeRect = new Rect(curveRect.x + curveRect.width + progressBarSpace, curveInRangeRect.y, timeValueProgressBarWidth, curveInRangeRect.height);
                EditorGUI.DrawRect(valueRangeRect, inRangeProgressColor);
                var valueCurrentRect = new Rect(curveRect.x + curveRect.width + progressBarSpace, currentPoint.y, timeValueProgressBarWidth, value0Pos - currentPoint.y);
                EditorGUI.DrawRect(valueCurrentRect, currentRangeProgressColor);

                // Value Block
                var valueBlocPos = new Vector2(curveRect.x + curveRect.width + timeValueAreaSpace + timeValueAreaWidth / 2f - valueBlockSize.x / 2f, currentPoint.y - valueBlockSize.y / 2f);
                var valueBlockRect = new Rect(valueBlocPos, valueBlockSize);
                EditorGUI.DrawRect(valueBlockRect, valueBlockColor);
                GUIUtil.DrawBox(valueBlockRect, borderColor);
            }
        }

        public Vector2 GetPreviewCurePoint(Vector2 curvePoint, Rect curveRect, float minValue, float maxValue)
        {
            var startX = curveRect.x;
            var startY = curveRect.y;
            var width = curveRect.width;
            var height = curveRect.height;
            var x = curvePoint.x * width + startX;
            var height01 = height / (maxValue - minValue);
            var y = height - ((0f - minValue) + curvePoint.y) * height01 + startY;
            return new Vector2(x, y);
        }

        #endregion

        #region Draw Curve Inspector

        public virtual void DrawEaseCurve()
        {
            using (GUIHorizontal.Create())
            {
                // Ease
                GUILayout.Label("Ease", EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                var displayEaseName = EaseType.FunctionInfoDic[EaseProperty.intValue].DisplayName;
                var easeRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.popup);
                var easeTypeBtn = GUI.Button(easeRect, displayEaseName, EditorStyles.popup);
                if (easeTypeBtn)
                {
                    var windowRect = new Rect
                    {
                        position = Event.current.mousePosition
                    };

                    windowRect.y += EditorGUIUtility.singleLineHeight;
                    Init();
                    PopupWindow.Show(windowRect, this);
                }

                if (IsCustomCurve)
                {
                    // Curve Mode
                    var modeBtnName = "";
                    switch ((EaseCurveMode)CurveModeProperty.intValue)
                    {
                        case EaseCurveMode.TimePosition:
                            modeBtnName = "TP";
                            break;
                        case EaseCurveMode.TimeVelocity:
                            modeBtnName = "TV";
                            break;
                        case EaseCurveMode.TimeAcceleration:
                            modeBtnName = "TA";
                            break;
                    }

                    GUIUtil.DrawSelectEnumButton(CurveModeProperty, typeof(EaseCurveMode), modeBtnName);
                }

                // Curve
                GUILayout.Label("Curve", EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                if (IsCustomCurve)
                {
                    using (var check = GUICheckChangeArea.Create(editorWindow))
                    {
                        EditCurveProperty.animationCurveValue = EditorGUILayout.CurveField(EditCurveProperty.animationCurveValue);
                        if (check.Changed)
                        {
                            CachePreviewCurve();
                        }
                    }
                }
                else
                {
                    using (GUIEnableArea.Create(false))
                    {
                        _cacheEaseCurve(EaseProperty.intValue);
                        EditorGUILayout.CurveField(_cacheCurve);
                    }
                }

                if (IsCustomCurve)
                {
                    // Option Menu
                    if (GUIUtil.DrawOptionMenuButton("Option"))
                    {
                        var menu = CreateCurveMenu();
                        menu.ShowAsContext();
                    }
                }
            }

            // Strength
            if (CacheEaseFunction.SupportStrength)
            {
                using (GUIHorizontal.Create())
                {
                    using (var check = GUICheckChangeArea.Create())
                    {
                        GUILayout.Label("Strength", EditorStyles.label, GUILayout.Width(EditorStyle.LabelWidth));
                        StrengthProperty.floatValue = GUILayout.HorizontalSlider(StrengthProperty.floatValue, 0f, 1f);

                        if (check.Changed)
                        {
                            _clearEaseCurvePreview();
                        }
                    }
                }
            }
        }

        public virtual GenericMenu CreateCurveMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem("Reset Curve", false, () =>
            {
                var curve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));
                EditCurveProperty.animationCurveValue = curve;
                CachePreviewCurve();
                CurveProperty.serializedObject.ApplyModifiedProperties();
            });

            menu.AddItem("Reverse Curve", false, () =>
            {
                EditCurveProperty.animationCurveValue = EditCurveProperty.animationCurveValue.Reverse();
                CachePreviewCurve();
                CurveProperty.serializedObject.ApplyModifiedProperties();
            });

            return menu;
        }

        #endregion

        #region Cache Preview Curve Info

        private AnimationCurve _cacheCurve;
        private int _cacheType;
        private float _cacheStrength;

        private float _curveMaxValue;
        private float _curveMinValue;

        private float _previewTimer;
        private float _previewInterval = 0.5f;
        private float _previewStep = 0.01f;

        private void _clearEaseCurvePreview()
        {
            _cacheCurve = null;
            _cacheType = -2;
            _cacheStrength = 1f;
            _curveMaxValue = 1f;
            _curveMinValue = 0f;
            _previewTimer = -0.2f;
        }

        private void _cacheEaseCurve(int easeType)
        {
            if (_cacheType == easeType && Math.Abs(_cacheStrength - StrengthProperty.floatValue) < 1e-6f && _cacheCurve != null) return;
            _clearEaseCurvePreview();
            _cacheType = easeType;
            _cacheStrength = StrengthProperty.floatValue;
            _cacheCurve = new AnimationCurve();
            EaseFunction easeFunction = null;
            var customCurve = CurveProperty.animationCurveValue;
            if (easeType >= 0) easeFunction = EaseType.FunctionDic[easeType];
            for (var time = 0f; time <= 1f; time += _previewStep)
            {
                float value;
                if (easeType >= 0)
                {
                    value = easeFunction.Ease(0f, 1f, time, StrengthProperty.floatValue);
                }
                else
                {
                    value = customCurve.Evaluate(time);
                }

                _cacheCurve.AddKey(time, value);
                if (value < _curveMinValue) _curveMinValue = value;
                if (value > _curveMaxValue) _curveMaxValue = value;
            }
        }


        #endregion
    }
}
#endif