using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyNode : MonoBehaviour
{
    public int order;
    private SpriteRenderer[] bossRenderers;
    private bool notHit = true;

    private void OnEnable()
    {
        notHit = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jealousy"))
        {
            if (collision.gameObject.name != "Head")
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order;
            }
            else
            {
                bossRenderers = collision.gameObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer r in bossRenderers)
                {
                    r.sortingOrder = order;
                    Debug.Log(r.sortingOrder);
                }
            }
        }
    }
}
