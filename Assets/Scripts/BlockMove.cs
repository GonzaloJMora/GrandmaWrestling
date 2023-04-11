using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    //access to lastCollision
    private PlayerManager playerManager;
    [SerializeField] private GameObject o;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip blockSFX;

    //index of who we are (0-blue, 1-red, 2-green, 3-yellow)
    private int index;

    //get playermanager script
    private void Awake()
    {
        playerManager = o.GetComponent<PlayerManager>();
    }

    //find out who we are
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            Debug.Log("Both players blocked nothing!");
        }

        else if (other.tag == "Attack")
        {
            //update who hit who
            Color color = other.transform.parent.gameObject.GetComponent<MeshRenderer>().material.color;
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

            audio.PlayOneShot(blockSFX);
            Debug.Log("You just blocked an attack!");
        }

        else if (other.tag == "Player")
        {
            Debug.Log("What is this debug statement here for, this is block!");
        }
    }
}
