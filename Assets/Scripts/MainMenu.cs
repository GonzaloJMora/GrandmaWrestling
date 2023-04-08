using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playerLobby;

    private AudioSource audio;

    [SerializeField]
    private AudioClip pressPlay;

    [SerializeField]
    private AudioClip pressSettings;

    [SerializeField]
    private AudioClip pressQuit; 

    private bool playGameFlag = false;

    void Start() {
        audio = GetComponent<AudioSource>();
    }

    public void startGame() {
        audio.PlayOneShot(pressPlay);
        playGameFlag = true;
        Invoke("toLobby", 2.5f);
    }

    private void toLobby() {
        SceneManager.LoadScene(playerLobby);
    }

    public void openSettings() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressSettings);
            Debug.Log("Not implemented yet.");
        }
    }

    public void closeSettings() {
        Debug.Log("You should not see this message cause it's not linked to anything.");
    }

    public void openCredits() {
        if (!playGameFlag) {
            audio.PlayOneShot(pressSettings);
            Debug.Log("Not implemented yet.");
        }
    }

    public void closeCredits() {
        Debug.Log("You should not see this message cause it's not linked to anything.");
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
