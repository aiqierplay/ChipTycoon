using UnityEngine;

namespace Aya.Physical
{
    public interface IColliderListener2D
    {
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerStay2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);
    }
}