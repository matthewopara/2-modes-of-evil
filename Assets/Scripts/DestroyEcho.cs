using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEcho : MonoBehaviour
{
    private WaitForSeconds pause;

    private void Awake()
    {
        pause = new WaitForSeconds(.5f);
    }
    private void OnEnable()
    {
        StartCoroutine("Deactivate");
    }

    IEnumerator Deactivate()
    {
        yield return pause;
        gameObject.SetActive(false);
    }
}
