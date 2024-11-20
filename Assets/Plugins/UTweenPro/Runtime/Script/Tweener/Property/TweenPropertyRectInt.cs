using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Property RectInt", "Property", "cs Script Icon")]
    [Serializable]
    public partial class TweenPropertyRectInt : TweenValueRectInt<Component>
    {
        public TweenPropertyData PropertyData = new TweenPropertyData();

        public override RectInt Value
        {
            get => PropertyData.GetValue<RectInt>(Target);
            set => PropertyData.SetValue(Target, value);
        }

        public override void Reset()
        {
            base.Reset();
            PropertyData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenPropertyRectInt : TweenValueRectInt<Component>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty PropertyDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            PropertyData.Tweener = this;
            PropertyData.PropertyType = typeof(RectInt);
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

    public partial class TweenPropertyRectInt : TweenValueRectInt<Component>
    {
        public TweenPropertyRectInt SetProperty(string property)
        {
            PropertyData.Property = property;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenPropertyRectInt Property(Component component, string propertyName, RectInt to, float duration)
        {
            var tweener = Create<TweenPropertyRectInt>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyRectInt;
            return tweener;
        }

        public static TweenPropertyRectInt Property(Component component, string propertyName, RectInt from, RectInt to, float duration)
        {
            var tweener = Create<TweenPropertyRectInt>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyRectInt;
            return tweener;
        }
    }

    public static partial class ComponentExtension
    {
        public static TweenPropertyRectInt TweenProperty(this Component component, string propertyName, RectInt to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, to, duration);
            return tweener;
        }

        public static TweenPropertyRectInt TweenProperty(this Component component, string propertyName, RectInt from, RectInt to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
