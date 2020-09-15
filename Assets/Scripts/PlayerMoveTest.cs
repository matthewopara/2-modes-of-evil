using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector3(2.5f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
