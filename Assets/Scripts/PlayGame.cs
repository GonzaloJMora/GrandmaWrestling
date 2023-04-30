using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayGame : MonoBehaviour
{    
    [Header("Transition Stuff")]
    [SerializeField]
    private GameObject lobbyStuff;

    [SerializeField]
    private GameObject gameStuff;

    [SerializeField]
    private GameObject loadingScreen;

    [Header("Player Manager")]
    [SerializeField]
    private GameObject playerManager;
    private PlayerManager manager;

    [Header("Chaos")]
    [SerializeField]
    private GameObject chaosSystem;

    [Header("Canvases")]
    [SerializeField]
    private GameObject lobbyCanvas;
    private TMP_Text readyPlayers, countdown;

    [SerializeField]
    private GameObject gameCanvas;

    //player currently ready
    private int playersReady = 0;

    [Header("Timers (in seconds)")]
    public float startTime = 3f;

    //current time (used for startTime)
    private float timer;

    private AudioSource audio;

    [Header("Audio")]
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

    //initialization
    private void Awake()
    {
        manager = playerManager.GetComponent<PlayerManager>();
    }

    void Start() {
        audio = GetComponent<AudioSource>();
        timer = startTime;
        readyPlayers = lobbyCanvas.transform.Find("ReadyPlayers").GetComponent<TMP_Text>();
        countdown = lobbyCanvas.transform.Find("Countdown").GetComponent<TMP_Text>();
    }

    void Update() {
        readyPlayers.text = "Players Ready: " + playersReady + "/" + manager.numPlayers;

        //if all players are currently ready
        if (manager.numPlayers > 1 && playersReady == manager.numPlayers) {
            countdown.gameObject.SetActive(true);
            int temp = (int) Mathf.Floor(timer);

            //startTime countdown
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
            //lobby to loading transition
            else {
                manager.isGameStarted = true;
                countdown.gameObject.SetActive(false);
                loadingScreen.SetActive(true);
                gameStuff.SetActive(true);
                chaosSystem.SetActive(true);
                lobbyCanvas.SetActive(false);
                lobbyStuff.SetActive(false);

                for(int i = 0; i < manager.numPlayers; i++) {
                    manager.players[i].transform.position = manager.startingPoints[i].position;
                    manager.players[i].gameObject.GetComponent<Controller>().ResetHits();
                    manager.players[i].gameObject.GetComponent<Controller>().ResetPhysics();
                }

                timer = startTime;
                playersReady = 0;

                Invoke("deactivateLoading", 0.5f);
            }
        }
    }

    //loading to game transition
    private void deactivateLoading() {
        loadingScreen.SetActive(false);
        gameCanvas.SetActive(true);
    }

    //player stands on red square to ready up
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            playersReady++;
            audio.PlayOneShot(playerReadyUp);
        }
    }

    //player leaves the red square
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            playersReady--;

            //if the countdown had already started
            if (countdown.gameObject.activeSelf) {
                timer = startTime;
                countdown.gameObject.SetActive(false);
                audio.PlayOneShot(timerInterrupted);
            }
            //if not everyone was readied up already
            else {
                audio.PlayOneShot(playerUnready);
            }
        }
    }
}
