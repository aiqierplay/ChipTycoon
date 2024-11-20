using System;
using UnityEngine;

[Serializable]
public abstract class CustomTypeReference<T> where T : MonoBehaviour
{
    public T Value;

    public bool IsEmpty => Value == null;

    protected CustomTypeReference(T value)
    {
        Value = value;
    }

    #region Override Operator

    public static implicit operator T(CustomTypeReference<T> customTypeReference) => customTypeReference.Value;


    #endregion
}