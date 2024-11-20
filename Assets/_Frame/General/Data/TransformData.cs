using System;
using UnityEngine;

[Serializable]
public class TransformData
{
    public Transform Transform;

    public Vector3 Position;
    public Vector3 EulerAngles;

    public Vector3 LocalPosition;
    public Vector3 LocalEulerAngles;
    public Vector3 LocalScale;

    public static TransformData Create(Transform transform)
    {
        return new TransformData(transform);
    }

    public TransformData(Transform transform)
    {
        CopyForm(transform);
    }

    public void CopyForm(Transform transform)
    {
        Transform = transform;

        Position = transform.position;
        EulerAngles = transform.eulerAngles;

        LocalPosition = transform.localPosition;
        LocalEulerAngles = transform.localEulerAngles;
        LocalScale = transform.localScale;
    }

    public void CopyTo(Transform transform, bool isWorld = true)
    {
        if (isWorld)
        {
            transform.position = Position;
            transform.eulerAngles = EulerAngles;
        }
        else
        {
            transform.localPosition = LocalPosition;
            transform.localEulerAngles = LocalEulerAngles;
        }

        transform.localScale = LocalScale;
    }

    public bool IsDifferent(Transform transform, bool isWorld = true)
    {
        if (isWorld)
        {
            if (transform.position != Position) return true;
            if (transform.eulerAngles != EulerAngles) return true;
        }
        else
        {
            if (transform.localPosition != LocalPosition) return true;
            if (transform.eulerAngles != EulerAngles) return true;
        }

        if (transform.localScale != LocalScale) return false;
        return false;
    }

    public bool IsDifferent(bool isWorld = true)
    {
        return IsDifferent(Transform, isWorld);
    }
}