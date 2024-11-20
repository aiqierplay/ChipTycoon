/////////////////////////////////////////////////////////////////////////////
//
//  Script   : PhysicsUtil.cs
//  Info     : 通用物理工具类
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

namespace Aya.Physical
{
    public static class PhysicsUtil
    {
        #region Cache

        public static Ray RayCache = new Ray(Vector3.zero, Vector3.forward);

        public static int RaycastHitCacheSize = 1000;
        private static readonly RaycastHit[] RaycastHitCache = new RaycastHit[RaycastHitCacheSize];

        public static int ColliderCacheSize = 1000;
        private static readonly Collider[] ColliderCache = new Collider[ColliderCacheSize];

        #endregion

        #region Ray

        public static Ray MouseToWorldRay()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray;
        }

        #endregion

        #region Reflection

        public static List<Vector3> GetReflectionPath(Vector3 startPos, Vector3 direction, float distance, LayerMask reflexLayer)
        {
            return GetReflectionPath(startPos, direction, distance, reflexLayer, Vector3.zero);
        }

        public static List<Vector3> GetReflectionPath(Vector3 startPos, Vector3 direction, float distance, LayerMask reflexLayer, Vector3 offset)
        {
            var path = new List<Vector3>();
            var length = distance;
            path.Add(startPos + offset);

            do
            {
                RayCache.origin = startPos + direction * 0.01f;
                RayCache.direction = direction;
                var count = Physics.RaycastNonAlloc(RayCache, RaycastHitCache, length, reflexLayer.value);
                if (count > 0)
                {
                    var hitInfo = RaycastHitCache[0];
                    var point = hitInfo.point;
                    var dis = (point - startPos).magnitude;
                    if (dis < length)
                    {
                        path.Add(point + offset);
                        startPos = point;
                        direction = Vector3.Reflect(direction, hitInfo.normal).normalized;
                        length -= dis;
                    }
                    else
                    {
                        point = startPos + length * direction;
                        path.Add(point + offset);
                        break;
                    }
                }
                else
                {
                    var point = startPos + length * direction;
                    path.Add(point + offset);
                    break;
                }
            } while (length > 0);

            return path;
        }

        #endregion

        #region Raycast

