using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthCount : MonoBehaviour
{
    private PlayerController player;
    public Text healthCount;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health.ToString() != healthCount.text)
        {
            healthCount.text = player.health.ToString();
        }
    }
}
