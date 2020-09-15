using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    private PlayerController player;
    private bool playerSlowed = false;
    private float lifeTime = 8f;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            lifeTime = 8f;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.moveSpeed /= 3f;
            playerSlowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerSlowed)
        {
            player.moveSpeed *= 3f;
            playerSlowed = false;
        }
    }

    private void OnDestroy()
    {
        if (playerSlowed)
        {
            player.moveSpeed *= 3f;
        }
    }

    private void OnDisable()
    {
        if (playerSlowed)
        {
            player.moveSpeed *= 3;
            playerSlowed = false;
        }
    }
}
