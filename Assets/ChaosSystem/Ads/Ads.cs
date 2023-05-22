using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Ads : Chaos
{
    [SerializeField] private float spawnRadius = 1.98f;
    [SerializeField] private float spawnDelay;
    [SerializeField] private Vector2Int minMaxSpawnAmount;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject[] ads;
   
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip cashSFX;

    [SerializeField]
    private AudioClip appearSFX;

    [SerializeField]
    private AudioClip disappearSFX;

    private GameObject par;

    private void OnDrawGizmos()
    {

    }

    public override void Stop()
    {
        //Debug.Log("STOP FROM ADS");
        StartCoroutine(ResetParent());
        //throw new System.NotImplementedException();
    }


    IEnumerator ResetParent()
    {
        yield return LoseMoney(par);
        Destroy(par);
    }

    IEnumerator LoseMoney(GameObject p)
    {
        audio.PlayOneShot(disappearSFX);
        for (int i = 0; i < p.transform.childCount; i += 1)//while (p.transform.childCount != 0)
        {
            audio.PlayOneShot(appearSFX);
            p.transform.GetChild(i).gameObject.SetActive(false);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public override void Trigger()
    {
        int spawnAmount = Random.Range(minMaxSpawnAmount.x, minMaxSpawnAmount.y + 1);
        par = Instantiate(canvas);
        //Debug.Log("HERE1" + spawnAmount);
        StartCoroutine(IncreaseRevenue(spawnAmount, par));
        //throw new System.NotImplementedException();
    }

    IEnumerator IncreaseRevenue(int spawnAmount, GameObject p)
    {
        float sf = p.GetComponent<Canvas>().scaleFactor;
        for (int i = 0; i < spawnAmount; i += 1)
        {

            float randY = Random.Range(0, 1080) * sf;
            float randX = Random.Range(0, 1920) * sf;

            int randIndex = Random.Range(0, ads.Length);

            audio.PlayOneShot(appearSFX);
            GameObject ad = Instantiate(ads[randIndex], p.transform, true);
            RectTransform rt = ad.GetComponent<RectTransform>();

            

            rt.position = new Vector3(randX, randY, 0f);

            Vector2 scales = ads[randIndex].GetComponent<AdSize>().minMaxScales;
            float randScale = Random.Range(scales.x, scales.y);

            rt.localScale = new Vector3(randScale, randScale, randScale);
            

            yield return new WaitForSeconds(spawnDelay);
        }

        audio.PlayOneShot(cashSFX);
    }

}
