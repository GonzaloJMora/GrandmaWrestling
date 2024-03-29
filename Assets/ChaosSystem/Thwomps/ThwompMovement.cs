using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ThwompMovement : MonoBehaviour
{
    private enum ThwompStates { Moving, Shaking, Attacking, Recovering, Resetting};

    [SerializeField] private float speed;
    [SerializeField] private float maxHeight;
    [SerializeField] private float shakeTime;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float recoveryTime;

    [SerializeField] private Vector3 targetPos;

    private GameObject[] players;

    private ThwompStates state;
    private float currTime = 0f;
    private float currShakeTime = 0f;
    private float sign = 1f;
    private float minFallHeight;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip shakingSFX;

    [SerializeField]
    private AudioClip crushSFX;

    private bool SFXFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        minFallHeight = gameObject.transform.localScale.y / 2;        
        NewTargetPos();
        state = ThwompStates.Moving;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        if (state == ThwompStates.Moving)
        {
            //is Within a certian distance
            float dist = Mathf.Abs(Vector3.Distance(targetPos, transform.position));
            if(dist < 0.1f)
            {
                state = ThwompStates.Shaking;
                //NewTargetPos();
            }
            transform.position += Vector3.Normalize(targetPos - transform.position) * speed * Time.deltaTime;
        }
        else if(state == ThwompStates.Shaking)
        {
            playSFX();

            currTime += Time.deltaTime;
            currShakeTime += Time.deltaTime;
            if (currTime > 0.05f)
            {
                float degree = Random.Range(0f, 15f) * sign;
                sign *= -1f;
                var nine = Quaternion.AngleAxis(degree, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, nine, 90f);//nine * transform.rotation
                currTime = 0f;
            }
            if(currShakeTime > shakeTime)
            {
                //reset rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), 90f);
                state = ThwompStates.Attacking;
                currTime = 0f;
                currShakeTime = 0f;
                audio.Stop();
                SFXFlag = true;
            }
        }
        else if(state == ThwompStates.Attacking)
        {
            //float dist = Mathf.Abs(minFallHeight - transform.position.y);
            if(transform.position.y < minFallHeight)
            {
                audio.PlayOneShot(crushSFX);
                state = ThwompStates.Recovering;
            }
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else if(state == ThwompStates.Recovering)
        {
            currTime += Time.deltaTime;
            if(currTime > recoveryTime)
            {
                currTime = 0f;
                state = ThwompStates.Resetting;
            }
        }
        else if(state == ThwompStates.Resetting)
        {
            //float dist = Mathf.Abs(maxHeight - transform.position.y);
            if (transform.position.y > maxHeight)
            {
                state = ThwompStates.Moving;
                NewTargetPos();
            }
            transform.position += Vector3.up * fallSpeed * Time.deltaTime;
        }
        


    }

    private void NewTargetPos()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length == 0) { targetPos = new Vector3(0f, maxHeight, 0f); return; }
        //Debug.Log(players.Length);  
        int targetIndex = Random.Range(0, players.Length);
        Vector3 t = players[targetIndex].transform.position;
        targetPos = new Vector3(t.x, maxHeight, t.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject g = other.transform.parent.gameObject;
        //Debug.Log("THWOMP: " + g.name);
        if(g.CompareTag("Player"))
        {
            g.transform.position = new Vector3(g.transform.position.x, -5f, g.transform.position.z);
        }
    }

    public void SetHeight(float height)
    {
        maxHeight = height;
    }

    private void playSFX() {
        if (SFXFlag) {
            audio.clip = shakingSFX;
            audio.Play();
            SFXFlag = false;
        }
    }
}
