using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListenerEnter
    {
        void OnCollisionEnter(Collision other);
    }
}