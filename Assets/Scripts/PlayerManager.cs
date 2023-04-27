using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Data")]
    public int numPlayers = 0;

    //list of players
    public List<PlayerInput> players = new List<PlayerInput>();

    //spawn points of players
    //[SerializeField]
    public List<Transform> startingPoints;

    //list of colors to help differentiate players
    [SerializeField]
    private List<Color> colors = new List<Color> {Color.blue, Color.red, Color.green, Color.yellow};

    //keeps track of the last collision that ocurred (-1 for none, 0-3 for P1-P4 respectively) (used for point incrementation)
    public int[] lastCollision = {-1, -1, -1, -1};

    //list of player numbers that can currently be spawned in
    private bool[] availablePlayers = { true, true, true, true };

    //reference to our input manager
    private PlayerInputManager playerInputManager;

    //handles score changes to the ui
    private elimGameMode uiRef;

    [Header("Game Mode")]
    [SerializeField]
    private GameObject gameMode;
    public bool isGameStarted = false;

    [Header("Audience")]
    [SerializeField] private AudienceBehavior audience;

    [Header("Audio")]
    [SerializeField]
    private SoundTicketManager joinSound;

    [SerializeField]
    private SoundTicketManager fallSound;

    [SerializeField]
    private AudioClip addPointSFX;

    [SerializeField]
    private AudioClip subPointSFX;

    private bool SFXFlag = true;

    [Header("Visual Effects")]
    [SerializeField]
    private GameObject explosionVFX;

    //initialization
    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
        uiRef = gameMode.GetComponent<elimGameMode>();
    }

    //initialization
    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    //called on destruction of player object
    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    //callback used to add a new player
    public void AddPlayer(PlayerInput player)
    {
        if (numPlayers >= 4 && !(isGameStarted)) {
            Destroy(player);
            return;
        }

        //get reference to the new player's mesh renderer
        MeshRenderer meshRenderer = player.GetComponent<MeshRenderer>();

        //add new player to player list
        players.Add(player);

        for (int i = 0; i < 4; i++)
        {
            //check to see if there is an available player slot and which one it is
            if (availablePlayers[i])
            {
                //set the player's color and spawn to the corresponding player's values
                joinSound.playSound();
                meshRenderer.material.color = colors[i];
                Instantiate(explosionVFX, startingPoints[i].position, Quaternion.identity);
                player.transform.position = startingPoints[i].position;
                availablePlayers[i] = false;
                numPlayers++;
                break;
            }
        }
    }

    //get rid of all players when returning to the lobby from the game
    public void RemoveAllPlayers() {
        int totPlayers = numPlayers;

        for (int i = 0; i < totPlayers; i++) {
            Destroy(players[0].gameObject);
            players.Remove(players[0]);
            availablePlayers[i] = true;
            numPlayers--;
        }
    }

    //used by the hitbox this script is attached to
    private void OnTriggerExit(Collider other)
    {
        //use the color to figure out which player fell
        Color color = other.gameObject.GetComponent<MeshRenderer>().material.color;

        //respawn player in appropriate spot
        if (color == Color.blue)
        {
            //availablePlayers[0] = true;
            other.transform.position = startingPoints[0].position;
            Instantiate(explosionVFX, startingPoints[0].position, Quaternion.identity);

            //update score
            if (!uiRef.isGameOver) {
                if (lastCollision[0] == -1) {
                    uiRef.scores[0] -= 1;
                    uiRef.audio.PlayOneShot(subPointSFX);
                }
                else {
                    uiRef.scores[lastCollision[0]] += 1;
                    uiRef.audio.PlayOneShot(addPointSFX);
                }
                lastCollision[0] = -1;
            }
        }
        else if (color == Color.red)
        {
            //availablePlayers[1] = true;
            other.transform.position = startingPoints[1].position;
            Instantiate(explosionVFX, startingPoints[1].position, Quaternion.identity);

            //update score
            if (!uiRef.isGameOver) {
                if (lastCollision[1] == -1) {
                    uiRef.scores[1] -= 1;
                    uiRef.audio.PlayOneShot(subPointSFX);
                }
                else {
                    uiRef.scores[lastCollision[1]] += 1;
                    uiRef.audio.PlayOneShot(addPointSFX);
                }
                lastCollision[1] = -1;
            }
        }
        else if (color == Color.green)
        {
            //availablePlayers[2] = true;
            other.transform.position = startingPoints[2].position;
            Instantiate(explosionVFX, startingPoints[2].position, Quaternion.identity);

            //update score
            if (!uiRef.isGameOver) {
                if (lastCollision[2] == -1) {
                    uiRef.scores[2] -= 1;
                    uiRef.audio.PlayOneShot(subPointSFX);
                }
                else {
                    uiRef.scores[lastCollision[2]] += 1;
                    uiRef.audio.PlayOneShot(addPointSFX);
                }
                lastCollision[2] = -1;
            }
        }
        else if (color == Color.yellow)
        {
            //availablePlayers[3] = true;
            other.transform.position = startingPoints[3].position;
            Instantiate(explosionVFX, startingPoints[3].position, Quaternion.identity);

            //update score
            if (!uiRef.isGameOver) {
                if (lastCollision[3] == -1) {
                    uiRef.scores[3] -= 1;
                    uiRef.audio.PlayOneShot(subPointSFX);
                }
                else {
                    uiRef.scores[lastCollision[3]] += 1;
                    uiRef.audio.PlayOneShot(addPointSFX);
                }
                lastCollision[3] = -1;
            }
        }

        Controller c = other.gameObject.GetComponentInParent<Controller>();
        if(c != null)
        {
            if (SFXFlag) {
                fallSound.playSound();
                SFXFlag = false;
            }
            else {
                SFXFlag = true;
            }
            audience.Trigger();
            c.ResetPhysics();
            c.ResetHits();
        }
    }
}
