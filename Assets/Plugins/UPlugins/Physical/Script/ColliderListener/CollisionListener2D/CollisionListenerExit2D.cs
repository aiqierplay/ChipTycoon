using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CollisionListenerExit2D : MonoBehaviour, ICollisionListenerExit2D
    {
        [NonSerialized] public Rigidbody2D Rigidbody2D;

        public ColliderCallback<Collision2D> onCollisionExit2D = new ColliderCallback<Collision2D>();

        public Collision2DEvent CollisionExit2DEvent;

        public void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public virtual void OnCollisionExit2D(Collision2D other)
        {
            onCollisionExit2D.Trigger(other);
            CollisionExit2DEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionExit2D.Clear();
            CollisionExit2DEvent?.RemoveAllListeners();
        }
    }
}