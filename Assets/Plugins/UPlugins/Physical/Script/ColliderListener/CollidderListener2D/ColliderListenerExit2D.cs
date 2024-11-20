using System;
using UnityEngine;

namespace Aya.Physical
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderListenerExit2D : MonoBehaviour, IColliderListenerExit2D
    {
        [NonSerialized] public Collider2D Collider2D;

        public ColliderCallback<Collider2D> onTriggerExit2D = new ColliderCallback<Collider2D>();

        public Collider2DEvent TriggerExit2DEvent;

        public void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExit2D.Trigger(other);
            TriggerExit2DEvent?.Invoke(other);
        }

        public virtual void Clear()
        {
            onTriggerExit2D.Clear();
            TriggerExit2DEvent?.RemoveAllListeners();
        }
    }
}