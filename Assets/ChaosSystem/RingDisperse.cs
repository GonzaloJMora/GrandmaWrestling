using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingDisperse : Chaos
{
    private enum RingState { Shaking, Moving, Reset}
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeTime;
    [SerializeField] private GameObject floor;
    private Transform ring;
    private RingState state;

    private float currTime = 0f;
    private float currShakeTime = 0f;
    private float sign = 1f;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip shakingSFX;

    [SerializeField]
    private AudioClip disappearSFX;

    [SerializeField]
    private AudioClip reappearSFX;

    public override void Stop()
    {
        StopCoroutine(RingMovement());
        ring.localPosition = Vector3.zero; 
        audio.PlayOneShot(reappearSFX); 
        ring.gameObject.SetActive(true);
        //throw new System.NotImplementedException();
    }

    public override void Trigger()
    {
        int index = Random.Range(0, floor.transform.childCount-1);
        
        ring = floor.transform.GetChild(index);
        state = RingState.Shaking;
        currTime = 0f;
        currShakeTime = 0f;
        sign = 1f;
        StartCoroutine(RingMovement());
        
    }

    private IEnumerator RingMovement()
    {
        audio.clip = shakingSFX;
        audio.Play();

        while(true)
        {
            if(state == RingState.Shaking)
            {
                currTime += Time.deltaTime;
                currShakeTime += Time.deltaTime;
                if (currTime > 0.05f)
                {
                    sign *= -1f;
                    ring.position += new Vector3(0f, shakeAmount * sign, 0f) * Time.deltaTime;
                    currTime = 0f;
                }
                if (currShakeTime > shakeTime)
                {
                    //reset rotation
                    //ring.rotation = Quaternion.RotateTowards(ring.rotation, Quaternion.AngleAxis(0, Vector3.forward), 90f);
                    state = RingState.Moving;
                    currTime = 0f;
                    currShakeTime = 0f;
                }
            }
            else if(state == RingState.Moving)
            {
                audio.Stop();
                audio.PlayOneShot(disappearSFX);
                ring.gameObject.SetActive(false);
                break;
            }

            yield return null;
        }

        yield return null;
    }

}
