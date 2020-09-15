using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private GameObject[] guns;
    private int index = 0;
    private bool clicked = false;
    private Vector3 difference;
    private float rot;
    public AudioSource aud;
    public AudioClip[] audioClips;
    private WaitForSeconds arPause;
    private WaitForSeconds srPause;
    private WaitForSeconds scPause;
    public AudioSource audSC;
    public AudioSource audImpact;
    public AudioClip[] impactClips;

    [HideInInspector]
    public bool playAudio = false;

    [HideInInspector]
    public bool playImpact = false;

    // Start is called before the first frame update
    void Start()
    {
        guns = GameObject.FindGameObjectsWithTag("Gun");
        foreach (GameObject g in guns)
        {
            if (g.GetComponent<SpriteRenderer>().enabled)
            {
                g.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //guns[index].SetActive(true);
        guns[index].GetComponent<SpriteRenderer>().enabled = true;

        arPause = new WaitForSeconds(guns[0].GetComponent<AssaultRifle>().startTimeBtwShots);
        srPause = new WaitForSeconds(guns[1].GetComponent<SniperRifle>().startTimeBtwShots);
        scPause = new WaitForSeconds(guns[2].GetComponent<SplashCannon>().startTimeBtwShots);

    }

    // Update is called once per frame
    void Update()
    {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        rot = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
        SwitchGun();
        FlipGun();

        if (playAudio)
        {
            aud.clip = audioClips[index];
            aud.Play();
            playAudio = false;
        }

        if (playImpact)
        {
            audImpact.clip = impactClips[index];
            audImpact.Play();
            playImpact = false;
        }
    }

    void FlipGun()
    {
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            guns[index].GetComponent<SpriteRenderer>().flipY = true;
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            guns[index].GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    void SwitchGun()
    {
        if (index < 2)
        {
            if (Input.GetMouseButtonDown(1))
            {
                index += 1;
                clicked = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                index = 0;
                clicked = true;
            }
        }

        if (clicked == true)
        {
            foreach (GameObject g in guns)
            {
                if (g.GetComponent<SpriteRenderer>().enabled)
                {
                    //g.SetActive(false);
                    g.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            //guns[index].SetActive(true);
            guns[index].GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
