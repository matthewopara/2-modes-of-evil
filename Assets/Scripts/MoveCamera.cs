using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public int direction;
    private Transform player;
    private Transform room;
    // 1 --> TOP
    // 2 --> BOTTOM
    // 3 --> LEFT
    // 4 --> RIGHT

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        room = gameObject.transform.root;
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (direction == 1 && player.position.y < transform.position.y)
            {
                Camera.main.transform.position = new Vector3(room.position.x, room.position.y, -10);
            }
            if (direction == 2 && player.position.y > transform.position.y)
            {
                Camera.main.transform.position = new Vector3(room.position.x, room.position.y, -10);
            }
            if (direction == 3 && player.position.x > transform.position.x)
            {
                Camera.main.transform.position = new Vector3(room.position.x, room.position.y, -10);
            }
            if (direction == 4 && player.position.x < transform.position.x)
            {
                Camera.main.transform.position = new Vector3(room.position.x, room.position.y, -10);
            }
        }
    }
}
