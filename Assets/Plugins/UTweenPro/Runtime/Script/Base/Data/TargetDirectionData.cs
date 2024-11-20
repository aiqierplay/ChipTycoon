using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    public enum TargetDirectionMode
    {
        [InspectorName("Transform Forward")]
        TransformForward = 0,
        [InspectorName("Custom Value")]
        CustomValue = 1,
    }

    [Serializable]
    public partial class TargetDirectionData
    {
        public TargetDirectionMode Mode;
        public Transform Transform;
        public Vector3 Direction;

        public Vector3 GetDirection()
        {
            if (Mode == TargetDirectionMode.TransformForward)
            {
                if (Transform == null) return Vector3.forward;
                return Transform.forward;
            }

            if (Mode == TargetDirectionMode.CustomValue) return Direction;
            return Vector3.forward;
        }

        public static implicit operator TargetDirectionData(Vector3 value)
        {
            var positionData = new TargetDirectionData { Direction = value };
            return positionData;
        }

        public static implicit operator Vector3(TargetDirectionData data)
        {
            return data.GetDirection();
        }

        public void Reset()
        {
            Mode = TargetDirectionMode.TransformForward;
            Transform = null;
            Direction = Vector3.forward;
        }
    }

#if UNITY_EDITOR

    public partial class TargetDirectionData : ITweenerSubData
    {
        public SerializedProperty DataProperty { get; set; }

        [TweenerProperty, NonSerialized] public SerializedProperty ModeProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty TransformProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty DirectionProperty;

        public void DrawInspector()
        {
            using (GUIHorizontal.Create())
            {
                GUILayout.Label(DataProperty.displayName, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));

                if (Mode == TargetDirectionMode.TransformForward)
                {
                    using (GUIErrorColorArea.Create(Transform == null))
                    {
                        EditorGUILayout.ObjectField(TransformProperty, GUIContent.none);
                    }
                }
                else if (Mode == TargetDirectionMode.CustomValue)
                {
                    DirectionProperty.vector3Value = EditorGUILayout.Vector3Field(GUIContent.none, DirectionProperty.vector3Value);
                    DirectionProperty.vector3Value = DirectionProperty.vector3Value.normalized;
                }

                GUIUtil.DrawSelectEnumButton(ModeProperty, typeof(TargetDirectionMode));
            }

            if (Mode == TargetDirectionMode.TransformForward && Transform != null)
            {
                Transform.forward = EditorGUILayout.Vector3Field("Forward", Transform.forward);
            }
        }
    }

#endif
}
