using UnityEngine;
using UnityEngine.EventSystems;

namespace Aya.Sample.Notify
{
    public class BtnWithRedPoint : MonoBehaviour, IPointerClickHandler
    {
        public RedPoint RedPoint;

        public void OnPointerClick(PointerEventData eventData)
        {
            RedPoint.SetNotify(false);
        }
    }
}