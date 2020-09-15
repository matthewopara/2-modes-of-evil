using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpBoss : MonoBehaviour
{
    public GameObject boss;
    private float timer = 1.5f;
    private bool startTimer = false;
    private WakeUpBoss bossScript;
    public CameraFollowPlayer cameraScript;
    private PlayerController player;
    public AssaultRifle ar;
    public SniperRifle sr;
    public SplashCannon sc;
    private WaitUntil waitTime;
    public bool playerCanMove;
    public Renderer closedRoomSR;
    public Collider2D closedRoomPC;
    public Renderer openRoomSR;
    public Collider2D openRoomPC;
    public FollowPlayer followPlayerScript;
    private bool preBoss = true;

    void Start()
    {
        bossScript = GetComponent<WakeUpBoss>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        waitTime = new WaitUntil(() => playerCanMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            if (boss.name == "Vengeance")
            {
                boss.GetComponent<Vengeance>().isAwake = true;
            }
            else if (boss.name == "JealousyController")
            {
                boss.GetComponent<JealousyController>().isAwake = true;
            }
            bossScript.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (preBoss)
            {
                startTimer = true;
                cameraScript.gameStart = true;
                closedRoomSR.enabled = true;
                closedRoomPC.enabled = true;
                openRoomSR.enabled = false;
                openRoomPC.enabled = false;
                followPlayerScript.bossLevel = true;
                StartCoroutine("FreezePlayer");
                preBoss = false;
            }
        }
    }

    IEnumerator FreezePlayer()
    {
        player.enabled = false;
        ar.enabled = false;
        sr.enabled = false;
        sc.enabled = false;
        yield return waitTime;
        player.enabled = true;
        ar.enabled = true;
        sr.enabled = true;
        sc.enabled = true;
    }
}
