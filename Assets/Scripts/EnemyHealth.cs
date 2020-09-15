using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    private int maxHealth;
    private bool isDead = false;
    public GameObject[] blood;
    public float r;
    public float g;
    public float b;
    public bool bleeds;
    public GameObject remains;
    private Vector2 scale;
    public bool isBoss;
    public bool canDie = true;
    public bool isSkeleton;
    public bool isVengeance;
    public bool isJealousy;

    void Awake()
    {
        if (isBoss)
        {
            canDie = false;
        }
        maxHealth = health;
    }

    private void OnEnable()
    {
        isDead = false;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (bleeds && health <= 0 && !isDead && canDie)
        {
            GameObject bloodSplatter = (GameObject)Instantiate(blood[Random.Range(0, blood.Length - 1)], transform.position, Quaternion.identity);
            bloodSplatter.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
            gameObject.SetActive(false);
            isDead = true;
        }
        else if (health <= 0 && !isDead && canDie)
        {
            if (isSkeleton)
            {
                GameObject deadBody = (GameObject)Instantiate(remains, transform.position, Quaternion.identity);
                if (!GetComponent<SkeletonMovement>().facingOriginal)
                {
                    scale = deadBody.transform.localScale;
                    scale.x *= -1;
                    deadBody.transform.localScale = scale;
                }
                gameObject.SetActive(false);
                isDead = true;
            }
            else if (isVengeance)
            {
                GameObject deadBody = (GameObject)Instantiate(remains, transform.position, Quaternion.identity);
                if (!GetComponent<Vengeance>().facingOriginal)
                {
                    scale = deadBody.transform.localScale;
                    scale.x *= -1;
                    deadBody.transform.localScale = scale;
                }
                gameObject.SetActive(false);
                isDead = true;
            }
            else if (!isJealousy)
            {
                gameObject.SetActive(false);
                isDead = true;
            }
        }
    }
}
