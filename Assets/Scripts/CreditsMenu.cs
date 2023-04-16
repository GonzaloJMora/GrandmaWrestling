using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsCanvas; 

    [SerializeField]
    private GameObject menuCanvas;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip pressButton;

    public void closeCredits() {
        audio.PlayOneShot(pressButton);
        menuCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
}
