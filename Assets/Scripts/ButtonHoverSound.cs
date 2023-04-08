using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    private AudioSource audio;

    [SerializeField]
    private AudioClip buttonHover;

    void Start()
    {
        audio = GetComponentInParent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        audio.PlayOneShot(buttonHover);
    }
}
