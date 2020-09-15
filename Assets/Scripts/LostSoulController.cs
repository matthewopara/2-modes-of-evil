using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoulController : MonoBehaviour
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

    public float attackTimer = 0f;
    public bool attacking = false;

    private RaycastHit2D sensor;
    private Vector3 sidestep;
    private bool burrowing;

    void Awake()
    {
        currentXPos = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("Spawned");
        canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        previousXPos = currentXPos;
        currentXPos = transform.position.x;
        Flip(currentXPos, previousXPos);
        attackTimer -= Time.deltaTime;

        if (canMove && !attacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            sensor = Physics2D.Raycast(transform.position, target.position, 1f);
            if (sensor.collider.gameObject.CompareTag("DungeonRoom"))
            {
                //Play burrow animation
                burrowing = true;
            }
            if (!sensor.collider.gameObject.CompareTag("DungeonRoom") && burrowing)
            {
                //player spawn animation
                burrowing = false;
            }
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
}