        public static bool Raycast(Vector3 position, Vector3 direction, out RaycastHit hitInfo, float distance, LayerMask layer)
        {
            RayCache.origin = position;
            RayCache.direction = direction;
            return Raycast(RayCache, out hitInfo, distance, layer);
        }

        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance, LayerMask layer)
        {
            var count = Physics.RaycastNonAlloc(ray, RaycastHitCache, distance, layer.value);
            if (count > 0)
            {
                hitInfo = RaycastHitCache[0];
                return true;
            }
            else
            {
                hitInfo = default;
                return false;
            }
        }

        public static ValueTuple<T, Vector3> Raycast<T>(Vector3 position, Vector3 direction, float distance, LayerMask layerMask) where T : Component
        {
            RayCache.origin = position;
            RayCache.direction = direction;
            var count = Physics.RaycastNonAlloc(RayCache, RaycastHitCache, distance, layerMask.value);
            if (count <= 0) return default;
            RaycastHitCache.SortAsc(0, count, r => (r.point - position).sqrMagnitude);
            for (var i = 0; i < count && i < RaycastHitCacheSize; i++)
            {
                var raycastHit = RaycastHitCache[i];
                var forwardTarget = raycastHit.collider.gameObject.GetComponentInParent<T>();
                if (forwardTarget != null) return (forwardTarget, raycastHit.point);
            }

            return default;
        }

        public static List<ValueTuple<T, Vector3>> RaycastAll<T>(Vector3 position, Vector3 direction, float distance, LayerMask layerMask) where T : Component
        {
            var result = new List<ValueTuple<T, Vector3>>();
            RayCache.origin = position;
            RayCache.direction = direction;
            var count = Physics.RaycastNonAlloc(RayCache, RaycastHitCache, distance, layerMask.value);
            if (count <= 0) return result;
            RaycastHitCache.SortAsc(0, count, r => (r.point - position).sqrMagnitude);
            for (var i = 0; i < count && i < RaycastHitCacheSize; i++)
            {
                var raycastHit = RaycastHitCache[i];
                var component = raycastHit.collider.gameObject.GetComponentInParent<T>();
                if (component != null) result.Add((component, raycastHit.point));
            }

            return result;
        }

        #endregion

        #region OverlapSphere

        public static List<T> OverlapSphere<T>(Vector3 position, float radius, LayerMask layerMask) where T : Component
        {
            var result = new List<T>();
            var count = Physics.OverlapSphereNonAlloc(position, radius, ColliderCache, layerMask.value);
            if (count <= 0) return result;
            ColliderCache.SortAsc(0, count, c => (c.transform.position - position).sqrMagnitude);
            for (var i = 0; i < count && i < ColliderCacheSize; i++)
            {
                var collider = ColliderCache[i];
                var component = collider.gameObject.GetComponentInParent<T>();
                if (component == null) continue;
                result.Add(component);
            }

            return result;
        }

        public static T OverlapSphereNearest<T>(Vector3 position, float radius, LayerMask layerMask) where T : Component
        {
            var components = OverlapSphere<T>(position, radius, layerMask);
            if (components.Count == 0) return default;
            var result = components.Min(c => (c.transform.position - position).sqrMagnitude);
            return result;
        }

        public static List<T> OverlapSphereNearest<T>(Vector3 position, float radius, LayerMask layerMask, int count) where T : Component
        {
            var components = OverlapSphere<T>(position, radius, layerMask);
            if (components.Count == 0) return default;
            var result = components.Min(c => (c.transform.position - position).sqrMagnitude, count);
            return result;
        }

        public static T OverlapSphereFarthest<T>(Vector3 position, float radius, LayerMask layerMask) where T : Component
        {
            var components = OverlapSphere<T>(position, radius, layerMask);
            if (components.Count == 0) return default;
            var result = components.Max(c => (c.transform.position - position).sqrMagnitude);
            return result;
        }

        public static List<T> OverlapSphereFarthest<T>(Vector3 position, float radius, LayerMask layerMask, int count) where T : Component
        {
            var components = OverlapSphere<T>(position, radius, layerMask);
            if (components.Count == 0) return default;
            var result = components.Max(c => (c.transform.position - position).sqrMagnitude, count);
            return result;
        }

        #endregion

        #region OverlapBox

        public static List<T> OverlapBox<T>(Vector3 center, Vector3 halfExtents, Quaternion quaternion, LayerMask layerMask) where T : Component
        {
            var result = new List<T>();
            var count = Physics.OverlapBoxNonAlloc(center, halfExtents, ColliderCache, quaternion, layerMask.value);
            if (count <= 0) return result;
            ColliderCache.SortAsc(0, count, c => (c.transform.position - center).sqrMagnitude);
            for (var i = 0; i < count && i < ColliderCacheSize; i++)
            {
                var collider = ColliderCache[i];
                var component = collider.gameObject.GetComponentInParent<T>();
                if (component == null) continue;
                result.Add(component);
            }

            return result;
        }

        public static T OverlapBoxNearest<T>(Vector3 center, Vector3 halfExtents, Quaternion quaternion, LayerMask layerMask) where T : Component
        {
            var components = OverlapBox<T>(center, halfExtents, quaternion, layerMask);
            if (components.Count == 0) return default;
            var result = components.Min(c => (c.transform.position - center).sqrMagnitude);
            return result;
        }

        public static List<T> OverlapBoxNearest<T>(Vector3 center, Vector3 halfExtents, Quaternion quaternion, LayerMask layerMask, int count) where T : Component
        {
            var components = OverlapBox<T>(center, halfExtents, quaternion, layerMask);
            if (components.Count == 0) return default;
            var result = components.Min(c => (c.transform.position - center).sqrMagnitude, count);
            return result;
        }

        public static T OverlapBoxFarthest<T>(Vector3 center, Vector3 halfExtents, Quaternion quaternion, LayerMask layerMask) where T : Component
        {
            var components = OverlapBox<T>(center, halfExtents, quaternion, layerMask);
            if (components.Count == 0) return default;
            var result = components.Max(c => (c.transform.position - center).sqrMagnitude);
            return result;
        }

        public static List<T> OverlapBoxFarthest<T>(Vector3 center, Vector3 halfExtents, Quaternion quaternion, LayerMask layerMask, int count) where T : Component
        {
            var components = OverlapBox<T>(center, halfExtents, quaternion, layerMask);
            if (components.Count == 0) return default;
            var result = components.Max(c => (c.transform.position - center).sqrMagnitude, count);
            return result;
        }

        #endregion

        #region OverlapCapsule

        public static List<T> OverlapCapsule<T>(Vector3 point0, Vector3 point1, float radius, LayerMask layerMask) where T : Component
        {
            var result = new List<T>();
            var count = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, ColliderCache, layerMask.value);
            if (count <= 0) return result;
            var center = (point0 + point1) / 2f;
            ColliderCache.SortAsc(0, count, c => (c.transform.position - center).sqrMagnitude);
            for (var i = 0; i < count && i < ColliderCacheSize; i++)
            {
                var collider = ColliderCache[i];
                var component = collider.gameObject.GetComponentInParent<T>();
                if (component == null) continue;
                result.Add(component);
            }

            return result;
        }

        public static T OverlapCapsuleNearest<T>(Vector3 point0, Vector3 point1, float radius, LayerMask layerMask) where T : Component
        {
            var components = OverlapCapsule<T>(point0, point1, radius, layerMask);
            if (components.Count == 0) return default;
            var center = (point0 + point1) / 2f;
            var result = components.Min(c => (c.transform.position - center).sqrMagnitude);
            return result;
        }

        public static List<T> OverlapCapsuleNearest<T>(Vector3 point0, Vector3 point1, float radius, LayerMask layerMask, int count) where T : Component
        {
            var components = OverlapCapsule<T>(point0, point1, radius, layerMask);
            if (components.Count == 0) return default;
            var center = (point0 + point1) / 2f;
            var result = components.Min(c => (c.transform.position - center).sqrMagnitude, count);
            return result;
        }

        public static T OverlapCapsuleFarthest<T>(Vector3 point0, Vector3 point1, float radius, LayerMask layerMask) where T : Component
        {
            var components = OverlapCapsule<T>(point0, point1, radius, layerMask);
            if (components.Count == 0) return default;
            var center = (point0 + point1) / 2f;
            var result = components.Max(c => (c.transform.position - center).sqrMagnitude);
            return result;
        }

        public static List<T> OverlapCapsuleFarthest<T>(Vector3 point0, Vector3 point1, float radius, LayerMask layerMask, int count) where T : Component
        {
            var components = OverlapCapsule<T>(point0, point1, radius, layerMask);
            if (components.Count == 0) return default;
            var center = (point0 + point1) / 2f;
            var result = components.Max(c => (c.transform.position - center).sqrMagnitude, count);
            return result;
        }

        #endregion
    }
}
