using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Property Bounds", "Property", "cs Script Icon")]
    [Serializable]
    public partial class TweenPropertyBounds : TweenValueBounds<Component>
    {
        public TweenPropertyData PropertyData = new TweenPropertyData();

        public override Bounds Value
        {
            get => PropertyData.GetValue<Bounds>(Target);
            set => PropertyData.SetValue(Target, value);
        }

        public override void Reset()
        {
            base.Reset();
            PropertyData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenPropertyBounds : TweenValueBounds<Component>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty PropertyDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            PropertyData.Tweener = this;
            PropertyData.PropertyType = typeof(Bounds);
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

    public partial class TweenPropertyBounds : TweenValueBounds<Component>
    {
        public TweenPropertyBounds SetProperty(string property)
        {
            PropertyData.Property = property;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenPropertyBounds Property(Component component, string propertyName, Bounds to, float duration)
        {
            var tweener = Create<TweenPropertyBounds>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyBounds;
            return tweener;
        }

        public static TweenPropertyBounds Property(Component component, string propertyName, Bounds from, Bounds to, float duration)
        {
            var tweener = Create<TweenPropertyBounds>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyBounds;
            return tweener;
        }
    }

    public static partial class ComponentExtension
    {
        public static TweenPropertyBounds TweenProperty(this Component component, string propertyName, Bounds to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, to, duration);
            return tweener;
        }

        public static TweenPropertyBounds TweenProperty(this Component component, string propertyName, Bounds from, Bounds to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
