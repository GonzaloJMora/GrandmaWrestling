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
                Debug.Log("STOPING CYCLE");
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
        float delta = 0.01f;
        float cameraYQaut = cameraAxis.transform.rotation.y;
        Quaternion target = Quaternion.Euler(Vector3.up * degree);
        bool thing = cameraYQaut > target.y - delta && cameraYQaut < target.y + delta;
        while (!(thing))
        {
            Quaternion dist = Quaternion.RotateTowards(cameraAxis.transform.rotation, target, Time.deltaTime * speed);
            cameraAxis.transform.rotation = dist;

            cameraYQaut = cameraAxis.transform.rotation.y;

            if (stop)
            {
                Debug.Log("STOPING Rotating");
                yield break;
            }

            thing = cameraYQaut > target.y - delta && cameraYQaut < target.y + delta;

            //Debug.Log("rotation: " + cameraYQaut + "Degree: " + target.y + " thing: " + thing);
            //Debug.Log(thing);
            yield return null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
