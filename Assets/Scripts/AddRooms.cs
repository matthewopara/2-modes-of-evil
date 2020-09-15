using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddRooms : MonoBehaviour
{
    private RoomTemplates templates;
    public int thresholdIndex;
    public bool isBossRoom = false;
    public GameObject[] gameBossRooms;
    private GameObject bossRoom;
    private float timer = 3f;
    private bool stopTimer = false;
    private string[] roomSides;
    private Transform[] openings;
    private Vector3[] bossSpawnLocations;
    private Quaternion[] bossSpawnRotations;
    private bool hasDoor = false;

    private Vector3 topL;
    private Vector3 downL;
    private Vector3 leftL;
    private Vector3 rightL;

    private Quaternion topR;
    private Quaternion downR;
    private Quaternion leftR;
    private Quaternion rightR;

    void Start()
    {
        bossRoom = gameBossRooms[BossRoomIndex(SceneManager.GetActiveScene().name)];
        topL = new Vector3 (0f, 3f, 0f);
        downL = new Vector3 (0f, -3f, 0f);
        leftL = new Vector3 (-5.7f, 0f, 0f);
        rightL = new Vector3(5.7f, 0f, 0f);

        topR = Quaternion.identity;
        downR = Quaternion.Euler(180f, 0f, 0f);
        leftR = Quaternion.Euler(0f, -90f, 0f);
        rightR = Quaternion.Euler(0f, 90f, 0f);


        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
        //openings = transform.GetComponentInChildren<Transform>().GetComponentsInChildren<Transform>();
        openings = transform.GetChild(thresholdIndex).transform.GetComponentsInChildren<Transform>();

        roomSides = new string[] { "Top", "Down", "Left", "Right" };
        bossSpawnLocations = new Vector3[] { topL, downL, leftL, rightL };
        bossSpawnRotations = new Quaternion[] { topR, downR, leftR, rightR };
    }

    private void Update()
    {
        if (timer <= 0 && !stopTimer)
        {
            if (isBossRoom)
            {
                foreach (string side in roomSides)
                {
                    foreach (Transform door in openings)
                    {
                        if (door.gameObject.name.Contains(side))
                        {
                            hasDoor = true;
                        }
                    }

                    if (!hasDoor)
                    {
                        int index = System.Array.IndexOf(roomSides, side);
                        Instantiate(bossRoom, transform.position + bossSpawnLocations[index], bossSpawnRotations[index]);
                        break;
                    }

                    hasDoor = false;
                }
                stopTimer = true;
            }
        }
        else if (!stopTimer)
        {
            timer -= Time.deltaTime;
        }
    }

    int BossRoomIndex(string scene)
    {
        if (scene.Contains("1"))
        {
            return 0;
        }
        else if (scene.Contains("2"))
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
