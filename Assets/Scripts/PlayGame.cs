using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayGame : MonoBehaviour
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

    [SerializeField]
    private GameObject chaosSystem;

    [SerializeField]
    private GameObject lobbyCanvas;
    private TMP_Text readyPlayers, countdown;

    [SerializeField]
    private GameObject gameCanvas;

    private int playersReady = 0;

    public float startTime = 3f;
    private float timer;

    //initialization
    private void Awake()
    {
        manager = playerManager.GetComponent<PlayerManager>();
    }

    void Start() {
        timer = startTime;
        readyPlayers = lobbyCanvas.transform.Find("ReadyPlayers").GetComponent<TMP_Text>();
        countdown = lobbyCanvas.transform.Find("Countdown").GetComponent<TMP_Text>();
    }

    void Update() {
        readyPlayers.text = "Players Ready: " + playersReady + "/" + manager.numPlayers;

        if (manager.numPlayers > 0 && playersReady == manager.numPlayers) {
            countdown.gameObject.SetActive(true);

            if (timer > 0) {
                timer -= Time.deltaTime;

                if (timer < 0) {
                    timer = 0f;
                }

                countdown.text = "" + Mathf.Floor(timer);
            }
            else {
                countdown.gameObject.SetActive(false);
                loadingScreen.SetActive(true);
                gameStuff.SetActive(true);
                chaosSystem.SetActive(true);
                lobbyCanvas.SetActive(false);
                lobbyStuff.SetActive(false);
                
                for(int i = 0; i < manager.numPlayers; i++) {
                    manager.players[i].transform.position = manager.startingPoints[i].position;
                }

                Invoke("deactivateLoading", 0.5f);

                timer = startTime;
                playersReady = 0;
            }
        }
    }

    private void deactivateLoading() {
        loadingScreen.SetActive(false);
        gameCanvas.SetActive(true);
    }

    private void OnCollisionEnter(Collision other) {
        playersReady++;
    }

    private void OnCollisionExit(Collision other) {
        playersReady--;
        timer = startTime;
        countdown.gameObject.SetActive(false);
    }
}
