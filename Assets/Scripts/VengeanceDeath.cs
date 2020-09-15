using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengeanceDeath : MonoBehaviour
{
    public float timeBeforeColorChange;
    public float timeBeforeFall;
    public float timeBeforeCollide;
    private SpriteRenderer sprite;
    private CapsuleCollider2D boxCollider;
    private PolygonCollider2D polyCollider;
    private Rigidbody2D rb;
    private float g = 0.944f;
    private float b = .504717f;
    public bool changeColor;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        polyCollider = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeColorChange -= Time.deltaTime;
        timeBeforeFall -= Time.deltaTime;
        timeBeforeCollide -= Time.deltaTime;

        if (timeBeforeColorChange <= 0f && changeColor)
        {
            sprite.color = new Color(1f, g, b);
        }

        if (timeBeforeFall <= 0f)
        {
            rb.isKinematic = false;
        }

        if (timeBeforeCollide <= 0f)
        {
            if (polyCollider == null)
            {
                boxCollider.enabled = true;
            }
            else
            {
                polyCollider.enabled = true;
            }
        }
    }
}
