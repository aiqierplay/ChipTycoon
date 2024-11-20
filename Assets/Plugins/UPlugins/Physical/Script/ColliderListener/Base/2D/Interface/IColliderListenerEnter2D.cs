using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListenerEnter2D
    {
        void OnTriggerEnter2D(Collider2D other);
    }
}