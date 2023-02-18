using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //list of players
    private List<PlayerInput> players = new List<PlayerInput>();

    //spawn points of players
    [SerializeField]
    private List<Transform> startingPoints;

    //unused (might remove if i dont find a use for it)
    [SerializeField]
    private List<LayerMask> playerLayers;

    //list of colors to help differentiate players
    [SerializeField]
    private List<Color> colors = new List<Color> {Color.blue, Color.red, Color.green, Color.yellow};

    //list of player numbers that can currently be spawned in
    private bool[] availablePlayers = { true, true, true, true };

    //reference to our input manager
    private PlayerInputManager playerInputManager;

    //initialization
    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
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
                meshRenderer.material.color = colors[i];
                player.transform.position = startingPoints[i].position;
                availablePlayers[i] = false;
                break;
            }
        }
    }

    //used by the plane this script is attached to
    private void OnTriggerEnter(Collider other)
    {
        //use the color to figure out which player got destroyed
        Color color = other.gameObject.GetComponent<MeshRenderer>().material.color;

        //set the corresponding player to available
        if (color == Color.blue)
        {
            availablePlayers[0] = true;
        }
        else if (color == Color.red)
        {
            availablePlayers[1] = true;
        }
        else if (color == Color.green)
        {
            availablePlayers[2] = true;
        }
        else if (color == Color.yellow)
        {
            availablePlayers[3] = true;
        }

        //remove player from list and finally destroy object
        players.Remove(other.gameObject.GetComponent<PlayerInput>());
        Destroy(other.gameObject);
    }
}
