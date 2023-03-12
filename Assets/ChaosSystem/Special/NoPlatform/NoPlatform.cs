using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPlatform : Chaos
{
    [SerializeField] private float waitTime;
    [SerializeField] private GameObject canvasPrefab;
    [SerializeField] private GameObject platform;

    private GameObject canvas;
    public override void Stop()
    {
        //throw new System.NotImplementedException();
    }

    public override void Trigger()
    {
        StartCoroutine(nothing());
        //throw new System.NotImplementedException();
    }

    private IEnumerator nothing()
    {
        yield return StartCoroutine(ShowCanvas());
        yield return StartCoroutine(HideCanvas());
    }

    private IEnumerator ShowCanvas()
    {
        canvas = Instantiate(canvasPrefab);
        yield return new WaitForSeconds(waitTime);
    }

    private IEnumerator HideCanvas()
    {
        canvas.SetActive(false);
        platform.SetActive(false);
        yield return new WaitForSeconds(3f);
        platform.SetActive(true);
    }

}
