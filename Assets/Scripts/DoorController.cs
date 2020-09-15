using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public SpriteRenderer doorSprite;
    public Collider2D doorCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorSprite.enabled = true;
            doorCollider.enabled = true;
        }
    }
}
