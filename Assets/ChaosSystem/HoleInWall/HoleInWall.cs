using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HoleInWall : Chaos
{
    [SerializeField] private Vector2Int minMaxSpawnAmount;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private GameObject[] walls;
    [SerializeField] private float intialDistance;

    private bool isDone = false;

    private GameObject par;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip hornSFX;

    public override void Stop()
    {
        isDone = false;
        Destroy(par);
        //throw new System.NotImplementedException();
    }

    private void FixedUpdate()
    {
        if(isDone)
        {
            par.transform.position -= Vector3.forward * Time.fixedDeltaTime * speed;
        }
    }

    public override void Trigger()
    {
        audio.PlayOneShot(hornSFX);
        int amount = Random.Range(minMaxSpawnAmount.x, minMaxSpawnAmount.y);
        par = new GameObject();
        par.transform.position = new Vector3(0, 0, 100);
        
        for(int i = 0; i < amount; i += 1)
        {
            int index = Random.Range(0, walls.Length);
            GameObject w = Instantiate(walls[index], par.transform, true);
            w.transform.position = new Vector3(0, 0, intialDistance + (distance * i));

            WallAmount thing = w.GetComponent<Wall>().wallAmount;

            if (thing == WallAmount.Any)
            {
                thing = (WallAmount)Random.Range(1, System.Enum.GetNames(typeof(WallAmount)).Length);
            }
            if(thing == WallAmount.Multiple)
            {
                int x = Random.Range(1, 3);
                for(int j = 0; j < x; j += 1)
                {
                    GameObject wc = Instantiate(walls[index], w.transform, true);
                    wc.GetComponent<Wall>().isUseless = true;
                    if (j%2 == 1)
                    {
                        wc.transform.position = w.transform.position - new Vector3(5f,0f,0f);
                    }
                    else
                    {
                        wc.transform.position = w.transform.position + new Vector3(5f, 0f, 0f);
                    }

                    wc.transform.position += new Vector3(0f, 0f, (5 * (j + 1)));
                }
            }
        }

        isDone = true;
        //throw new System.NotImplementedException();
    }

}
