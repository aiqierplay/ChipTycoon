using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Property Vector2Int", "Property", "cs Script Icon")]
    [Serializable]
    public partial class TweenPropertyVector2Int : TweenValueVector2Int<Component>
    {
        public TweenPropertyData PropertyData = new TweenPropertyData();

        public override Vector2Int Value
        {
            get => PropertyData.GetValue<Vector2Int>(Target);
            set => PropertyData.SetValue(Target, value);
        }

        public override void Reset()
        {
            base.Reset();
            PropertyData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenPropertyVector2Int : TweenValueVector2Int<Component>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty PropertyDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            PropertyData.Tweener = this;
            PropertyData.PropertyType = typeof(Vector2Int);
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

    public partial class TweenPropertyVector2Int : TweenValueVector2Int<Component>
    {
        public TweenPropertyVector2Int SetProperty(string property)
        {
            PropertyData.Property = property;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenPropertyVector2Int Property(Component component, string propertyName, Vector2Int to, float duration)
        {
            var tweener = Create<TweenPropertyVector2Int>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyVector2Int;
            return tweener;
        }

        public static TweenPropertyVector2Int Property(Component component, string propertyName, Vector2Int from, Vector2Int to, float duration)
        {
            var tweener = Create<TweenPropertyVector2Int>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyVector2Int;
            return tweener;
        }
    }

    public static partial class ComponentExtension
    {
        public static TweenPropertyVector2Int TweenProperty(this Component component, string propertyName, Vector2Int to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, to, duration);
            return tweener;
        }

        public static TweenPropertyVector2Int TweenProperty(this Component component, string propertyName, Vector2Int from, Vector2Int to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
