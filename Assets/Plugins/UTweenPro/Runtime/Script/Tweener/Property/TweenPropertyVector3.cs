using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Property Vector3", "Property", "cs Script Icon")]
    [Serializable]
    public partial class TweenPropertyVector3 : TweenValueVector3<Component>
    {
        public TweenPropertyData PropertyData = new TweenPropertyData();

        public override Vector3 Value
        {
            get => PropertyData.GetValue<Vector3>(Target);
            set => PropertyData.SetValue(Target, value);
        }

        public override void Reset()
        {
            base.Reset();
            PropertyData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenPropertyVector3 : TweenValueVector3<Component>
    {
        [TweenerProperty(true), NonSerialized] public SerializedProperty PropertyDataProperty;

        public override void InitEditor(int index, TweenAnimation animation, SerializedProperty tweenerProperty)
        {
            PropertyData.Tweener = this;
            PropertyData.PropertyType = typeof(Vector3);
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

    public partial class TweenPropertyVector3 : TweenValueVector3<Component>
    {
        public TweenPropertyVector3 SetProperty(string property)
        {
            PropertyData.Property = property;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenPropertyVector3 Property(Component component, string propertyName, Vector3 to, float duration)
        {
            var tweener = Create<TweenPropertyVector3>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyVector3;
            return tweener;
        }

        public static TweenPropertyVector3 Property(Component component, string propertyName, Vector3 from, Vector3 to, float duration)
        {
            var tweener = Create<TweenPropertyVector3>()
                .SetTarget(component)
                .SetProperty(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenPropertyVector3;
            return tweener;
        }
    }

    public static partial class ComponentExtension
    {
        public static TweenPropertyVector3 TweenProperty(this Component component, string propertyName, Vector3 to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, to, duration);
            return tweener;
        }

        public static TweenPropertyVector3 TweenProperty(this Component component, string propertyName, Vector3 from, Vector3 to, float duration)
        {
            var tweener = UTween.Property(component, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
