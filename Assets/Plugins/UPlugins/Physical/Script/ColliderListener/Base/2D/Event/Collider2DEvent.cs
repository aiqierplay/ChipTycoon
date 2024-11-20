using System;
using UnityEngine;
using UnityEngine.Events;

namespace Aya.Physical
{
    [Serializable]
    public class Collider2DEvent : UnityEvent<Collider2D>
    {
    }
}