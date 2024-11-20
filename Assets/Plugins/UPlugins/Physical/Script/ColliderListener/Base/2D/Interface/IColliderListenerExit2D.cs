using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListenerExit2D
    {
        void OnTriggerExit2D(Collider2D other);
    }
}