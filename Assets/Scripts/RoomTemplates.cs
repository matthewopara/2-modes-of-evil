using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBossRoom = false;

    void Update()
    {
        if (waitTime <= 0)
        {
            if (!spawnedBossRoom)
            {
                rooms[rooms.Count - 1].GetComponent<AddRooms>().isBossRoom = true;
                spawnedBossRoom = true;
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
