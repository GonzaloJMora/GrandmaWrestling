using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            Debug.Log("Both players blocked nothing!");
        }

        else if (other.tag == "Attack")
        {
            Debug.Log("You just blocked an attack!");
        }

        else if (other.tag == "Player")
        {
            Debug.Log("What is this debug statement here for, this is block!");
        }
    }
}
