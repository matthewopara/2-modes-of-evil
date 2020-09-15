using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallTarget : MonoBehaviour
{
    public float speed;
    private Vector3 difference;
    private float rot;
    private Transform player;
    private float timer = 8f;
    private int damage = 2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        difference = player.position - transform.position;
        rot = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
    }

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
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("DungeonRoom"))
        {
            Destroy(gameObject);
        }
    }
}
