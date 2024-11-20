using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListenerExit
    {
        void OnTriggerExit(Collider other);
    }
}