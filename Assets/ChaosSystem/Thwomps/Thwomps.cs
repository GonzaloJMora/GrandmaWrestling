using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomps : Chaos
{

    [SerializeField] private Vector2Int minMaxSpawnAnoumt;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float maxHeight = 10f;
    private GameObject par;
    public override void Stop()
    {
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
            GameObject t = Instantiate(prefab, par.transform, false);

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
