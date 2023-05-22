using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [Header("Transition Stuff")]
    [SerializeField]
    private GameObject subCanvas; 

    [SerializeField]
    private GameObject mainCanvas;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip pressButton;

    //transition from sub menu to main menu
    public void closeSubMenu() {
        audio.PlayOneShot(pressButton);
        mainCanvas.SetActive(true);
        subCanvas.SetActive(false);
    }
}
