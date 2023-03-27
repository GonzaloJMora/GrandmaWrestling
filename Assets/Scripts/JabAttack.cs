using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabAttack : MonoBehaviour
{
    //access to lastCollision
    private PlayerManager playerManager;
    [SerializeField] private GameObject o;

    //which player are we (0-blue, 1-red, 2-green, 3-yellow)
    private int index;

    private bool isOpponent(Collider other)
    {
        //if owner of the attack is same color as the what we collided with, it is ourself, not an opponent
        if ((other.gameObject.GetComponent<MeshRenderer>().material.color) == (transform.parent.GetComponent<MeshRenderer>().material.color))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //get script
    private void Awake()
    {
        playerManager = o.GetComponent<PlayerManager>();
    }

    //figure out what player we are
    private void Start() {
        Color c = transform.parent.GetComponent<MeshRenderer>().material.color;

        if (c == Color.blue) {
            index = 0;
        }

        else if (c == Color.red) {
            index = 1;
        }

        else if (c == Color.green) {
            index = 2;
        }

        else if (c == Color.yellow) {
            index = 3;
        }
    }

    //update who was hit by who
    private void updateCollision(Color color) {
            if (color == Color.blue) {
                playerManager.lastCollision[0] = index;
            }

            else if (color == Color.red) {
                playerManager.lastCollision[1] = index;
            }

            else if (color == Color.green) {
                playerManager.lastCollision[2] = index;
            }

            else if (color == Color.yellow) {
                playerManager.lastCollision[3] = index;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Block")
        {
            Debug.Log("Attack has been blocked!");
        }

        //remove this if you dont want attacks to block each other
        else if (other.tag == "Attack")
        {
            Debug.Log("Attack collided with another attack!");
        }

        else if ((other.tag == "Player") && (isOpponent(other)))
        {
            if (other.gameObject.GetComponent<Controller>().isBlocking)
            {
                gameObject.transform.parent.GetComponent<Controller>().getHit(50, other.gameObject.transform.position);
                return;
            }
            updateCollision(other.gameObject.GetComponent<MeshRenderer>().material.color);
            Debug.Log("Player has taken a hit!");
            other.gameObject.GetComponent<Controller>().getHit(25, gameObject.transform.position);
        }
    }
}
