using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class elimGameMode : MonoBehaviour
{
    private float timeRemaining;
    public float gameLen = 60;
    private float reset = 5;

    [SerializeField]
    private GameObject gameModeCanvas, win;
    private TMP_Text timer, p1, p2, p3, p4, winner;
    public int[] scores = {0, 0, 0, 0};

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
            p2.text = "Red: " + scores[1];
            p3.text = "Green: " + scores[2];
            p4.text = "Yellow: " + scores[3];
            timeRemaining -= Time.deltaTime;
            //Debug.Log(Mathf.Floor(timeRemaining));

            if (timeRemaining < 0)
            {
                timeRemaining = 0f;
            }

            if (timeRemaining < 10) 
            {
                temp = "0";
            }

            timer.text = "0:" + temp + Mathf.Floor(timeRemaining);
        }
        else {
            //winner is displayed when timer expires
            int max = scores[0];
            for (int i = 1; i < 4; i++) {
                if (scores[i] > max) {
                    max = scores[i];
                }
            }

            winner.text = "#1 Victory Royale\n";

            if (scores[0] == max) {
                winner.text += "Blue ";
            }

            if (scores[1] == max) {
                winner.text += "Red ";
            }

            if (scores[2] == max) {
                winner.text += "Green ";
            }

            if (scores[3] == max) {
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
            }
        }
    }
}
