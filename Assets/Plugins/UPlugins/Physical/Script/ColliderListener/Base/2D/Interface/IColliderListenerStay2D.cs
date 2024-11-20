using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListenerStay2D
    {
        void OnTriggerStay2D(Collider2D other);
    }
}