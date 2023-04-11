using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceBehavior : MonoBehaviour
{

    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject floor;
    [SerializeField] private static Vector2 minMaxScale = new Vector2(0.5f, 0.9f);

    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpStep;
    private float baseHeight;
    private bool isJumping = false;

    [SerializeField]
    private SoundTicketManager sound;

    [SerializeField]
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    { 
        foreach (Transform row in transform)
        { 
            foreach(Transform aud in row)
            {
                MeshRenderer m = aud.GetComponent<MeshRenderer>();

                int index = Random.Range(0, colors.Length);
                m.material.color = colors[index];

                float s = Random.Range(minMaxScale.x, minMaxScale.y);
                aud.localScale = new Vector3(s, s, s);
                aud.transform.position += aud.localScale / 2f;

                aud.LookAt(floor.transform);
            }
        }
    }

    public void  Trigger()
    {
        if(!isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
            //StartCoroutine(Spinner());
        }
    }

    private IEnumerator Jump()
    {
        float currStep = jumpStep;
        float t = 0f;

        sound.playSound();

        while(true)
        {
            t += Time.deltaTime;
            transform.position += new Vector3(0f, currStep, 0f) * Time.deltaTime;
            if(transform.position.y > baseHeight + jumpHeight)
            {
                currStep *= -1f;
            }
            else if(transform.position.y < baseHeight)
            {
                transform.position = new Vector3(0f, 0f, 0f);
                break;
            }

            yield return null;
        }
        //Debug.Log("T: " + t);
        //Debug.Log("TD: " + Time.deltaTime);
        isJumping = false;
        yield return null;
    }

    private IEnumerator Spinner()
    {
        foreach (Transform row in transform)
        {
            foreach(Transform aud in row)
            {
                StartCoroutine(Spin(aud));
            }
            yield return null;
        }
        yield return null;
    }
  
    private IEnumerator Spin(Transform child)
    {
        float angle = 180;
        float rotationTime = 2f * (jumpHeight / jumpStep);
        float rotationSpeed = angle / rotationTime;

        Quaternion target = Quaternion.Euler(Vector3.up * angle);

        float t = 0f;
        while (t < rotationTime)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0f;
        angle = 360f;
        target = Quaternion.Euler(Vector3.up * angle);
        while (t < rotationTime)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.identity;
        yield return null;
    }    

}
