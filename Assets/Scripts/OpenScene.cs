using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScene : MonoBehaviour
{
    private Animator anim;
    private bool startTimer = false;
    private float timer = 7f;
    private WaitForSeconds pause;
    public PlayerController player;

    [HideInInspector]
    public bool sceneOpening = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            player.enabled = false;
        }
        else
        {
            Debug.Log("Player is Null");
        }
        pause = new WaitForSeconds(5f);
        anim = GetComponent<Animator>();
        System.GC.Collect();
        StartCoroutine("SceneOpen");
        startTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator SceneOpen()
    {
        yield return pause;
        anim.SetTrigger("OpenScene");
        sceneOpening = true;

        if (player != null)
        {
            player.enabled = true;
        }
    }
}
