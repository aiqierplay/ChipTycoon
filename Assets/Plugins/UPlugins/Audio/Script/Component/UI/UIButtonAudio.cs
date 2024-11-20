using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Aya.Media.Audio
{
    [AddComponentMenu("Audio/UI Button Audio")]
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [NonSerialized] public Button Button;

        public AudioGroupType Group = AudioGroupType.SE;
        public AudioClip Click;
        public AudioClip Enter;
        public AudioClip Exit;
        public AudioClip Disable;

        public void Awake()
        {
            Button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Enter == null) return;
            Audio.PlayOneShot(Group, Enter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Exit == null) return;
            Audio.PlayOneShot(Group, Exit);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Button.interactable)
            {
                if (Click == null) return;
                Audio.PlayOneShot(Group, Click);
            }
            else
            {
                if (Disable == null) return;
                Audio.PlayOneShot(Group, Disable);
            }
        }
    }
}