using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public partial class TweenPathPoint
    {
        public Vector3 Position;
        public Vector3 EulerAngle;
    }

#if UNITY_EDITOR
    public partial class TweenPathPoint
    {
        [TweenerProperty, NonSerialized] public SerializedProperty PositionProperty;
        [TweenerProperty, NonSerialized] public SerializedProperty EulerAngleProperty;
    }
#endif
}