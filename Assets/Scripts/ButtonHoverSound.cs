using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    [Header("Audio")]
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip buttonHover;

    //sound is played when you hover over button with this script attached
    public void OnPointerEnter(PointerEventData eventData) {
        audio.PlayOneShot(buttonHover);
    }
    
    
    
}
