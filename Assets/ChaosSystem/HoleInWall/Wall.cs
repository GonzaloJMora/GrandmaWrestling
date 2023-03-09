using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallPosition { Any, Center, Side }
public enum WallAmount { Any, Single, Multiple}

public enum WallMovement { Any, Stable, Sin, SparaticSin}

public class Wall : MonoBehaviour
{
    public WallPosition wallPosition;
    public WallAmount wallAmount;
    public WallMovement wallMovement;

    private Vector3 startPos;

    private float currTime = 0f;
    [SerializeField] private Vector2 freq = new Vector2(1f, 4f);
    [SerializeField] private Vector2 peak = new Vector2(1, 3f);
    public bool isUseless;
    private float currFreq;
    private float currPeak;
    public void Move()
    {
        StartCoroutine(Movement());
    }

    private void Start()
    {

        //Debug.Log("HERE FROM WALL");
        currFreq = Random.Range(freq.x, freq.y);
        currPeak = Random.Range(peak.x, peak.y);

        if(wallMovement == WallMovement.Any)
        {
            wallMovement = (WallMovement)Random.Range(1, 2 + 1);
        }
        
        startPos = transform.localPosition;

    }

    private void FixedUpdate()
    {
        if(isUseless) { return; }
        currTime += Time.fixedDeltaTime;
        if (wallMovement == WallMovement.Sin)
        {
            transform.localPosition = startPos + new Vector3(Mathf.Sin(currTime * currFreq),0,0) * currPeak;
        }
        else if(wallMovement == WallMovement.Stable)
        {
            //don't move
        }
        else if(wallMovement == WallMovement.SparaticSin)
        {

        }


    }

    private IEnumerator Movement()
    {
        yield return null;
    }

    public void Stop()
    {
        StopCoroutine(Movement());
    }

}


