using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListenerStay
    {
        void OnCollisionStay(Collision other);
    }
}