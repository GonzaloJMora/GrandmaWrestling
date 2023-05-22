using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    [Header("Main Menu Scene")]
    public string mainMenu;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip pressButton;

    //play sound when button is pressed
    public void returnToMenu() {
        audio.PlayOneShot(pressButton);
        Invoke("toMain", 0.25f);
    }

    //transition from lobby to main menu
    private void toMain() {
        SceneManager.LoadScene(mainMenu);
    }
}
