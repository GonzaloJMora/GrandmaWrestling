using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : Chaos
{
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private float mineRadius;
    [SerializeField] private float force;
    [SerializeField] private float Upforce;
    [SerializeField] private Vector2Int MinMaxMineAmount;
    [SerializeField] private float mineSpawnDelay;

    private IEnumerator coroutine;

    private GameObject par;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip mineSpawnSFX;

    [SerializeField]
    private AudioClip explosionSFX;

    public override void Stop()
    {
        //Debug.Log("STOP FRO MINES");
        if (par != null) {
            audio.PlayOneShot(explosionSFX);
            StartCoroutine(ResetParent());
        }

    }

    IEnumerator ResetParent()
    {
        StopCoroutine(coroutine);
        yield return ExplodeChildren(par);
        Destroy(par);
    }

    IEnumerator ExplodeChildren(GameObject p)
    {
        for(int i = 0; i<p.transform.childCount; i += 1)//while (p.transform.childCount != 0)
        {
            MinesExplosion me = p.transform.GetChild(i).gameObject.GetComponent<MinesExplosion>();
            //Debug.Log("About to explode");
            me.Explode();

            yield return null;
        }       
    }

    public override void Trigger()
    {
        //Get Random number of mines to spawn
        int spawnAmount = Random.Range(MinMaxMineAmount.x, MinMaxMineAmount.y + 1);
        par = new GameObject();
        par.transform.position = new Vector3(0f, 10f, 0f);
        coroutine = SpawnMines(spawnAmount, par);
        StartCoroutine(coroutine);
    }

    IEnumerator SpawnMines(int spawnAmount, GameObject p)
    {
        int i = 0;
        while (i < spawnAmount)
        { 
            Vector2 sph = Random.insideUnitCircle * radius;
            Vector3 pos = new Vector3(sph.x, p.transform.position.y, sph.y);
            GameObject mine = Instantiate(minePrefab, p.transform, true);
            mine.transform.position = pos;
            audio.PlayOneShot(mineSpawnSFX);
            
            MinesExplosion me = mine.GetComponent<MinesExplosion>();
            me.Radius = mineRadius;
            me.Force = force;
            me.UpForce = Upforce;
            me.count = i;


            i += 1;
            //Debug.Log("i: " + i);
            yield return new WaitForSeconds(mineSpawnDelay);
        }
    }
}
