using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.DragDrop
{
    public static class DragDropUtil 
    {
        #region UI
       
        private static readonly Vector3[] RectCornerArray = new Vector3[4];

        public static bool CheckInRect(RectTransform srcRect, RectTransform dstRect)
        {
            if (srcRect == null || dstRect == null) return false;
            srcRect.GetWorldCorners(RectCornerArray);
            var srcLeftTop = RectCornerArray[1];
            var srcRightBottom = RectCornerArray[3];
            var srcCenter = srcLeftTop + (srcRightBottom - srcLeftTop) / 2f;

            dstRect.GetWorldCorners(RectCornerArray);
            var dstLeftTop = RectCornerArray[1];
            var dstRightBottom = RectCornerArray[3];

            var c1 = srcCenter.x >= dstLeftTop.x && srcCenter.x <= dstRightBottom.x;
            var c2 = srcCenter.y <= dstLeftTop.y && srcCenter.y >= dstRightBottom.y;
            var result = c1 && c2;

            return result;
        } 

        #endregion

        #region Math

        public static bool LineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
        {
            var vecA = p2 - p1;
            var vecB = p4 - p3;
            var t = ((p3.x - p1.x) * vecB.y - (p3.y - p1.y) * vecB.x) / (vecA.x * vecB.y - vecA.y * vecB.x);
            var s = ((p1.x - p3.x) * vecA.y - (p1.y - p3.y) * vecA.x) / (vecB.x * vecA.y - vecB.y * vecA.x);
            if (t >= 0 && t <= 1 && s >= 0 && s <= 1)
            {
                intersection = p1 + t * vecA;
                return true;
            }
            else
            {
                intersection = Vector2.zero;
                return false;
            }
        }

        public static bool LinePlaneIntersection(Vector3 linePoint, Vector3 lineVector, Vector3 planePoint,
            Vector3 planeNormal, out Vector3 intersection)
        {
            intersection = Vector3.zero;
            var num = Vector3.Dot(planePoint - linePoint, planeNormal);
            var num2 = Vector3.Dot(lineVector, planeNormal);
            if (Math.Abs(num2) > 1e-6f)
            {
                var size = num / num2;
                var vector = SetLength(lineVector, size);
                intersection = linePoint + vector;
                return true;
            }

            return false;
        }

        public static Vector3 SetLength(Vector3 vector, float length)
        {
            var normalize = Vector3.Normalize(vector);
            var result = normalize * length;
            return result;
        }

        #endregion

        #region Physics

        public static Ray RayCache = new Ray(Vector3.zero, Vector3.forward);

        public static int RaycastHitCacheSize = 100;
        private static readonly RaycastHit[] RaycastHitCache = new RaycastHit[RaycastHitCacheSize];

        public class RaycastHitDistanceComparer : IComparer<RaycastHit>
        {
            [NonSerialized] public Vector3 Position;

            public RaycastHitDistanceComparer(Vector3 position)
            {
                Position = position;
            }

            public int Compare(RaycastHit r1, RaycastHit r2)
            {
                var d1 = (r1.point - Position).sqrMagnitude;
                var d2 = (r2.point - Position).sqrMagnitude;
                return d1.CompareTo(d2);
            }
        }

        public static ValueTuple<T, Vector3> Raycast<T>(Vector3 position, Vector3 direction, float distance, LayerMask layerMask) where T : Component
        {
            RayCache.origin = position;
            RayCache.direction = direction;
            var count = Physics.RaycastNonAlloc(RayCache, RaycastHitCache, distance, layerMask.value);
            if (count <= 0) return default;
            Array.Sort(RaycastHitCache, 0, count, new RaycastHitDistanceComparer(position));
            for (var i = 0; i < count && i < RaycastHitCacheSize; i++)
            {
                var raycastHit = RaycastHitCache[i];
                var forwardTarget = raycastHit.collider.gameObject.GetComponentInParent<T>();
                if (forwardTarget != null) return (forwardTarget, raycastHit.point);
            }

            return default;
        }

        #endregion

        #region Debug

        internal static void DebugDrawRect(RectTransform rect)
        {
            var corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            var leftBottomPos = corners[0];
            var leftTopPos = corners[1];
            var rightTopPos = corners[2];
            var rightBottomPos = corners[3];
            var centerPos = leftTopPos + (rightBottomPos - leftTopPos) / 2f;
            Debug.DrawLine(leftTopPos, rightTopPos, Color.red);
            Debug.DrawLine(rightTopPos, rightBottomPos, Color.red);
            Debug.DrawLine(leftTopPos, leftBottomPos, Color.red);
            Debug.DrawLine(leftBottomPos, rightBottomPos, Color.red);

            Debug.DrawLine(centerPos, leftBottomPos, Color.red);
            Debug.DrawLine(centerPos, leftTopPos, Color.red);
            Debug.DrawLine(centerPos, rightTopPos, Color.red);
            Debug.DrawLine(centerPos, rightBottomPos, Color.red);
        }

        #endregion
    }
}