using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deceit : MonoBehaviour
{
    ObjectPool objectPool;
    public Rigidbody2D rb;
    public GameObject player;
    private Vector3 playerPos;
    public float speed;
    private bool canMove = true;
    private Vector3 target;
    private bool walking = false;

    private WaitForSeconds timeBtwWebShots;

    private void Start()
    {
        objectPool = ObjectPool.Instance;
        timeBtwWebShots = new WaitForSeconds(.3f);
    }

    private void Update()
    {
        playerPos = player.transform.position;

        while (canMove)
        {
            Walk();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("ShootWebs");
        }
    }

    void Walk()
    {
        if (!walking)
        {
            ChooseRandLocation();
            walking = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position == target)
            {
                walking = false;
            }
        }
    }

    void ChooseRandLocation()
    {
        while (true)
        {
            target = new Vector3(Random.Range(-7f, 7f), Random.Range(-4f, 4f), 0f);
            if (!((transform.position.y - target.y) / (transform.position.x - target.x) < 2f))
            {
                break;
            }
        }
    }

    IEnumerator ShootWebs()
    {
        for (int i = 0; i < 9; i++)
        {
            //Spawn small webshots
            //objectPool.SpawnFromPool(   );
            yield return timeBtwWebShots;
        }

        //Spawn large webshot
    }
}
