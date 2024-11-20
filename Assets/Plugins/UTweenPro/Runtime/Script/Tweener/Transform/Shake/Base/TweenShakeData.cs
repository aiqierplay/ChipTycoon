using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenShakeData
    {
        public Vector3Value Position = new Vector3Value();
        public Vector3Value Rotation = new Vector3Value();
        public Vector3Value Scale = new Vector3Value();
        public int Count;
        public AnimationCurve Curve;

        public void Reset()
        {
            Position.Reset();
            Position.SetValue(Vector3.one);
            Rotation.Reset();
            Scale.Reset();
            Count = 5;
            var defaultSlope = 5f;
            Curve = new AnimationCurve(
                new Keyframe(0f, 0f, defaultSlope, defaultSlope), 
                new Keyframe(0.25f, 1f, 0f, 0f), 
                new Keyframe(0.5f, 0f, -defaultSlope, -defaultSlope),
                new Keyframe(0.75f, -1f, 0f, 0f),
                new Keyframe(1f, 0f, defaultSlope, defaultSlope));
        }
    }

#if UNITY_EDITOR

    public partial class TweenShakeData : ITweenerSubData
    {
        public SerializedProperty DataProperty { get; set; }

        [NonSerialized] public TweenShake Tweener;

        [TweenerProperty(true), NonSerialized] public SerializedProperty PositionProperty;
        [TweenerProperty(true), NonSerialized] public SerializedProperty RotationProperty;
        [TweenerProperty(true), NonSerialized] public SerializedProperty ScaleProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CountProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty CurveProperty;

        public void DrawInspector()
        {
            var labelWidth = Tweener.EnableAxis ? EditorGUIUtility.labelWidth - EditorStyle.CharacterWidth : EditorGUIUtility.labelWidth;
            using (GUILabelWidthArea.Create(labelWidth))
            {
                using (GUIHorizontal.Create())
                {
                    if (Tweener.EnableAxis)
                    {
                        Tweener.AxisX = GUILayout.Toggle(Tweener.AxisX, "", GUILayout.Width(EditorStyle.CharacterWidth));
                    }
                    
                    using (GUIEnableArea.Create(Tweener.AxisX))
                    {
                        Position.AxisX = Position.AxisY = Position.AxisZ = Tweener.AxisX;
                        Position.AxisXName = "X";
                        Position.AxisYName = "Y";
                        Position.AxisZName = "Z";
                        Position.DrawInspector();
                    }
                }

                if (Tweener.AxisX && Position.Mode == TweenValueMode.Current)
                {
                    GUIUtil.DrawTipArea(UTweenEditorSetting.Ins.ErrorColor, "Not support [Current] mode");
                }

                using (GUIHorizontal.Create())
                {
                    if (Tweener.EnableAxis)
                    {
                        Tweener.AxisY = GUILayout.Toggle(Tweener.AxisY, "", GUILayout.Width(EditorStyle.CharacterWidth));
                    }

                    using (GUIEnableArea.Create(Tweener.AxisY))
                    {
                        Rotation.AxisX = Rotation.AxisY = Rotation.AxisZ = Tweener.AxisY;
                        Rotation.AxisXName = "X";
                        Rotation.AxisYName = "Y";
                        Rotation.AxisZName = "Z";
                        Rotation.DrawInspector();
                    }
                }

                if (Tweener.AxisY && Rotation.Mode == TweenValueMode.Current)
                {
                    GUIUtil.DrawTipArea(UTweenEditorSetting.Ins.ErrorColor, "Not support [Current] mode");
                }

                using (GUIHorizontal.Create())
                {
                    if (Tweener.EnableAxis)
                    {
                        Tweener.AxisZ = GUILayout.Toggle(Tweener.AxisZ, "", GUILayout.Width(EditorStyle.CharacterWidth));
                    }

                    using (GUIEnableArea.Create(Tweener.AxisZ))
                    {
                        Scale.AxisX = Scale.AxisY = Scale.AxisZ = Tweener.AxisZ;
                        Scale.AxisXName = "X";
                        Scale.AxisYName = "Y";
                        Scale.AxisZName = "Z";
                        Scale.DrawInspector();
                    }
                }

                if (Tweener.AxisZ && Scale.Mode == TweenValueMode.Current)
                {
                    GUIUtil.DrawTipArea(UTweenEditorSetting.Ins.ErrorColor, "Not support [Current] mode");
                }
            }

            using (GUIHorizontal.Create())
            {
                EditorGUILayout.PropertyField(CountProperty);
                EditorGUILayout.PropertyField(CurveProperty, new GUIContent("Shake"));
            }
        }
    }

#endif
}