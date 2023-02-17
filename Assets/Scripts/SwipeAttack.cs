using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAttack : MonoBehaviour
{
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

        else if (other.tag == "Player")
        {
            Debug.Log("Player has taken a hit!");
        }
    }
}
