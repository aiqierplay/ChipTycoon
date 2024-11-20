using UnityEngine;

namespace Aya.TweenPro
{
    [SerializeField]
    public class TweenPathCachePoint
    {
        public Vector3 Position;
        public Vector3 EulerAngle;
        public float Distance;
        public float Length;

        public static void Lerp(TweenPathCachePoint from, TweenPathCachePoint to, float factor, out Vector3 position, out Vector3 eulerAngle)
        {
            position = Vector3.Lerp(from.Position, to.Position, factor);
            eulerAngle = LerpUtil.LerpAngle(from.EulerAngle, to.EulerAngle, factor);
        }

        // public Vector3 EulerAngle => Rotation.eulerAngles;
        // public Quaternion Rotation => FromToRotation(Vector3.forward, Forward);

        // public static Quaternion FromToRotation(Vector3 v1, Vector3 v2)
        // {
        //     var a = Vector3.Cross(v1, v2);
        //     var w = Mathf.Sqrt(v1.sqrMagnitude * v2.sqrMagnitude) + Vector3.Dot(v1, v2);
        //     if (a.sqrMagnitude < Mathf.Epsilon)
        //     {
        //         return Mathf.Abs(w) < Mathf.Epsilon ? new Quaternion(0f, 1f, 0f, 0f) : Quaternion.identity;
        //     }
        //
        //     return new Quaternion(a.x, a.y, a.z, w).normalized;
        // }
    }
}
