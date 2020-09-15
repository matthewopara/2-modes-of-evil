using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivateSys : MonoBehaviour
{
    public ParticleSystem partSys;
    public float lifeTime;

    private void OnEnable()
    {
        partSys.Play();
        StartCoroutine("DeActivate");
    }

    IEnumerator DeActivate()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
