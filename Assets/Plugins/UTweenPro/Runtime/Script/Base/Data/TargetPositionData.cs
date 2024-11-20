using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public enum TargetPositionMode
    {
        TransformPosition = 0,
        CustomValue = 1,
    }

    [Serializable]
    public partial class TargetPositionData
    {
        public TargetPositionMode Mode;
        public Transform Transform;
        public Vector3 Position;

        public Vector3 GetPosition()
        {
            if (Mode == TargetPositionMode.TransformPosition)
            {
                if (Transform == null) return default;
                return Transform.position;
            }

            if (Mode == TargetPositionMode.CustomValue) return Position;
            return default;
        }

        public static implicit operator TargetPositionData(Vector3 value)
        {
            var positionData = new TargetPositionData {Position = value};
            return positionData;
        }

        public static implicit operator Vector3(TargetPositionData data)
        {
            return data.GetPosition();
        }

        public void Reset()
        {
            Mode = TargetPositionMode.TransformPosition;
            Transform = null;
            Position = Vector3.zero;
        }
    }

    #region Extension

    public partial class TargetPositionData
    {
        public TargetPositionData SetPosition(Vector3 position)
        {
            Mode = TargetPositionMode.CustomValue;
            Position = position;
            return this;
        }

        public TargetPositionData SetTarget(Transform transform)
        {
            Mode = TargetPositionMode.TransformPosition;
            Transform = transform;
            return this;
        }
    }

    #endregion

#if UNITY_EDITOR

    public partial class TargetPositionData : ITweenerSubData
    {
        public SerializedProperty DataProperty { get; set; }

        [TweenerProperty, NonSerialized] public SerializedProperty ModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty TransformProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty PositionProperty;

        public void DrawInspector()
        {
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(DataProperty.displayName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));

                if (Mode == TargetPositionMode.TransformPosition)
                {
                    using (GUIErrorColorArea.Create(Transform == null))
                    {
                        EditorGUILayout.ObjectField(TransformProperty, GUIContent.none);
                    }
                }
                else if (Mode == TargetPositionMode.CustomValue)
                {
                    PositionProperty.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, PositionProperty.vector3Value);
                }

                GUIUtil.DrawSelectEnumButton(ModeProperty, typeof(TargetPositionMode));
            }

            if (Mode == TargetPositionMode.TransformPosition && Transform != null)
            {
                Transform.position = EditorGUILayout.Vector3Field(nameof(Position), Transform.position);
            }
        }
    }

#endif
}
