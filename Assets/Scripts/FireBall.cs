using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed;
    private float timer = 8f;
    private int damage = 2;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Instantiate Particles
            collision.GetComponent<PlayerController>().health -= damage;
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("DungeonRoom"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        timer = 8f;
    }
}
