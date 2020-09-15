using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 initialPos;
    private Vector3 endPos;
    public Transform bossRoom;
    private Rigidbody2D rb;
    public bool gameStart = false;
    private float lerpTimer = 0f;
    private bool atDestination = false;
    public Vector3 cameraOffset;
    private Vector3 zeroVector;
    private bool begin = false;
    public FollowPlayer camFollow;
    public WakeUpBoss wakeUpBoss;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        endPos = bossRoom.position + cameraOffset;
        zeroVector = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!gameStart)
        {
            transform.position = player.position + cameraOffset;
        }*/
    }

    void FixedUpdate()
    {
        if (gameStart && !begin)
        {
            if (initialPos == zeroVector)
            {
                initialPos = transform.position; 
            }

            if (!(1 - lerpTimer <= Time.fixedDeltaTime / 2))
            {
                lerpTimer += Time.fixedDeltaTime / 2;
            }
            else
            {
                lerpTimer += 1 - lerpTimer;
            }

            transform.position = Vector3.Lerp(initialPos, endPos, lerpTimer);
            if (transform.position == endPos)
            {
                begin = true;
            }
        }
        if (begin)
        {
            //use enumerator using waitTime from WakeUpBoss
            StartCoroutine("Pause");
        }
    }

    IEnumerator Pause()
    {
        //yield return new WaitForSeconds(10);
        yield return new WaitUntil(() => wakeUpBoss.playerCanMove);
        camFollow.bossLevel = false;
    }
}
