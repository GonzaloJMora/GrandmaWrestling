using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JabAttack : MonoBehaviour
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
            if (other.gameObject.GetComponent<Controller>().isBlocking)
            {
                gameObject.transform.parent.GetComponent<Controller>().getHit(50, other.gameObject.transform.position);
                return;
            }
            Debug.Log("Player has taken a hit!");
            other.gameObject.GetComponent<Controller>().getHit(40, gameObject.transform.position);
        }
    }
}
