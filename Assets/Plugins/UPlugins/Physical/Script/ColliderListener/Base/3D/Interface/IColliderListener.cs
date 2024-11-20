using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListener
    {
        void OnTriggerEnter(Collider other);
        void OnTriggerStay(Collider other);
        void OnTriggerExit(Collider other);
    }
}