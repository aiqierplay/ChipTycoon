using System;
using UnityEngine;
using UnityEngine.Events;

namespace Aya.Physical
{
    [Serializable]
    public class ColliderEvent : UnityEvent<Collider>
    {
    }
}