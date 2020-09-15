using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    ObjectPool objectPool;
    private GameObject player;
    public float speed;
    private bool canMove = true;
    private Rigidbody2D rb;
    private float startTimer;
    private float timer = 4.5f;
    private bool spinningWeb = false;
    private WaitForSeconds pause;
    private Vector3 offset;
    public GameObject dustEffect;

    // Start is called before the first frame update
    private void OnEnable()
    {
        GameObject dust = (GameObject)Instantiate(dustEffect, transform.position, Quaternion.identity);
        Destroy(dust, 3f);
        player = GameObject.FindGameObjectWithTag("Player");

        objectPool = ObjectPool.Instance;
        pause = new WaitForSeconds(3);
        startTimer = timer;
        offset = new Vector3(0f, -.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !spinningWeb)
        {
            StartCoroutine("SpinWeb");
            spinningWeb = true;
        }
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    IEnumerator SpinWeb()
    {
        canMove = false;
        yield return pause;
        objectPool.SpawnFromPool("SpiderWeb", transform.position + offset, Quaternion.identity);
        canMove = true;
        spinningWeb = false;
        timer = startTimer;
    }

    private void OnDisable()
    {
        spinningWeb = false;
        timer = startTimer;
        canMove = true;
    }
}
