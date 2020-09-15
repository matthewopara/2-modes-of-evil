using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoulMovement : MonoBehaviour
{
    public float speed;
    private Transform target;
    private Rigidbody2D rb;
    private bool facingOriginal = true;
    private Vector2 scale;
    public int damage;
    private Animator anim;

    private float previousXPos;
    private float currentXPos;
    public bool canMove;

    public GameObject dustEffect;

    public float attackTimer = 0f;
    public bool attacking = false;
    private RaycastHit2D wallCheck1;
    private RaycastHit2D wallCheck2;
    private RaycastHit2D wallCheck3;
    private int layerMask = 11;
    private bool walkAroundWall = false;
    private Vector3 rotatedDir;
    private float avoidTimer = .5f;
    public float spawnDelay;
    private bool spawned = false;
    private SpriteRenderer spriteRender;


    void Awake()
    {
        currentXPos = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        previousXPos = currentXPos;
        currentXPos = transform.position.x;
        Flip(currentXPos, previousXPos);
        if (spawned)
        {
            attackTimer -= Time.deltaTime;
        }

        if (canMove && !attacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && attackTimer <= 0)
        {
            attacking = true;
            canMove = false;
            other.GetComponent<PlayerController>().health -= damage;
            anim.SetTrigger("Attack");
            attackTimer = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canMove = true;
            attacking = false;
        }
    }

    void Flip(float prevPos, float currPos)
    {
        if ((currPos > prevPos && !facingOriginal) || currPos < prevPos && facingOriginal)
        {
            scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            facingOriginal = !facingOriginal;
        }
    }

    void CanMove()
    {
        canMove = true;
    }

    void NotAttacking()
    {
        attacking = false;
    }

    private void OnDisable()
    {
        attacking = false;
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        spriteRender.enabled = true;
        GameObject dust = (GameObject)Instantiate(dustEffect, transform.position, Quaternion.identity);
        Destroy(dust, 3f);
        anim.SetTrigger("Spawned");
        canMove = false;
        spawned = true;
    }
}
