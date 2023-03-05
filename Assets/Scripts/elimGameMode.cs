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
    private GameObject gameModeCanvas;
    private TMP_Text timer;

    void Start() 
    {
        timer = gameModeCanvas.transform.Find("Timer").GetComponent<TMP_Text>();
        timeRemaining = gameLen;
    }

    // Update is called once per frame
    void Update()
    {
        string temp = "";

        if (timeRemaining > 0) 
        {
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
            //Debug.Log("Timer is done");
            if (reset > 0) 
            {
                reset -= Time.deltaTime;
            }

            else 
            {
                timeRemaining = gameLen;
                reset = 5;
                timer.text = "0:" + Mathf.Floor(timeRemaining);
            }
        }
    }
}
