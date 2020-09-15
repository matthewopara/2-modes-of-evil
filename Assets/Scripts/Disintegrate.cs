using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disintegrate : MonoBehaviour
{
    public GameObject dust;
    public SpriteRenderer render;
    private WaitForSeconds pause;

    private void Awake()
    {
        pause = new WaitForSeconds(2f);
    }

    private void OnEnable()
    {
        StartCoroutine("Dust");
    }

    IEnumerator Dust()
    {
        yield return pause;
        Debug.Log("Dust");
        render.enabled = false;
        dust.SetActive(true);
        //after a couple of seconds, load the cutscene
    }
}
