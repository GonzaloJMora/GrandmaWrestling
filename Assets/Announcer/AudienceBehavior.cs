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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  Trigger()
    {
        if(!isJumping)
        {
            isJumping = true;
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        float currStep = jumpStep;
        while(true)
        {
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

        isJumping = false;
        yield return null;
    }

}
