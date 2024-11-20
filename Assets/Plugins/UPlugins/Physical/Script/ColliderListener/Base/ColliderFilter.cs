using System;
using UnityEngine;

namespace Aya.Physical
{
    public struct ColliderFilter<T>
    {
        public LayerMask? Layer;
        public Type ComponentType;

        public Action<object> ComponentCallback;
        internal int ComponentCallbackHashCode;

        public Action<object, object> ComponentSourceCallback;
        public int ComponentSourceCallbackHashCode;

        public ColliderEvent Event;
    }
}