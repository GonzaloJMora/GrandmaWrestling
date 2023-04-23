using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Game Scene")]
    //game scene
    public string playerLobby;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip pressButton;

    [SerializeField]
    private AudioClip pressQuit;

    [Header("Transition Stuff")]
    [SerializeField]
    private GameObject creditsCanvas; 

    [SerializeField]
    private GameObject controlsCanvas; 

    [SerializeField]
    private GameObject menuCanvas; 

    //used to not allow any other buttons to be pressed when play button is clicked
    private bool playGameFlag = false;

    //play sound when play button is pressed
    public void startGame() {
        audio.PlayOneShot(pressButton);
        playGameFlag = true;
        Invoke("toLobby", 0.25f);
    }

    //transition from main menu to lobby
    private void toLobby() {
        SceneManager.LoadScene(playerLobby);
    }

    //transition from main menu to controls
    public void openControls() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressButton);
            controlsCanvas.SetActive(true);
            menuCanvas.SetActive(false);
        }
    }

    //transition from main menu to credits
    public void openCredits() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressButton);
            creditsCanvas.SetActive(true);
            menuCanvas.SetActive(false);
        }
    }

    //plays sound when quit button is pressed
    public void quitGame() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressQuit);
            Debug.Log("This is implemented, it just does not do anything until we actually build the game.");
            Invoke("closeApp", 1);
        }
    }

    //app is closed
    private void closeApp() {
        Debug.Log("App has quit");
        Application.Quit();
    }
}
