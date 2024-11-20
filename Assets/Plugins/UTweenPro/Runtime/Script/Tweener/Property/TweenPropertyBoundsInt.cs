using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Property BoundsInt", "Property", "cs Script Icon")]
    [Serializable]
    public partial class TweenPropertyBoundsInt : TweenValueBoundsInt<Component>
    {
        public TweenPropertyData PropertyData = new TweenPropertyData();

        public override BoundsInt Value
        {
            get => PropertyData.GetValue<BoundsInt>(Target);
            set => PropertyData.SetValue(Target, value);
        }

        public override void Reset()
        {
            base.Reset();
            PropertyData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenPropertyBoundsInt : TweenValueBoundsInt<Component>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty PropertyDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            PropertyData.Tweener = this;
            PropertyData.PropertyType = typeof(BoundsInt);
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

    public partial class TweenPropertyBoundsInt : TweenValueBoundsInt<Component>
    {
        public TweenPropertyBoundsInt SetProperty(string property)
        {
            PropertyData.Property = property;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenPropertyBoundsInt Property(Component component, string propertyName, BoundsInt to, float duration)
        {
            var tweener = Create<TweenPropertyBoundsInt>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyBoundsInt;
            return tweener;
        }

        public static TweenPropertyBoundsInt Property(Component component, string propertyName, BoundsInt from, BoundsInt to, float duration)
        {
            var tweener = Create<TweenPropertyBoundsInt>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyBoundsInt;
            return tweener;
        }
    }

    public static partial class ComponentExtension
    {
        public static TweenPropertyBoundsInt TweenProperty(this Component component, string propertyName, BoundsInt to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, to, duration);
            return tweener;
        }

        public static TweenPropertyBoundsInt TweenProperty(this Component component, string propertyName, BoundsInt from, BoundsInt to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
