using UnityEngine;

namespace Aya.Physical
{
    public interface ICollisionListenerEnter2D
    {
        void OnCollisionEnter2D(Collision2D other);
    }
}