using System;
using UnityEngine;
using UnityEngine.Events;

namespace Aya.Physical
{
    [Serializable]
    public class Collision2DEvent : UnityEvent<Collision2D>
    {
    }
}