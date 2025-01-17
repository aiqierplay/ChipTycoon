﻿using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionListener : MonoBehaviour, ICollisionListener
    {
        [NonSerialized] public Rigidbody Rigidbody;

        public ColliderCallback<Collision> onCollisionEnter = new ColliderCallback<Collision>();
        public ColliderCallback<Collision> onCollisionStay = new ColliderCallback<Collision>();
        public ColliderCallback<Collision> onCollisionExit = new ColliderCallback<Collision>();

        public CollisionEvent CollisionEnterEvent;
        public CollisionEvent CollisionStayEvent;
        public CollisionEvent CollisionExitEvent;

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            onCollisionEnter.Trigger(other);
            CollisionEnterEvent?.Invoke(other);
        }

        public virtual void OnCollisionStay(Collision other)
        {
            onCollisionStay.Trigger(other);
            CollisionStayEvent?.Invoke(other);
        }

        public virtual void OnCollisionExit(Collision other)
        {
            onCollisionExit.Trigger(other);
            CollisionExitEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onCollisionEnter.Clear();
            onCollisionStay.Clear();
            onCollisionExit.Clear();

            CollisionEnterEvent?.RemoveAllListeners();
            CollisionStayEvent?.RemoveAllListeners();
            CollisionExitEvent?.RemoveAllListeners();
        }
    }
}