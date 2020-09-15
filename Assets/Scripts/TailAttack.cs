using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailAttack : MonoBehaviour
{
    public Animator anim;
    //public Collider2D dmgCollider;
    public PlayerController player;
    private WaitForSeconds pause;
    private JealousyDmg dmgScript;
    public AudioSource tailSFX;

    private void OnEnable()
    {
        anim.SetTrigger("Attack");
    }

    private void Start()
    {
        pause = new WaitForSeconds(.2f);
        dmgScript = gameObject.GetComponent<JealousyDmg>();
        dmgScript.canDealDmg = false;
    }

    IEnumerator DisableTail()
    {
        yield return pause;
        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator DealDamage()
    {
        dmgScript.canDealDmg = true;
        tailSFX.Play();
        yield return pause;
        dmgScript.canDealDmg = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.health -= 2;
            Debug.Log("Damage");
        }
    }
}
