#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.TweenPro
{
    public class UTweenMonitorWindow : EditorWindow
    {
        #region Menu
        
        public static UTweenMonitorWindow Instance;

        [MenuItem("Tools/Aya Game/UTween Pro/UTween Monitor", false, 0)]
        public static void OpenWindow()
        {
            if (Instance == null)
            {
                Instance = GetWindow<UTweenMonitorWindow>();
                Instance.titleContent.text = "UTween Monitor";
                Instance.maximized = false;
            }

            Instance.Show();
        } 

        #endregion

        public TreeViewState TreeViewState;
        public MultiColumnHeaderState MultiColumnHeaderState;
        public MultiColumnHeader MultiColumnHeader;
        public UTweenTreeView TreeView;

        public void OnEnable()
        {
            UTweenCallback.OnAddAnimation += OnRefresh;
            UTweenCallback.OnRemoveAnimation += OnRefresh;
        }

        public void OnDisable()
        {
            UTweenCallback.OnAddAnimation -= OnRefresh;
            UTweenCallback.OnRemoveAnimation -= OnRefresh;
        }

        public void OnRefresh(TweenAnimation tweenAnimation)
        {
            if (TreeView == null) return;
            TreeView.Reload();
        }

        public void CreateTree()
        {
            TreeViewState = new TreeViewState();
            MultiColumnHeaderState = TweenRowData.CreateMultiColumnHeaderState();
            MultiColumnHeader = new MultiColumnHeader(MultiColumnHeaderState);
            TreeView = new UTweenTreeView(TreeViewState, MultiColumnHeader);
        }

        public void OnGUI()
        {
            if (!Application.isPlaying) return;
            if (UTweenManager.Ins.PlayingList.Count == 0) return;
            if (TreeView == null) CreateTree();
            using (GUIHorizontal.Create())
            {
                using (GUIVertical.Create())
                {
                    DrawGrid();
                    DrawOverview();
                }
              
                DrawSelectionInfo();
            }

            Repaint();
        }

        public void DrawOverview()
        {
            using (GUIGroup.Create("Overview"))
            {
                var manager = UTweenManager.Ins;
                using (GUIVertical.Create())
                {
                    using (GUIHorizontal.Create())
                    {
                        var info = $"<color=cyan>{UTweenPool.ActiveCount}</color>/" +
                                   $"<color=red>{UTweenPool.DeActiveCount}</color>/" +
                                   $"<color=white>{UTweenPool.Count}</color>";
                        GUILayout.Label("Pool : " + info, EditorStyle.RichLabel);
                    }

                    using (GUIHorizontal.Create())
                    {
                        var pool = UTweenPool.GetPoolList<TweenAnimation>();
                        var info = $"<color=cyan>{pool.ActiveCount}</color>/" +
                                   $"<color=red>{pool.DeActiveCount}</color>/" +
                                   $"<color=white>{pool.Count}</color>";
                        GUILayout.Label("Animation : " + info, EditorStyle.RichLabel);
                    }

                    using (GUIHorizontal.Create())
                    {
                        var info = $"<color=cyan>{manager.PlayingList.Count}</color>/" +
                                   $"<color=yellow>{manager.AddList.Count}</color>/" +
                                   $"<color=red>{manager.RemoveList.Count}</color>/" +
                                   $"<color=white>{manager.Count}</color>";
                        GUILayout.Label("Playing : " + info, EditorStyle.RichLabel);
                    }
                }
            }
        }

        public void DrawGrid()
        {
            using (GUIGroup.Create("Playing Tweener"))
            {
                using (GUIGroup.Create())
                {
                    // GUILayout.TextArea("", EditorStyles.toolbarSearchField);
                    using (GUIVertical.Create(GUILayout.ExpandHeight(true)))
                    {
                    }

                    var rect = GUILayoutUtility.GetLastRect();
                    TreeView.OnGUI(rect);
                }
            }
        }

        public int SelectId = -1;
        public TweenAnimation SelectTweenAnimation;
        private Vector2 _infoScrollPos;

        public void DrawSelectionInfo()
        {
            if (!TreeView.HasSelection()) return;
            var selectId = TreeView.GetSelection()[0];
            if (SelectId != selectId)
            {
                SelectId = selectId;
                var selection = TreeView.DataDic[selectId];
                var tweenAnimation = selection.Tweener.Animation;
                var targetObject = tweenAnimation.TweenPlayer;
                if (targetObject != null)
                {
                    var serializedObject = new SerializedObject(targetObject);
                    var inspectorType = typeof(UTweenPlayerEditor);
                    var editor = Editor.CreateEditor(serializedObject.targetObject, inspectorType);
                    tweenAnimation.InitEditor(TweenEditorMode.Component, editor);
                    SelectTweenAnimation = tweenAnimation;
                }
                else
                {
                    SelectTweenAnimation = tweenAnimation;
                }
            }

            if (SelectTweenAnimation.ControlMode == TweenControlMode.Component)
            {
                DrawTweenAnimationInspector();
            }
            else
            {
                DrawTweenAnimationDebugger();
            }
        }

        public void DrawTweenAnimationInspector()
        {
            using (GUIGroup.Create("Inspector", GUILayout.Width(250), GUILayout.MinWidth(200), GUILayout.MaxWidth(400)))
            {
                using (GUIScrollView.Create(ref _infoScrollPos, false, false))
                {

                    using (GUIHorizontal.Create())
                    {
                        GUILayout.Space(14);
                        using (GUIVertical.Create())
                        {
                            SelectTweenAnimation.OnInspectorGUI();
                        }
                    }
                }
            }
        }

        public void DrawTweenAnimationDebugger()
        {
            using (GUIGroup.Create("Debugger", GUILayout.Width(250), GUILayout.MinWidth(200), GUILayout.MaxWidth(400)))
            {
                using (GUIScrollView.Create(ref _infoScrollPos, false, false))
                {

                    using (GUIHorizontal.Create())
                    {
                        GUILayout.Space(14);
                        using (GUIVertical.Create())
                        {
                            DrawDebugInfo(SelectTweenAnimation);
                        }
                    }
                }
            }
        }

        public void DrawDebugInfo(object target)
        {
            var targetType = target.GetType();
            using (GUIGroup.Create(targetType.Name))
            {
                GUILayout.Space(2);
                using (GUIHorizontal.Create())
                {
                    GUILayout.Space(16);
                    using (GUIVertical.Create())
                    {
                        var fieldInfoList = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var field in fieldInfoList)
                        {
                            DrawField(target, field);
                        }
                    }
                }
            }
        }

        public void DrawField(object target, FieldInfo fieldInfo)
        {
            var fieldName = fieldInfo.Name;
            var fieldType = fieldInfo.FieldType;
            var value = fieldInfo.GetValue(target);
            if (fieldType == typeof(float))
            {
                EditorGUILayout.FloatField(fieldName, (float)value);
            }
            else if (fieldType == typeof(int))
            {
                EditorGUILayout.IntField(fieldName, (int)value);
            }
            else if (fieldType == typeof(string))
            {
                EditorGUILayout.TextField(fieldName, (string)value);
            }
            else if (fieldType.IsEnum)
            {
                EditorGUILayout.EnumPopup(fieldName, (Enum)value);
            }
            else if (fieldType == typeof(bool))
            {
                EditorGUILayout.Toggle(fieldName, (bool) value);
            }
            else if (fieldType == typeof(Color))
            {
                EditorGUILayout.ColorField(fieldName, (Color) value);
            }
            else if(fieldType == typeof(Vector2))
            {
                EditorGUILayout.Vector2Field(fieldName, (Vector2)value);
            }
            else if (fieldType == typeof(Vector2Int))
            {
                EditorGUILayout.Vector2IntField(fieldName, (Vector2Int)value);
            }
            else if (fieldType == typeof(Vector3))
            {
                EditorGUILayout.Vector3Field(fieldName, (Vector3)value);
            }
            else if (fieldType == typeof(Vector3Int))
            {
                EditorGUILayout.Vector3IntField(fieldName, (Vector3Int)value);
            }
            else if (fieldType == typeof(Vector4))
            {
                EditorGUILayout.Vector4Field(fieldName, (Vector4)value);
            }
            else if (fieldType == typeof(Bounds))
            {
                EditorGUILayout.BoundsField(fieldName, (Bounds)value);
            }
            else if (fieldType == typeof(BoundsInt))
            {
                EditorGUILayout.BoundsIntField(fieldName, (BoundsInt)value);
            }
            else if (fieldType == typeof(Rect))
            {
                EditorGUILayout.RectField(fieldName, (Rect)value);
            }
            else if (fieldType == typeof(RectInt))
            {
                EditorGUILayout.RectIntField(fieldName, (RectInt)value);
            }
            else if (fieldType == typeof(AnimationCurve))
            {
                EditorGUILayout.CurveField(fieldName, (AnimationCurve)value);
            }
            else if (fieldType.IsSubclassOf(typeof(Object)))
            {
                EditorGUILayout.ObjectField(fieldName, (Object) value, fieldType, true);
            }
            else if (fieldType.IsGenericType && typeof(List<>) == fieldType.GetGenericTypeDefinition())
            {
                using (GUIGroup.Create(fieldName))
                {
                    var list = (IList)fieldInfo.GetValue(target);
                    foreach (var item in list)
                    {
                        DrawDebugInfo(item);
                    }
                }
            }
        }
    }
}

#endif