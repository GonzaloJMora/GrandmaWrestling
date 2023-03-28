using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLobby : MonoBehaviour
{
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

    [SerializeField]
    private GameObject playerManager;
    private PlayerManager manager;

    //initialization
    private void Awake()
    {
        manager = playerManager.GetComponent<PlayerManager>();
    }

    public void toLobby() {
        loadingScreen.SetActive(true);
        lobbyStuff.SetActive(true);
        gameStuff.SetActive(false);

        manager.RemoveAllPlayers();

        Invoke("deactivateLoading", 0.5f);
    }

    private void deactivateLoading() {
        loadingScreen.SetActive(false);
        lobbyCanvas.SetActive(true);
    }
}
