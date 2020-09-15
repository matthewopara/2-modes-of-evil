using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public int nextSceneIndex;
    private Animator anim;
    private bool startTimer = false;
    private float timer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
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
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("EndScene");
            startTimer = true;
        }
    }
}
