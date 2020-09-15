using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossStatusChecker : MonoBehaviour
{
    public GameObject boss;
    private EnemyHealth bossHealth;
    private bool startTimer = false;
    private float timer = 10f;
    private Animator anim;
    private bool canRun = true;
    public int index;
    public bool jealousyBoss;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
        bossHealth = boss.GetComponent<EnemyHealth>();

        if (jealousyBoss)
        {
            timer = 13f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth.health <= 0)
        {
            startTimer = true;
        }

        if (startTimer)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && canRun)
        {
            StartCoroutine("NewSceneTransition");
            canRun = false;
        }
    }

    IEnumerator NewSceneTransition()
    {
        anim.SetTrigger("EndScene");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(index);
    }
}
