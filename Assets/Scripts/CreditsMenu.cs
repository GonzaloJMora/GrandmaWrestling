using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    [Header("Transition Stuff")]
    [SerializeField]
    private GameObject creditsCanvas; 

    [SerializeField]
    private GameObject menuCanvas;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip pressButton;

    //transition from credits to main menu
    public void closeCredits() {
        audio.PlayOneShot(pressButton);
        menuCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
}
