using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListenerExit2D
    {
        void OnCollisionExit2D(Collision2D other);
    }
}