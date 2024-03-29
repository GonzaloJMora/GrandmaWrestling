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
    [SerializeField] private GameObject gameModeCanvas;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject scoreBoards;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text winnerText;
    
    private TMP_Text p1, p2, p3, p4;

    [Header("Player Manager")]
    [SerializeField]
    private GameObject playerManager;
    private PlayerManager manager;

    //reference to game to lobby transition script
    private BackToLobby lobbyWarp;

    [Header("Game Information")]
    public int[] scores = {0, 0, 0, 0};

    public bool isGameOver = false;

    public bool canStartVoting = true;

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
        p1 = scoreBoards.transform.Find("P1").GetComponentInChildren<TMP_Text>();
        p2 = scoreBoards.transform.Find("P2").GetComponentInChildren<TMP_Text>();
        p3 = scoreBoards.transform.Find("P3").GetComponentInChildren<TMP_Text>();
        p4 = scoreBoards.transform.Find("P4").GetComponentInChildren<TMP_Text>();

        timeRemaining = gameLen;
    }

    // Update is called once per frame
    void Update()
    {
        string temp = "";

        //scores and time are updated if the timer is greater than 0
        if (timeRemaining > 0) 
        {
            p1.text = "" + scores[0];

            if (manager.numPlayers >= 2) {
                p2.transform.parent.gameObject.SetActive(true);
                p2.text = "" + scores[1];
            }
            else {
                p2.text = "";
                p2.transform.parent.gameObject.SetActive(false);
            }

            if (manager.numPlayers >= 3) {
                p3.transform.parent.gameObject.SetActive(true);
                p3.text = "" + scores[2];                
            }
            else {
                p3.text = "";
                p3.transform.parent.gameObject.SetActive(false);
            }

            if (manager.numPlayers >= 4) {
                p4.transform.parent.gameObject.SetActive(true);
                p4.text = "" + scores[3];
            }
            else {
                p4.text = "";
                p4.transform.parent.gameObject.SetActive(false);
            }
            
            timeRemaining -= Time.deltaTime;

            if (Mathf.Floor(timeRemaining) == 15 && timerLowFlag) {
                audio.PlayOneShot(timerLowSFX);
                timerLowFlag = false;
            }

            if (Mathf.Floor(timeRemaining) < 25) {
                canStartVoting = false;
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

            winnerText.text = "#1 Victory Royale\n";

            if (scores[0] == max) {
                winnerText.text += "Blue ";
            }

            if (scores[1] == max && manager.numPlayers >= 2) {
                winnerText.text += "Red ";
            }

            if (scores[2] == max && manager.numPlayers >= 3) {
                winnerText.text += "Green ";
            }

            if (scores[3] == max && manager.numPlayers >= 4) {
                winnerText.text += "Yellow ";
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
                canStartVoting = true;
                
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
