using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListenerStay2D
    {
        void OnCollisionStay2D(Collision2D other);
    }
}