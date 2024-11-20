using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListenerExit
    {
        void OnCollisionExit(Collision other);
    }
}