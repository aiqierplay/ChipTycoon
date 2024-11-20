using System;
using UnityEngine;

namespace Aya.DragDrop
{
    [Serializable]
    public class TransformData
    {
        public static TransformData Zero = new TransformData();

        public Vector3 Position = Vector3.zero;
        public Vector3 EulerAngle = Vector3.zero;
        public Vector3 LocalScale = Vector3.zero;

        [NonSerialized] public Transform Transform;
        [NonSerialized] public Transform Parent;

        [NonSerialized] public int SiblingIndex;

        public TransformData()
        {
        }

        public TransformData(Transform transform)
        {
            CopyFrom(transform);
        }

        public void CopyFrom(Transform transform, bool trs = true, bool parent = true, bool siblingIndex = true)
        {
            Transform = transform;
            if (trs)
            {
                Position = transform.position;
                EulerAngle = transform.eulerAngles;
                LocalScale = transform.localScale;
            }

            if (parent)
            {
                Parent = transform.parent;
            }

            if (siblingIndex)
            {
                SiblingIndex = transform.GetSiblingIndex();
            }
        }

        public void CopyTo(Transform transform, bool trs = true, bool parent = false, bool siblingIndex = false)
        {
            if (parent)
            {
                transform.parent = Parent;
            }

            if (trs)
            {
                transform.position = Position;
                transform.eulerAngles = EulerAngle;
                transform.localScale = LocalScale;
            }

            if (siblingIndex)
            {
                transform.SetSiblingIndex(SiblingIndex);
            }
        }

        public static void Add(TransformData lhs, TransformData rhs, TransformData result)
        {
            result.Position = lhs.Position + rhs.Position;
            result.EulerAngle = lhs.EulerAngle + rhs.EulerAngle;
            result.LocalScale = lhs.LocalScale + rhs.LocalScale;
        }

        public static void Minus(TransformData lhs, TransformData rhs, TransformData result)
        {
            result.Position = lhs.Position - rhs.Position;
            result.EulerAngle = lhs.EulerAngle - rhs.EulerAngle;
            result.LocalScale = lhs.LocalScale - rhs.LocalScale;
        }

        public static void Lerp(LerpType type, TransformData from, TransformData to, float factor, TransformData result)
        {
            factor = Mathf.Clamp01(factor);
            var position = LerpUtil.Lerp(type, from.Position, to.Position, factor);
            result.Position = position;

            var eulerAngle = LerpUtil.Lerp(type, from.EulerAngle, to.EulerAngle, factor);
            result.EulerAngle = eulerAngle;

            var localScale = LerpUtil.Lerp(type, from.LocalScale, to.LocalScale, factor);
            result.LocalScale = localScale;
        }
    }
}