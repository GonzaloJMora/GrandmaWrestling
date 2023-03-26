using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playerLobby;

    public void startGame() {
        SceneManager.LoadScene(playerLobby);
    }

    public void openSettings() {
        Debug.Log("Not implemented yet.");
    }

    public void closeSettings() {
        Debug.Log("You should not see this message cause it's not linked to anything.");
    }

    public void quitGame() {
        Debug.Log("This is implemented, it just does not do anything until we actually build the game.");
        Application.Quit();
    }
}
