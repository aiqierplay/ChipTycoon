using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Property Vector3Int", "Property", "cs Script Icon")]
    [Serializable]
    public partial class TweenPropertyVector3Int : TweenValueVector3Int<Component>
    {
        public TweenPropertyData PropertyData = new TweenPropertyData();

        public override Vector3Int Value
        {
            get => PropertyData.GetValue<Vector3Int>(Target);
            set => PropertyData.SetValue(Target, value);
        }

        public override void Reset()
        {
            base.Reset();
            PropertyData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenPropertyVector3Int : TweenValueVector3Int<Component>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty PropertyDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            PropertyData.Tweener = this;
            PropertyData.PropertyType = typeof(Vector3Int);
            base.InitEditor(index, animation, tweenerProperty);
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            PropertyData.DrawInspector();
        }
    }

#endif

    #region Extension

    public partial class TweenPropertyVector3Int : TweenValueVector3Int<Component>
    {
        public TweenPropertyVector3Int SetProperty(string property)
        {
            PropertyData.Property = property;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenPropertyVector3Int Property(Component component, string propertyName, Vector3Int to, float duration)
        {
            var tweener = Create<TweenPropertyVector3Int>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyVector3Int;
            return tweener;
        }

        public static TweenPropertyVector3Int Property(Component component, string propertyName, Vector3Int from, Vector3Int to, float duration)
        {
            var tweener = Create<TweenPropertyVector3Int>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyVector3Int;
            return tweener;
        }
    }

    public static partial class ComponentExtension
    {
        public static TweenPropertyVector3Int TweenProperty(this Component component, string propertyName, Vector3Int to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, to, duration);
            return tweener;
        }

        public static TweenPropertyVector3Int TweenProperty(this Component component, string propertyName, Vector3Int from, Vector3Int to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
