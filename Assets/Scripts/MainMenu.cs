using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playerLobby;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip pressButton;

    [SerializeField]
    private AudioClip pressQuit;

    [SerializeField]
    private GameObject creditsCanvas; 

    [SerializeField]
    private GameObject menuCanvas; 

    private bool playGameFlag = false;

    public void startGame() {
        audio.PlayOneShot(pressButton);
        playGameFlag = true;
        Invoke("toLobby", 0.25f);
    }

    private void toLobby() {
        SceneManager.LoadScene(playerLobby);
    }

    public void openSettings() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressButton);
            Debug.Log("Not implemented yet.");
        }
    }

    public void closeSettings() {
        Debug.Log("You should not see this message cause it's not linked to anything.");
    }

    public void openCredits() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressButton);
            creditsCanvas.SetActive(true);
            menuCanvas.SetActive(false);
        }
    }

    public void quitGame() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressQuit);
            Debug.Log("This is implemented, it just does not do anything until we actually build the game.");
            Invoke("closeApp", 1);
        }
    }

    private void closeApp() {
        Debug.Log("App has quit");
        Application.Quit();
    }
}
