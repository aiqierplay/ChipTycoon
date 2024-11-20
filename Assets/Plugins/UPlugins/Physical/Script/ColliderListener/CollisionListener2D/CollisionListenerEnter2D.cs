using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CollisionListenerEnter2D : MonoBehaviour, ICollisionListenerEnter2D
    {
        [NonSerialized] public Rigidbody2D Rigidbody2D;

        public ColliderCallback<Collision2D> onCollisionEnter2D = new ColliderCallback<Collision2D>();

        public Collision2DEvent CollisionEnter2DEvent;

        public void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public virtual void OnCollisionEnter2D(Collision2D other)
        {
            onCollisionEnter2D.Trigger(other);
            CollisionEnter2DEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionEnter2D.Clear();
            CollisionEnter2DEvent?.RemoveAllListeners();
        }
    }
}