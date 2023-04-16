using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip buttonHover;

    public void OnPointerEnter(PointerEventData eventData) {
        audio.PlayOneShot(buttonHover);
    }
}
