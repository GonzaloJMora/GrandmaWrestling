using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpin : Chaos
{
    [SerializeField] private GameObject cameraAxis;
    [SerializeField] private Vector2 minMaxRotateSpeed;
    [SerializeField] private float waitTime;
    private bool stop = false;

    public override void Stop()
    {
        Debug.Log("STOP FRO CAMERASPIN");
        stop = true;
        
        StartCoroutine(ResetCamera(0f, Random.Range(minMaxRotateSpeed.x, minMaxRotateSpeed.y)));
        
    }

    public override void Trigger()
    {
        stop = false;
        StartCoroutine(StartCyclce());

    }

    IEnumerator StartCyclce()
    {
        while(true)
        {
            float yRotateDegree = Random.Range(-180, 180f);
            //Debug.Log(yRotateDegree);
            float speed = Random.Range(minMaxRotateSpeed.x, minMaxRotateSpeed.y);
            
            if(stop)
            {
                //Debug.Log("STOPING CYCLE");
                yield break;
            }

            yield return RotateToNewPosition(yRotateDegree, speed);
        }

    }

    IEnumerator ResetCamera(float degree, float speed)
    {
        Debug.Log("RESETTING CAMERA");
        float delta = 0.01f;
        float cameraYQaut = cameraAxis.transform.rotation.y;
        Quaternion target = Quaternion.Euler(Vector3.up * degree);
        bool thing = cameraYQaut > target.y - delta && cameraYQaut < target.y + delta;
        while (!(thing))
        {
            Quaternion dist = Quaternion.RotateTowards(cameraAxis.transform.rotation, target, Time.deltaTime * speed);
            cameraAxis.transform.rotation = dist;

            cameraYQaut = cameraAxis.transform.rotation.y;

            thing = cameraYQaut > target.y - delta && cameraYQaut < target.y + delta;

            //Debug.Log("rotation: " + cameraYQaut + "Degree: " + target.y + " thing: " + thing);
            //Debug.Log(thing);
            yield return null;
        }
    }

    IEnumerator RotateToNewPosition(float degree, float speed)
    { 
        //distance to be within
        float delta = 0.01f;

        //the Y quanternion of the camera
        float cameraYQaut = cameraAxis.transform.rotation.y;
        
        //position to be
        Quaternion target = Quaternion.Euler(Vector3.up * degree);

        //if within delta of the target
        bool thing = cameraYQaut > target.y - delta && cameraYQaut < target.y + delta;
        while (!(thing))
        {
            //distance to move
            Quaternion dist = Quaternion.RotateTowards(cameraAxis.transform.rotation, target, Time.deltaTime * speed);
            cameraAxis.transform.rotation = dist;

            //update value
            cameraYQaut = cameraAxis.transform.rotation.y;

            //if not called to stop
            if (stop)
            {
                //Debug.Log("STOPING Rotating");
                yield break;
            }

            thing = cameraYQaut > target.y - delta && cameraYQaut < target.y + delta;

            //Debug.Log("rotation: " + cameraYQaut + "Degree: " + target.y + " thing: " + thing);
            //Debug.Log(thing);
            yield return null;
        }
    }


}
