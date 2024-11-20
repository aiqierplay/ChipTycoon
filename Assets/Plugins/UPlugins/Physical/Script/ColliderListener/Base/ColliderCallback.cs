using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Physical
{
    public class ColliderCallback<TSource> where TSource : class
    {
        internal List<ColliderFilter<TSource>> Filters = new List<ColliderFilter<TSource>>();
        public int Count => Filters.Count;

        public ColliderFilter<TSource> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) return default;
                return Filters[index];
            }
        }

        #region Add

        public ColliderFilter<TSource> Add<TComponent>(Action<TComponent> callback) where TComponent : Component
        {
            return Add(callback, null, null);
        }

        public ColliderFilter<TSource> Add<TTarget>(Action<TTarget> callback, LayerMask? layerMask) where TTarget : Component
        {
            return Add(callback, null, layerMask);
        }

        public ColliderFilter<TSource> Add<TTarget>(Action<TTarget, TSource> callback) where TTarget : Component
        {
            return Add(null, callback, null);
        }

        public ColliderFilter<TSource> Add<TTarget>(Action<TTarget, TSource> callback, LayerMask? layerMask) where TTarget : Component
        {
            return Add(null, callback, layerMask);
        }

        public ColliderFilter<TSource> Add<TTarget>(Action<TTarget> componentCallback, Action<TTarget, TSource> componentSourceCallback, LayerMask? layerMask) where TTarget : Component
        {
            var componentType = typeof(TTarget);
            var filter = new ColliderFilter<TSource>
            {
                ComponentType = componentType,
                Layer = layerMask,
            };

            if (componentCallback != null)
            {
                filter.ComponentCallback = component => { componentCallback(component as TTarget); };
                filter.ComponentCallbackHashCode = componentCallback.GetHashCode();
            }

            if (componentSourceCallback != null)
            {
                filter.ComponentSourceCallback = (component, source) => { componentSourceCallback(component as TTarget, source as TSource); };
                filter.ComponentSourceCallbackHashCode = componentSourceCallback.GetHashCode();
            }

            Filters.Add(filter);
            return filter;
        }

        #endregion

        #region Remove & Clear
       
        public void Remove<TTarget>(Action<TTarget> callback) where TTarget : Component
        {
            for (var i = Filters.Count - 1; i >= 0; i--)
            {
                var filter = Filters[i];
                var hashCode = callback.GetHashCode();
                if (filter.ComponentCallbackHashCode == hashCode)
                {
                    Filters.Remove(filter);
                }
            }
        }

        public void Remove<TTarget>(Action<TTarget, TSource> callback) where TTarget : Component
        {
            for (var i = Filters.Count - 1; i >= 0; i--)
            {
                var filter = Filters[i];
                var hashCode = callback.GetHashCode();
                if (filter.ComponentSourceCallbackHashCode == hashCode)
                {
                    Filters.Remove(filter);
                }
            }
        }
        public void Clear()
        {
            Filters.Clear();
        }

        #endregion

        #region Trigger

        public void Trigger(TSource target)
        {
            if (target == null) return;
            var filterCount = Filters.Count;
            for (var i = 0; i < filterCount; i++)
            {
                var filter = Filters[i];
                if (target is Collider collider)
                {
                    TriggerInternal(collider, collider.gameObject, filter);
                }
                else if (target is Collision collision)
                {
                    TriggerInternal(collision, collision.gameObject, filter);
                }
                else if (target is Collider2D collider2D)
                {
                    TriggerInternal(collider2D, collider2D.gameObject, filter);
                }
                else if (target is Collision2D collision2D)
                {
                    TriggerInternal(collision2D, collision2D.gameObject, filter);
                }
            }
        }

        internal void TriggerInternal(object source, GameObject gameObject, ColliderFilter<TSource> filter)
        {
            var componentCallback = filter.ComponentCallback;
            var componentSourceCallback = filter.ComponentSourceCallback;
            if (filter.Layer == null)
            {
                var component = gameObject.GetComponentInParent(filter.ComponentType);
                componentCallback?.Invoke(component);
                componentSourceCallback?.Invoke(component, source);
            }
            else
            {
                var targetLayer = gameObject.layer;
                var filterLayer = filter.Layer.Value;
                if (((1 << targetLayer) & filterLayer.value) > 0)
                {
                    var component = gameObject.GetComponentInParent(filter.ComponentType);
                    componentCallback?.Invoke(component);
                    componentSourceCallback?.Invoke(component, source);
                }
            }
        }
    } 

    #endregion
}