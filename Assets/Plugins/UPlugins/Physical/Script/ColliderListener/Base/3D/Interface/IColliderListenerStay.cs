using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListenerStay
    {
        void OnTriggerStay(Collider other);
    }
}