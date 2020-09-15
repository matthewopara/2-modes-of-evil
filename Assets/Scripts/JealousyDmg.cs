using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JealousyDmg : MonoBehaviour
{
    public EnemyHealth bossHealth;
    public Rigidbody2D playerRb;
    public PlayerController playerScript;
    private WaitForSeconds pause;
    private JealousyController jealousy;
    private Vector3 resetPlayer;

    [HideInInspector]
    public bool canDealDmg = true;

    private void Start()
    {
        pause = new WaitForSeconds(.4f);
        jealousy = GameObject.Find("JealousyController").GetComponent<JealousyController>();
        resetPlayer = new Vector3(0f, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Bullet"))
        {
            if (collision.gameObject.name == "AssaultRifleBullet(Clone)")
            {
                bossHealth.health -= 1;
            }
            else if (collision.gameObject.name == "SplashCannonBullet(Clone)")
            {
                bossHealth.health -= 3;
            }
            else if (collision.gameObject.name == "SniperRifleBullet(Clone)")
            {
                bossHealth.health -= 9;
            }
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Player") && canDealDmg && !jealousy.isDead)
        {
            playerScript.health -= 3;
            StartCoroutine("PausePlayer");
            playerRb.AddForce(new Vector3(250f, -250f, 0f));
        }
    }

    IEnumerator PausePlayer()
    {
        playerScript.enabled = false;
        yield return pause;
        playerRb.velocity = resetPlayer;
        playerScript.enabled = true;
    }
}
