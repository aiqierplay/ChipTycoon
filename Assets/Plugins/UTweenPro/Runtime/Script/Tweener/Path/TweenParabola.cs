using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Path Parabola", "Path")]
    [Serializable]
    public partial class TweenParabola : TweenPathBase
    {
        public TargetPositionData Start = new TargetPositionData();
        public TargetPositionData End = new TargetPositionData();
        public float Height;

        public override Vector3 GetPositionByFactor(float factor)
        {
            return GetPositionByFactor(Start.GetPosition(), End.GetPosition(), Height, factor);
        }

        public static Vector3 GetPositionByFactor(Vector3 start, Vector3 end, float height, float factor)
        {
            return GetPositionByFactor(start, end, Vector3.up, height, factor);
        }

        public static Vector3 GetPositionByFactor(Vector3 start, Vector3 end, Vector3 direction, float height, float factor)
        {
            var a = height * 8f;
            var t = 0.5f - factor;
            var tempHeight = height - 0.5f * a * t * t;
            var result = Vector3.LerpUnclamped(start, end, factor);
            result += direction * tempHeight;
            return result;
        }

        public override void Reset()
        {
            base.Reset();
            Start.Reset();
            End.Reset();
            Height = 1f;
        }
    }

#if UNITY_EDITOR

    public partial class TweenParabola : TweenPathBase
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty StartProperty;
        [TweenerProperty(true), NonSerialized] public SerializedProperty EndProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty HeightProperty;

        public override void DrawFromToValue()
        {
            Start.DrawInspector();
            End.DrawInspector();
            EditorGUILayout.PropertyField(HeightProperty);
            base.DrawFromToValue();
        }
    }

#endif

    #region Extension

    public partial class UTween
    {
        public static TweenParabola Parabola(Transform transform, Vector3 from, Vector3 to, float height, float duration)
        {
            var tweener = Play<TweenParabola, Transform>(transform, duration);
            tweener.Start.Position = from;
            tweener.Start.Mode = TargetPositionMode.CustomValue;
            tweener.End.Position = to;
            tweener.End.Mode = TargetPositionMode.CustomValue;
            tweener.Height = height;
            return tweener;
        }

        public static TweenParabola Parabola(Transform transform, Transform from, Transform to, float height, float duration)
        {
            var tweener = Play<TweenParabola, Transform>(transform, duration);
            tweener.Start.Position = from.position;
            tweener.Start.Mode = TargetPositionMode.CustomValue;
            tweener.End.Position = to.position;
            tweener.End.Mode = TargetPositionMode.CustomValue;
            tweener.Height = height;
            return tweener;
        }
    }

    #endregion
}
