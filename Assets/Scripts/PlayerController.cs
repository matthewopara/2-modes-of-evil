using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health = 20;
    public float moveSpeed;
    private Rigidbody2D rb;
    private float directionX;
    private float directionY;
    private bool facingRight = true;
    private Vector3 resetWallBug;

    private Vector2 moveInput;
    private Vector2 playerPos;

    private Animator anim;
    public bool forceApplied = false;
    private bool isDead = false;
    public GameObject guns;
    private bool canMove = true;
    private Animator reloadScene;
    public AudioSource deathSFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        resetWallBug = new Vector3(0f, 0f, 0f);
        reloadScene = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(health);
        //playerPos = transform.position;
        rb.velocity = resetWallBug;
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Flip(rb.velocity.x);

        if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        //Debug.Log(health);

        if (health <= 0 && !isDead)
        {
            StartCoroutine("Death");
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            transform.Translate(moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
        }
        //rb.MovePosition(playerPos + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

    }

    void Flip(float speed)
    {
        if (speed > 0 && !facingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            facingRight = !facingRight;
        }
        if (speed < 0 && facingRight)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            facingRight = !facingRight;
        }
    }

    IEnumerator Death()
    {
        Debug.Log("Dead");
        anim.SetTrigger("Dead");
        deathSFX.Play();
        isDead = true;
        guns.SetActive(false);
        canMove = false;
        yield return new WaitForSeconds(3f);
        reloadScene.SetTrigger("EndScene");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}