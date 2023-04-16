using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class elimGameMode : MonoBehaviour
{
    [Header("Game Length (in seconds)")]
    public float gameLen = 60;

    //current time remaining for game
    private float timeRemaining;

    //reset timer
    private float reset = 5;

    [Header("Game Canvas")]
    [SerializeField]
    private GameObject gameModeCanvas, win;
    private TMP_Text timer, p1, p2, p3, p4, winner;

    [Header("Player Manager")]
    [SerializeField]
    private GameObject playerManager;
    private PlayerManager manager;

    //reference to game to lobby transition script
    private BackToLobby lobbyWarp;

    [Header("Chaos")]
    [SerializeField]
    private GameObject chaosSystem;

    [Header("Game Information")]
    public int[] scores = {0, 0, 0, 0};

    public bool isGameOver = false;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audienceAudio;

    [SerializeField]
    public AudioSource audio;

    [SerializeField]
    private AudioClip audienceIdle;

    [SerializeField]
    private AudioClip audienceCheer;

    [SerializeField]
    private AudioClip gameOverSFX;

    [SerializeField]
    private AudioClip gameOverMusic;

    [SerializeField]
    private AudioClip timerLowSFX;

    private bool SFXFlag = true;
    private bool timerLowFlag = true;

    //initialization
    private void Awake()
    {
        lobbyWarp = playerManager.GetComponent<BackToLobby>();
        manager = playerManager.GetComponent<PlayerManager>();
    }

    //initialization
    void Start() 
    {
        timer = gameModeCanvas.transform.Find("Timer").GetComponent<TMP_Text>();
        p1 = gameModeCanvas.transform.Find("P1").GetComponent<TMP_Text>();
        p2 = gameModeCanvas.transform.Find("P2").GetComponent<TMP_Text>();
        p3 = gameModeCanvas.transform.Find("P3").GetComponent<TMP_Text>();
        p4 = gameModeCanvas.transform.Find("P4").GetComponent<TMP_Text>();
        winner = gameModeCanvas.transform.Find("Winner").GetComponent<TMP_Text>();
        win = gameModeCanvas.transform.Find("Winner").gameObject;
        timeRemaining = gameLen;
    }

    // Update is called once per frame
    void Update()
    {
        string temp = "";

        //scores and time are updated if the timer is greater than 0
        if (timeRemaining > 0) 
        {
            p1.text = "Blue: " + scores[0];

            if (manager.numPlayers >= 2) {
                p2.text = "Red: " + scores[1];
            }
            else {
                p2.text = "";
            }

            if (manager.numPlayers >= 3) {
                p3.text = "Green: " + scores[2];                
            }
            else {
                p3.text = "";
            }

            if (manager.numPlayers >= 4) {
                p4.text = "Yellow: " + scores[3];
            }
            else {
                p4.text = "";
            }
            
            timeRemaining -= Time.deltaTime;

            if (Mathf.Floor(timeRemaining) == 15 && timerLowFlag) {
                audio.PlayOneShot(timerLowSFX);
                timerLowFlag = false;
            }

            if (timeRemaining < 0)
            {
                timeRemaining = 0f;
            }

            if (((int) timeRemaining) % 60 < 10) 
            {
                temp = "0";
            }
            else {
                temp = "";
            }

            timer.text = (((int) timeRemaining) / 60) + ":" + temp + (((int) timeRemaining) % 60);
        }
        else {
            isGameOver = true;
            chaosSystem.SetActive(false);

            if (SFXFlag) {
                audio.PlayOneShot(gameOverSFX);
                audio.clip = gameOverMusic;
                audio.Play();
                audienceAudio.clip = audienceCheer;
                audienceAudio.Play();
                SFXFlag = false;
            }

            //winner is displayed when timer expires
            int max = scores[0];
            for (int i = 1; i < manager.numPlayers; i++) {
                if (scores[i] > max) {
                    max = scores[i];
                }
            }

            winner.text = "#1 Victory Royale\n";

            if (scores[0] == max) {
                winner.text += "Blue ";
            }

            if (scores[1] == max && manager.numPlayers >= 2) {
                winner.text += "Red ";
            }

            if (scores[2] == max && manager.numPlayers >= 3) {
                winner.text += "Green ";
            }

            if (scores[3] == max && manager.numPlayers >= 4) {
                winner.text += "Yellow ";
            }

            win.SetActive(true);

            //timer for auto reset
            if (reset > 0) 
            {
                reset -= Time.deltaTime;
            }

            //reset entire game
            else 
            {
                timeRemaining = gameLen;
                reset = 5;
                
                float minutes = Mathf.Floor(timeRemaining) / 60, seconds = Mathf.Floor(timeRemaining) % 60;
                if (seconds < 10) {
                    timer.text = minutes + ":0" + seconds;
                }
                else {
                    timer.text = minutes + ":" + seconds;
                }

                for (int i = 0; i < 4; i++) {
                    scores[i] = 0;
                }

                win.SetActive(false);
                audienceAudio.clip = audienceIdle;
                SFXFlag = true;
                timerLowFlag = true;
                isGameOver = false;

                lobbyWarp.toLobby();
            }
        }
    }
}
