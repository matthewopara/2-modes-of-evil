using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFire : MonoBehaviour
{
    public GameObject fireBall;
    public Vector3 headOffset;
    private GameObject player;
    private DragonBones.UnityArmatureComponent armatureComponent;
    private Vector3 spawnOffset = new Vector3(0f, -0.7f, 0f);
    private SkeletonMovement moveScript;

    private float shootTimer;
    private SpriteRenderer fireHead;
    private WaitForSeconds pause = new WaitForSeconds(1f);
    private bool shootingFire = false;
    private bool inCorrectPos = false;
    public bool shouldAttack = false;
    private bool attacking = false;

    void Awake()
    {
        // Spawn animation is off center by a bit so shift is when spawning and recenter it when walking
        transform.position = transform.position + spawnOffset;
        armatureComponent = GetComponent<DragonBones.UnityArmatureComponent>();
        //armatureComponent.animation.Play("SkeletonSpawn", 1);
        moveScript = GetComponent<SkeletonMovement>();

        fireHead = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        moveScript.canMove = false;
        armatureComponent.animation.Play("SkeletonSpawn", 1);
        shootTimer = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (CanShootFire() && !shootingFire)
        {
            shootingFire = true;
            StartCoroutine(ShootFire());
        }

        if (armatureComponent.animation.isCompleted && !attacking)
        {
            if (!inCorrectPos)
            {
                // Spawn animation is off center by a bit so shift is when spawning and recenter it when walking
                transform.position = transform.position - spawnOffset;
                inCorrectPos = true;
            }
            moveScript.canMove = true;
            armatureComponent.animation.Play("SkeletonWalk", -1);
        }
    }

    bool CanShootFire()
    {
        if (shootTimer <= 0)
        {
            fireHead.enabled = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator ShootFire()
    {
        yield return pause;
        Instantiate(fireBall, transform.position + headOffset, Quaternion.identity);
        fireHead.enabled = false;
        shootTimer = Random.Range(1f, 2f);
        shootingFire = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && shouldAttack)
        {
            attacking = true;
            gameObject.GetComponent<SkeletonMovement>().canMove = false;
            armatureComponent.animation.Play("SkeletonAttack", 1);
            shouldAttack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attacking = false;
        }
    }

    private void OnDisable()
    {
        attacking = false;
        fireHead.enabled = false;
        shootingFire = false;
    }
}
