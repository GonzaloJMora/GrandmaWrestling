using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabAttack : MonoBehaviour
{
    [Header("Player Manager")]
    //access to lastCollision
    [SerializeField] private PlayerManager playerManager;

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
            //Debug.Log("Attack has been blocked!");
        }

        //remove this if you dont want attacks to block each other
        else if (other.tag == "Attack")
        {
            //Debug.Log("Attack collided with another attack!");
        }

        else if ((other.tag == "Player") && (isOpponent(other)))
        {
            if (other.gameObject.GetComponent<Controller>().isBlocking)
            {
                if (other.transform.Find("BlockBox").GetComponent<BlockMove>().isSweet)
                {
                    gameObject.transform.parent.GetComponent<Controller>().getHit((int)(25*1.5), other.gameObject.transform.position); //damage is reflected to you
                    Debug.Log("Sweet Parry!");
                }
                else if (other.transform.Find("BlockBox").GetComponent<BlockMove>().isVulnerable)
                {
                    Debug.Log("Missed Block!");
                    other.gameObject.GetComponent<Controller>().getHit((int)(25), gameObject.transform.position); //opponent take full knockback
                }
                else
                {
                    Debug.Log("Weak Block");
                    other.gameObject.GetComponent<Controller>().getHit((int)(25/3), gameObject.transform.position); //opponent take reduced knockback
                }
                return;
            }
            updateCollision(other.gameObject.GetComponent<MeshRenderer>().material.color);
            Debug.Log("Player has taken a hit!");
            other.gameObject.GetComponent<Controller>().getHit(25, gameObject.transform.position);
        }
    }
}
