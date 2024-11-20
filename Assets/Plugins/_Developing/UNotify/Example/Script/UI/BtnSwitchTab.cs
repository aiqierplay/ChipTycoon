using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Aya.Sample.Notify
{
    public class BtnSwitchTab : MonoBehaviour, IPointerClickHandler
    {
        public GameObject Open;
        public List<GameObject> Close;

        public void OnPointerClick(PointerEventData eventData)
        {
            Open.SetActive(true);
            foreach (var target in Close)
            {
                target.SetActive(false);
            }
        }
    }
}