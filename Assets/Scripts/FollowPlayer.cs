using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;
    public bool bossLevel;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0f, 0f, -10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!bossLevel)
        {
            transform.position = player.position + offset;
        }
    }
}
