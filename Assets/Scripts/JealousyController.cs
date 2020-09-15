using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class JealousyController : MonoBehaviour
{
    //JumpAttack variables
    public JealousyPathFollower[] paths;
    private JealousyPathFollower currentPath;
    //change canAttack to false when done creating Jealousy Controller
    private WaitUntil doneJumping;
    private WaitForSeconds jumpPause;

    //TailAttack variables
    public GameObject[] tailSpawnPoints;
    private GameObject tailSpawnPoint;
    private WaitForSeconds tailPause;

    [SerializeField]
    public bool isAwake = false;
    private bool canPrep = false;
    private bool canAttack = false;
    private List<string> attackMoves = new List<string> { "JumpAttack", "TailAttack" };
    private int attackNum;

    public GameObject attackPrep;
    private WaitForSeconds idlePause;

    public GameObject idlePath;
    public GameObject idleBoss;
    public GameObject[] bossParts;

    private Vector3 hidePos;
    private bool attacking;

    public GameObject enterOne;
    public GameObject enterTwo;

    public Slider healthBar;
    private EnemyHealth healthScript;

    [HideInInspector]
    public bool isDead = false;

    [HideInInspector]
    public bool battleStarted = false;

    public WakeUpBoss wakeUpBoss;
    public GameObject deathAnim;
    private bool canDie = false;
    private bool isIdle = false;
    private bool startIdleTimer = false;
    private float idleTimer = 6f;
    private bool isPrepped = false;
    private bool startPrepTimer = false;
    private float prepTimer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            Debug.Log(Random.Range(0, attackMoves.Count));
        }
        Debug.Log("attackMove.Count = " + attackMoves.Count);
        doneJumping = new WaitUntil(() => currentPath.endJump);
        jumpPause = new WaitForSeconds(1.5f);
        tailPause = new WaitForSeconds(3.5f);
        idlePause = new WaitForSeconds(2f);
        hidePos = new Vector3(-50f, 0f, 0f);
        healthScript = gameObject.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (idleTimer <= 0f)
        {
            isIdle = false;
        }
        else if (startIdleTimer)
        {
            idleTimer -= Time.deltaTime;
        }

        if (prepTimer <= 0f)
        {
            isPrepped = true;
        }
        else if (startPrepTimer)
        {
            prepTimer -= Time.deltaTime;
        }

        if (isAwake && !battleStarted)
        {
            battleStarted = true;
            StartCoroutine("EnterSequence");
        }

        if (healthBar.value != healthScript.health && !isDead)
        {
            healthBar.value = healthScript.health;
        }

        if (battleStarted)
        {
            if (canPrep)
            {
                StartCoroutine("AttackPrep");
                canPrep = false;
            }

            if (canAttack)
            {
                idleBoss.SetActive(false);
                canAttack = false;
                attackNum = Random.Range(0, attackMoves.Count);
                Debug.Log(attackNum);
                StartCoroutine(attackMoves[attackNum]);
            }
        }

        if (healthScript.health <= 0)
        {
            StartCoroutine("DeathSequence");
        }
    }

    IEnumerator DeathSequence()
    {
        Debug.Log("Death Sequence");
        isDead = true;
        yield return new WaitUntil(() => canDie);
        Debug.Log(canDie);
        deathAnim.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator EnterSequence()
    {
        enterOne.SetActive(true);
        yield return new WaitUntil(() => !enterOne.activeSelf);
        yield return new WaitForSeconds(2f);
        enterTwo.SetActive(true);
        yield return new WaitUntil(() => !enterTwo.activeSelf);
        yield return idlePause;
        idlePath.SetActive(true);

        yield return new WaitUntil(() => !idlePath.activeSelf);
        idleBoss.SetActive(true);

        foreach (GameObject g in bossParts)
        {
            g.transform.position = hidePos;
        }

        wakeUpBoss.playerCanMove = true;
        StartCoroutine("Idle");
    }

    IEnumerator Idle()
    {
        isIdle = true;
        startIdleTimer = true;
        yield return new WaitUntil(() => (!isIdle || isDead));
        startIdleTimer = false;
        
        if (!isDead)
        {
            idleTimer = 6f;
        }
        canPrep = true;
    }

    IEnumerator JumpAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!isDead)
            {
                currentPath = paths[Random.Range(0, paths.Length)];
                currentPath.gameObject.SetActive(true);
                yield return doneJumping;
                Debug.Log("Done");
                currentPath.gameObject.SetActive(false);


                foreach (GameObject g in bossParts)
                {
                    g.transform.position = hidePos;
                }

                yield return jumpPause;
            }
            /*else
            {
                Debug.Log("Is Dead");
                currentPath.gameObject.SetActive(false);
            }*/

            if (isDead)
            {
                canDie = true;
            }
        }

        yield return idlePause;

        if (!isDead)
        {
            idlePath.SetActive(true);

            yield return new WaitUntil(() => !idlePath.activeSelf);
            idleBoss.SetActive(true);

            foreach (GameObject g in bossParts)
            {
                g.transform.position = hidePos;
            }

            StartCoroutine("Idle");
        }
    }

    IEnumerator TailAttack()
    {
        for (int i = 0; i < 6; i++)
        {
            if (!isDead)
            {
                //Particle system dust
                tailSpawnPoint = tailSpawnPoints[Random.Range(0, tailSpawnPoints.Length - 1)];
                tailSpawnPoint.SetActive(true);
                yield return tailPause;
            }
        }
        if (!isDead)
        {
            yield return idlePause;
            idlePath.SetActive(true);

            yield return new WaitUntil(() => !idlePath.activeSelf);
            idleBoss.SetActive(true);

            foreach (GameObject g in bossParts)
            {
                g.transform.position = hidePos;
            }

            StartCoroutine("Idle");
        }

        if (isDead)
        {
            canDie = true;
        }
    }

    IEnumerator AttackPrep()
    {
        isPrepped = false;
        idleBoss.SetActive(false);
        attackPrep.SetActive(true);
        startPrepTimer = true;
        yield return new WaitUntil(() => isPrepped);
        startPrepTimer = false;
        prepTimer = 3f;

        if (!isDead)
        {
            attackPrep.SetActive(false);
            canAttack = true;
        }
        else
        {
            attackPrep.SetActive(false);
            canDie = true;
        }
    }
}
