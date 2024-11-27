using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenPathPointCollection
    {
        public List<TweenPathPoint> PointList = new List<TweenPathPoint>();

        public void Reset()
        {
            if (PointList == null) PointList = new List<TweenPathPoint>();
            else PointList.Clear();
        }
    }

#if UNITY_EDITOR
    public partial class TweenPathPointCollection : ITweenerSubData
    {
        public bool FoldOut = true;

        public SerializedProperty DataProperty { get; set; }

        [TweenerProperty, NonSerialized] public SerializedProperty PointListProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty FoldOutProperty;

        public void DrawInspector()
        {
            using (GUIFoldOut.Create(FoldOutProperty, DataProperty.displayName + " (" + PointList.Count + ")"))
            {
                if (!FoldOut) return;

                for (var index = 0; index < PointList.Count; index++)
                {
                    var point = PointList[index];
                    TweenerPropertyAttribute.CacheProperty(point, PointListProperty.GetArrayElementAtIndex(index));
                    using (GUIHorizontal.Create())
                    {
                        using (GUIGroup.Create())
                        {
                            point.PositionProperty.vector3Value = EditorGUILayout.Vector3Field("Pos", point.PositionProperty.vector3Value);
                            point.EulerAngleProperty.vector3Value = EditorGUILayout.Vector3Field("Rot", point.EulerAngleProperty.vector3Value);
                        }

                        using (GUIHorizontal.Create(GUILayout.Width(EditorStyle.SingleButtonWidth)))
                        {
                            var btnRemove = GUIUtil.DrawDeleteButton("Remove");
                            if (btnRemove)
                            {
                                RemovePoint(index);
                            }
                        }
                    }
                }

                using (GUIHorizontal.Create())
                {
                    var btnAdd = GUILayout.Button("Add");
                    if (btnAdd)
                    {
                        AddPoint(new TweenPathPoint());
                    }

                    var btnClear = GUILayout.Button("Clear");
                    if (btnClear)
                    {
                        ClearPoint();
                    }
                }
            }
        }

        internal void AddPoint(TweenPathPoint point)
        {
            UndoOperate(() =>
            {
                PointList.Add(point); 

            },"Add Array Item");
        }

        internal void RemovePoint(int index)
        {
            UndoOperate(() =>
            {
                PointList.RemoveAt(index);

            }, "Remove Array Item");
        }

        internal void ClearPoint()
        {
            UndoOperate(() =>
            {
                PointList.Clear();
            }, "Clear Array Item");
        }

        internal void UndoOperate(Action action, string operateInfo)
        {
            Undo.RegisterCompleteObjectUndo(DataProperty.serializedObject.targetObject, operateInfo);
            action?.Invoke();
            DataProperty.serializedObject.ApplyModifiedProperties();
        }

        private int _windowId = 0;
        private Rect _windowRect;

        // private bool _editMode = false;
        private PathOperationMode _operationMode = PathOperationMode.Position;
        private int _selectIndex = -1;

        public virtual void OnSceneGUI()
        {
            DrawOperateWindow();
            DrawPathPointList();
        }

        internal void DrawOperateWindow()
        {
            var width = 300f;
            _windowRect.x = 10f;
            _windowRect.y = 30f;
            _windowRect.width = width;
            _windowRect.height = 170f;
            _windowRect = GUILayout.Window(_windowId, _windowRect, id =>
            {
                GUILayout.Space(5);
                using (GUILabelWidthArea.Create(EditorStyle.LabelWidth))
                {
                    using (GUIGroup.Create())
                    {
                        if (_selectIndex >= 0 && _selectIndex < PointList.Count)
                        {
                            var point = PointList[_selectIndex];
                            point.Position = EditorGUILayout.Vector3Field("Position", point.Position);
                            point.EulerAngle = EditorGUILayout.Vector3Field("EulerAngle", point.EulerAngle);
                        }
                    }

                    GUILayout.Space(10);

                    using (GUIHorizontal.Create(GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        _operationMode = (PathOperationMode)GUIUtil.DrawToolbarEnum((int)_operationMode, "Operate", typeof(PathOperationMode));
                    }

                    GUILayout.Space(5);

                    using (GUIHorizontal.Create())
                    {
                        using (GUIColorArea.Create(UTweenEditorSetting.Ins.EnableColor))
                        {
                            GUILayout.Button("＋ Add");
                        }

                        using (GUIColorArea.Create(UTweenEditorSetting.Ins.ErrorColor))
                        {
                            GUILayout.Button("× Delete");
                        }
                    }
                }
                
                GUI.DragWindow(_windowRect);
            }, "UTween Path Editor", GUILayout.Width(width));
        }

        internal void DrawPathPointList()
        {
            for (var index = 0; index < PointList.Count; index++)
            {
                var point = PointList[index];
                // var worldPos = IsLocal ? tweenAnimation.transform.parent.localToWorldMatrix.MultiplyPoint(pos) : pos;

                var selected = _selectIndex == index;
                var position = point.Position;
                var eulerAngle = point.EulerAngle;
                var size = HandleUtility.GetHandleSize(position) * 0.08f;

                var dotIcon = EditorIcon.DotFill;
                var iconSize = new Vector2(dotIcon.width, dotIcon.height);
                var selectSize = iconSize * 1.5f;
                var dotSize = selected ? selectSize : iconSize;
                HandlesUtil.DrawTexture(position, dotIcon, dotSize);
                var btnSelect = Handles.Button(position, Quaternion.identity, size, size, SelectCapFunction);

                if (btnSelect)
                {
                    _selectIndex = index;
                }

                if (selected)
                {
                    if (_operationMode == PathOperationMode.Position)
                    {
                        var newPosition = Handles.DoPositionHandle(position, Quaternion.identity);
                        if (newPosition != position)
                        {
                            GUI.changed = true;
                            UndoOperate(() =>
                            {
                                point.Position = newPosition;
                                position = newPosition;
                            }, "Change Array Item");
                            // newPos = TweenAnimation.transform.parent.worldToLocalMatrix.MultiplyPoint(newPos);
                        }
                    }

                    if (_operationMode == PathOperationMode.Rotation)
                    {
                        var newEulerAngle = Handles.DoRotationHandle(Quaternion.Euler(eulerAngle), position).eulerAngles;
                        if (newEulerAngle != eulerAngle)
                        {
                            GUI.changed = true;
                            UndoOperate(() =>
                            {
                                point.EulerAngle = newEulerAngle;
                                eulerAngle = newEulerAngle;
                            }, "Change Array Item");
                            // newPos = TweenAnimation.transform.parent.worldToLocalMatrix.MultiplyPoint(newPos);
                        }
                    }
                }
            }
        }

        internal Quaternion QuaternionLookAtCamera => Quaternion.LookRotation(Camera.current.transform.forward, Camera.current.transform.up);

        internal void SelectCapFunction(int id, Vector3 position, Quaternion rotation, float size, EventType callbackType)
        {
            Handles.color = Color.clear;
            Handles.CircleHandleCap(id, position, QuaternionLookAtCamera, size, callbackType);
        }
    }
#endif
}