using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoom : MonoBehaviour
{
    private int bossRoomNumber;
    private Scene activeScene;

    private Animator anim;
    private bool startTimer = false;
    private float timer = 1f;

    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
        activeScene = SceneManager.GetActiveScene();

        if (activeScene.name.Contains("Scene1"))
        {
            bossRoomNumber = 1;
        }

        if (activeScene.name.Contains("Scene2"))
        {
            bossRoomNumber = 3;
        }
    }

    void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            SceneManager.LoadScene(bossRoomNumber);
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
