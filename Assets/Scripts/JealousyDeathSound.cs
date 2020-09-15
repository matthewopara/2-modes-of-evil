using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JealousyDeathSound : MonoBehaviour
{
    private WaitForSeconds pause;
    public AudioSource deadSFXOne;
    public AudioSource deadSFXTwo;
    public AudioSource deadSFXThree;
    // Start is called before the first frame update
    void Start()
    {
        pause = new WaitForSeconds(1.5f);
    }

    private void OnEnable()
    {
        StartCoroutine("DeathSFX");
    }

    IEnumerator DeathSFX()
    {
        deadSFXOne.Play();
        yield return pause;
        deadSFXTwo.Play();
        yield return pause;
        deadSFXThree.Play();
    }
}
