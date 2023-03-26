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
                
        for(int i = 0; i < manager.numPlayers; i++) {
            manager.players[i].transform.position = manager.startingPoints[i].position;
        }

        Invoke("deactivateLoading", 1);
    }

    private void deactivateLoading() {
        loadingScreen.SetActive(false);
    }
}
