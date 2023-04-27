using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomps : Chaos
{

    [SerializeField] private Vector2Int minMaxSpawnAnoumt;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float maxHeight = 10f;

    [SerializeField] private Texture[] bens;
    private GameObject par;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip disappearSFX;

    [SerializeField]
    private AudioClip appearSFX;

    public override void Stop()
    {
        audio.PlayOneShot(disappearSFX);
        Destroy(par);
        //throw new System.NotImplementedException();
    }

    public override void Trigger()
    {
        par = new GameObject();
        
        int amount = Random.Range(minMaxSpawnAnoumt.x, minMaxSpawnAnoumt.y);

        StartCoroutine(SpawnThwomps(amount, par));

        //throw new System.NotImplementedException();
    }

    IEnumerator SpawnThwomps(int spawnAmount, GameObject p)
    {
        int i = 0;
        while (i < spawnAmount)
        {
            audio.PlayOneShot(appearSFX);
            GameObject t = Instantiate(prefab, par.transform, false);
            t.GetComponent<Renderer>().material.mainTexture = bens[i % bens.Length];
            Vector3 spawnPos = Random.insideUnitCircle * radius;
            t.transform.position = spawnPos + new Vector3(0f, maxHeight, 0f);
            ThwompMovement tm = t.GetComponent<ThwompMovement>();
            tm.SetHeight(maxHeight);


            i += 1;
            //Debug.Log("i: " + i);
            yield return new WaitForSeconds(spawnDelay);
        }
    }


}
