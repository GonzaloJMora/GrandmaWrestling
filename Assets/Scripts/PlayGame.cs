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

    private AudioSource audio;

    [SerializeField]
    private AudioClip playerReadyUp;

    [SerializeField]
    private AudioClip playerUnready;

    [SerializeField]
    private AudioClip timerInterrupted;

    [SerializeField]
    private AudioClip timerThree;

    [SerializeField]
    private AudioClip timerTwo;

    [SerializeField]
    private AudioClip timerOne;

    [SerializeField]
    private AudioClip toGame;

    //initialization
    private void Awake()
    {
        manager = playerManager.GetComponent<PlayerManager>();
    }

    void Start() {
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(toGame);
        timer = startTime;
        readyPlayers = lobbyCanvas.transform.Find("ReadyPlayers").GetComponent<TMP_Text>();
        countdown = lobbyCanvas.transform.Find("Countdown").GetComponent<TMP_Text>();
    }

    void Update() {
        readyPlayers.text = "Players Ready: " + playersReady + "/" + manager.numPlayers;

        if (manager.numPlayers > 0 && playersReady == manager.numPlayers) {
            countdown.gameObject.SetActive(true);
            int temp = (int) Mathf.Floor(timer);

            if (timer > 0) {
                timer -= Time.deltaTime;

                if (timer < 0) {
                    timer = 0f;
                }

                int currTime = (int) Mathf.Floor(timer);

                countdown.text = "" + currTime;

                if (currTime < temp) {
                    if (currTime == 3) {
                        audio.PlayOneShot(timerThree);
                    }
                    else if (currTime == 2) {
                        audio.PlayOneShot(timerTwo);
                    }
                    else if (currTime == 1) {
                        audio.PlayOneShot(timerOne);
                    }
                }
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

                timer = startTime;
                playersReady = 0;

                Invoke("deactivateLoading", 0.5f);
            }
        }
    }

    private void deactivateLoading() {
        loadingScreen.SetActive(false);
        gameCanvas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            playersReady++;
            audio.PlayOneShot(playerReadyUp);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            playersReady--;

            if (countdown.gameObject.activeSelf) {
                timer = startTime;
                countdown.gameObject.SetActive(false);
                audio.PlayOneShot(timerInterrupted);
            }
            else {
                audio.PlayOneShot(playerUnready);
            }
        }
    }
}
