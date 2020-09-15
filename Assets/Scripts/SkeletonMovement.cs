using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    public float speed;
    private Transform target;
    private Rigidbody2D rb;
    public bool facingOriginal = true;
    private Vector2 scale;
    public int damage;

    private float previousXPos;
    private float currentXPos;
    public bool canMove;

    public GameObject dustEffect;

    public float attackTimer = 0f;

    void OnEnable()
    {
        GameObject dust = (GameObject)Instantiate(dustEffect, transform.position, Quaternion.identity);
        Destroy(dust, 3f);
        currentXPos = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        previousXPos = currentXPos;
        currentXPos = transform.position.x;
        attackTimer -= Time.deltaTime;

        if (canMove)
        {
            Flip(currentXPos, previousXPos);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && attackTimer <= 0)
        {
            other.GetComponent<PlayerController>().health -= damage;
            attackTimer = 1f;
            GetComponent<SkeletonFire>().shouldAttack = true;
        }
    }

    void Flip(float prevPos, float currPos)
    {
        if ((currPos > prevPos && !facingOriginal) || (currPos < prevPos && facingOriginal))
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
}
