using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void Update() {
        if (manager.numPlayers > 0 && playersReady == manager.numPlayers) {
            if (timer > 0) {
                timer -= Time.deltaTime;
                //Debug.Log(Mathf.Floor(timer));

                if (timer < 0) {
                    timer = 0f;
                }
            }
            else {
                loadingScreen.SetActive(true);
                gameStuff.SetActive(true);
                chaosSystem.SetActive(true);
                lobbyStuff.SetActive(false);
                
                for(int i = 0; i < manager.numPlayers; i++) {
                    manager.players[i].transform.position = manager.startingPoints[i].position;
                }

                Invoke("deactivateLoading", 1);
            }
        }
    }

    private void deactivateLoading() {
        loadingScreen.SetActive(false);
    }

    private void OnCollisionEnter(Collision other) {
        playersReady++;
    }

    private void OnCollisionExit(Collision other) {
        playersReady--;
        timer = startTime;
    }
}
