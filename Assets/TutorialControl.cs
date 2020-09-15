using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    public GameObject tip;
    public Animator tipAnim;
    private WaitForSeconds pause;

    private void Start()
    {
        pause = new WaitForSeconds(.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tip.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("Hide");
        }
    }

    IEnumerator Hide()
    {
        tipAnim.SetTrigger("HideTip");
        yield return pause;
        tip.SetActive(false);
    }
}