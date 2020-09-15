using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] spawnArea;
    private bool spawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spawned && collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject col in spawnArea)
            {
                col.SetActive(true);
            }
            spawned = true;
        }
    }
}
