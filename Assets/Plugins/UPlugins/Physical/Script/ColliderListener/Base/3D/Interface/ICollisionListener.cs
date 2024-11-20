using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListener
    {
        void OnCollisionEnter(Collision other);
        void OnCollisionStay(Collision other);
        void OnCollisionExit(Collision other);
    }
}