using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListenerEnter
    {
        void OnTriggerEnter(Collider other);
    }
}