using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLobby : MonoBehaviour
{
    [Header("Transition Stuff")]
    [SerializeField]
    private GameObject lobbyStuff;

    [SerializeField]
    private GameObject gameStuff;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private GameObject lobbyCanvas;

    [SerializeField]
    private GameObject gameCanvas;

    [Header("Player Manager")]
    //used to reset player count to 0
    [SerializeField]
    private PlayerManager manager;

    //game to loading screen transition
    public void toLobby() {
        loadingScreen.SetActive(true);
        lobbyStuff.SetActive(true);
        gameStuff.SetActive(false);

        manager.RemoveAllPlayers();

        Invoke("deactivateLoading", 0.5f);
    }

    //loading screen to lobby transition
    private void deactivateLoading() {
        loadingScreen.SetActive(false);
        lobbyCanvas.SetActive(true);
        manager.isGameStarted = false;
    }
}
