using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAppear : MonoBehaviour
{
    public bool vengeance;
    public bool jealousy;
    public GameObject boss;
    private Image image;
    private bool canAppear = true;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAppear)
        {
            StartCoroutine("ControlHealthBar");
            canAppear = false;
        }
    }

    IEnumerator ControlHealthBar()
    {
        image.CrossFadeAlpha(0f, 0f, false);
        if (vengeance)
        {
            yield return new WaitUntil(() => boss.GetComponent<Vengeance>().battleStarted);
        }
        else if (jealousy)
        {
            yield return new WaitUntil(() => boss.GetComponent<JealousyController>().battleStarted);
        }
        image.CrossFadeAlpha(1f, .5f, false);
        if (vengeance)
        {
            yield return new WaitUntil(() => boss.GetComponent<Vengeance>().healthBar.value <= 0f);
        }
        else if (jealousy)
        {
            yield return new WaitUntil(() => boss.GetComponent<JealousyController>().healthBar.value <= 0f);
        }
        image.CrossFadeAlpha(0f, 1f, false);
    }
}
