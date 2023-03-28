using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popcorn : Chaos
{
    [SerializeField] private float force;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private Vector2 minMaxAngle;
    [SerializeField] private GameObject platform;
    private float sign = 1f;
    private float currTime = 0f;
    private float degree;

    private bool isTriggered = false;
    private bool isReady = false;
    private bool hasAppliedForce = false;
    // Start is called before the first frame update
    void Start()
    {
        degree = Random.Range(minMaxAngle.x, minMaxAngle.y) * sign;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isTriggered) { isReady = true;  return; }
        isReady = false;

        currTime += Time.fixedDeltaTime;
     
        //rotate platform by amount;
        var nine = Quaternion.AngleAxis(degree, Vector3.forward);
        var amount = Quaternion.RotateTowards(platform.transform.rotation, nine, speed * Time.fixedDeltaTime);
        platform.transform.rotation = amount;
        
        //if done rotating and has not applied force
        if(platform.transform.rotation == nine && !hasAppliedForce)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            hasAppliedForce = true;
            
            //loop through all the players
            for(int i = 0; i < players.Length; i += 1)
            {
                Debug.Log("POP: " + players[i].name);
                //if player in the air, dont add force
                Controller c = players[i].GetComponent<Controller>();
                if(!c.isGrounded()) { continue; }

                Rigidbody rb = players[i].GetComponent<Rigidbody>();
                
                //get direction
                Vector3 direction = players[i].transform.position - platform.transform.position;
                
                //don't apply force in z direction
                direction.z = 0f;

                
                //if player not on the correct side of platform
                if (amount.z * players[i].transform.position.x < 0f)
                {
                    continue;
                }

                //reverse the x direction
                direction.x *= -1f;
                direction = direction.normalized;
                
                //if not at the top part of the platform
                if(direction.y < 0)
                {
                    continue;
                }

                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
            }
        }
        
        //rotate if enough time has passed
        if (currTime > waitTime)
        {
            hasAppliedForce = false;
            currTime = 0f;
            sign *= -1f;
            degree = Random.Range(minMaxAngle.x, minMaxAngle.y) * sign;
            
        }

    }

    public override void Trigger()
    {
        //Debug.Log("orgin: " + platform.transform.rotation);
        //Debug.Log("iden: " + Quaternion.identity);
        isTriggered = true;
    }

    public override void Stop()
    {
        StartCoroutine(ResetPlatform());
    }

    IEnumerator ResetPlatform()
    {
        isTriggered = false;
        while (!isReady) { yield return new WaitForFixedUpdate();}
        var nine = Quaternion.AngleAxis(0, Vector3.forward);
        var amount = Quaternion.RotateTowards(platform.transform.rotation, nine, speed * Time.fixedDeltaTime);
        while(platform.transform.rotation != nine)
        {
            platform.transform.rotation = amount;
            nine = Quaternion.AngleAxis(0, Vector3.forward);
            amount = Quaternion.RotateTowards(platform.transform.rotation, nine, speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        //Debug.Log(platform.transform.rotation.eulerAngles.z);
        platform.transform.rotation = amount;
        //Debug.Log("HERE");
    }
}
