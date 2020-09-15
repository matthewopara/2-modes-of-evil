using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vengeance : MonoBehaviour
{
    ObjectPool objectPool;
    private EnemyHealth healthScript;
    public GameObject player;
    private PlayerController playerScript;
    private Rigidbody2D playerRb;
    private bool canWalk = false;
    public float walkSpeed;
    public bool isAwake;
    public bool battleStarted = false;
    private bool canAttack = false;
    private float startAttackTimer = 6f;
    private float attackTimer;
    private bool attacking = false;
    public PolygonCollider2D dmgCollider;

    private Vector3 playerPos;
    private Rigidbody2D rb;
    private WaitUntil atDestination;
    private DragonBones.UnityArmatureComponent armatureComponent;
    private WaitUntil animationDone;

    private WaitForSeconds fireWaitTime;
    public GameObject fireBall;
    private Vector3 firePosOffset;

    private Vector3 difference;
    private float rot;

    private WaitForSeconds fireTimer;
    public SpriteRenderer fireHand;
    private GameObject fireInstance;

    private List<string> attackMoves = new List<string> { "SwordRain", "FireFistAttack" };
    private bool hasDash = false;
    private bool finalStage = false;
    private bool dashing = false;
    public bool facingOriginal = true;
    private Vector2 scale;
    private float previousXPos;
    private float currentXPos;

    public AttackTrigger attackTrigger;
    public DashDamage dashDamage;
    public ForceTrigger forceTrigger;
    public Slider healthBar;
    private float maxHealth;
    private bool isDead = false;
    private bool touchingWall = false;
    private Vector3 resetWallBug;

    public WakeUpBoss wakeUpBoss;
    private WaitForSeconds dashDmgWaitTime;
    public float knockBackForce;

    private List<Vector2> swordDropLoc;

    public AudioClip[] vengeanceTalk;
    public AudioSource talkAudio;

    public AudioClip[] swordSounds;
    public AudioSource swordAudio;
    public AudioSource deathAudio;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = ObjectPool.Instance;
        rb = GetComponent<Rigidbody2D>();
        fireWaitTime = new WaitForSeconds(3f);
        atDestination = new WaitUntil(() => ((playerPos == transform.position) || touchingWall || forceTrigger.canApplyForce) && !isDead);
        armatureComponent = GetComponent<DragonBones.UnityArmatureComponent>();
        armatureComponent.animation.Play("StartAnimation", -1);
        animationDone = new WaitUntil(() => armatureComponent.animation.isCompleted);
        firePosOffset = new Vector3(0f, .82f, 0f);
        fireTimer = new WaitForSeconds(.2f);

        currentXPos = transform.position.x;
        healthScript = GetComponent<EnemyHealth>();
        maxHealth = healthScript.health;
        attackTimer = startAttackTimer;
        resetWallBug = new Vector3(0f, 0f, 0f);

        playerScript = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody2D>();
        dashDmgWaitTime = new WaitForSeconds(.3f);

        dmgCollider.enabled = false;
        swordDropLoc = new List<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        previousXPos = currentXPos;
        currentXPos = transform.position.x;
        rb.velocity = resetWallBug;


        if (healthBar.value != healthScript.health && !isDead)
        {
            healthBar.value = healthScript.health;
        }

        if (isAwake && !battleStarted && !isDead)
        {
            dmgCollider.enabled = true;
            StartCoroutine("WakeUp");
        }

        if (canWalk && !isDead)
        {
            Flip(currentXPos, previousXPos);
            transform.position = Vector2.MoveTowards(transform.position, playerPos, walkSpeed * Time.deltaTime);
        }

        if (canWalk && transform.position == playerPos && !dashing && !isDead)
        {
            playerPos = player.transform.position;
        }

        if (!attacking && canWalk && !isDead)
        {
            attackTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0f && !isDead)
        {
            canAttack = true;
        }

        if (canAttack && !isDead)
        {
            Debug.Log("Is Attacking");
            canAttack = false;
            canWalk = false;
            attackTimer = startAttackTimer;
            attacking = true;
            StartCoroutine(attackMoves[Random.Range(0, attackMoves.Count)]);
        }

        if (attackTrigger.canSwingWeapon && !attacking && !isDead)
        {
            StartCoroutine("SwingWeapon");
            attackTrigger.canSwingWeapon = false;
        }

        CheckHealth(healthScript.health);
    }

    void CheckHealth(float health)
    {
        if (health <= maxHealth / 1.7f && !hasDash && !isDead)
        {
            for (int i = 0; i < 4; i++)
            {
                attackMoves.Add("DashAttack");
            }
            startAttackTimer = 3f;
            hasDash = true;
        }

        if (health <= maxHealth / 3 && !finalStage && !isDead)
        {
            attackMoves.Remove("FireFistAttack");
            attackMoves.Remove("SwordRain");
            startAttackTimer = 1f;
            finalStage = true;
        }

        if (health <= 0f && !isDead)
        {
            Debug.Log("Died");
            StartCoroutine("Death");
            isDead = true;
        }
    }
    void WalkToPlayer()
    {
        playerPos = player.transform.position;
        canWalk = true;
    }

    void Flip(float prevPos, float currPos)
    {
        if (((currPos > prevPos && facingOriginal) || (currPos < prevPos && !facingOriginal)) && !touchingWall)
        {
            scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            facingOriginal = !facingOriginal;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DungeonRoom"))
        {
            playerPos = player.transform.position;
            touchingWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DungeonRoom"))
        {
            touchingWall = false;
        }
    }

    IEnumerator SwingWeapon()
    {
        Flip(player.transform.position.x, transform.position.x);
        canWalk = false;
        attacking = true;
        armatureComponent.animation.Play("AttackAnimation", 1);
        swordAudio.clip = swordSounds[0];
        swordAudio.Play();
        DealDamage();
        ApplyForce();
        yield return animationDone;
        playerScript.enabled = true;
        armatureComponent.animation.Play("WalkAnimation", -1);
        WalkToPlayer();
        attacking = false;
        attackTimer = startAttackTimer;
    }

    IEnumerator WakeUp()
    {
        armatureComponent.animation.Reset();
        armatureComponent.animation.Play("AwakeAnimation", 1);
        battleStarted = true;
        //play vengeance's voice lines
        yield return new WaitForSeconds(6f);
        talkAudio.clip = vengeanceTalk[0];
        talkAudio.Play();
        //wait correct time then play sword sfx
        yield return animationDone;
        wakeUpBoss.playerCanMove = true;
        armatureComponent.animation.Play("WalkAnimation", -1);
        WalkToPlayer();
    }

    IEnumerator FireFistAttack()
    {
        armatureComponent.animation.Play("FireFistAnimation", -1);
        fireHand.enabled = true;
        WalkToPlayer();
        
        yield return fireWaitTime;

        for (int i = 0; i < 20; i++)
        {
            if (player.transform.position == transform.position)
            {
                WalkToPlayer();
            }
            difference = player.transform.position - transform.position;
            rot = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            //fireInstance = (GameObject)Instantiate(fireBall, transform.position + firePosOffset, Quaternion.Euler(0f, 0f, Random.Range(rot - 35f, rot + 35f)));
            objectPool.SpawnFromPool("FireBall", transform.position + firePosOffset, Quaternion.Euler(0f, 0f, Random.Range(rot - 35f, rot + 35f)));
            yield return fireTimer;
        }

        fireHand.enabled = false;
        armatureComponent.animation.Play("WalkAnimation", -1);
        WalkToPlayer();
        attacking = false;
    }

    IEnumerator DashAttack()
    {
        armatureComponent.animation.Play("DashPrepAnimation", 1);
        yield return animationDone;
        armatureComponent.animation.Play("DashAnimation", -1);
        dashing = true;
        WalkToPlayer();
        walkSpeed = walkSpeed * 9f;
        yield return atDestination;
        canWalk = false;
        walkSpeed = walkSpeed / 9f;
        dashing = false;
        armatureComponent.animation.Play("DashAttackAnimation", 1);
        DealDamage();
        swordAudio.clip = swordSounds[0];
        swordAudio.Play();
        yield return dashDmgWaitTime;
        DealDamage();
        swordAudio.clip = swordSounds[0];
        swordAudio.Play();
        yield return dashDmgWaitTime;
        DealDamage();
        swordAudio.clip = swordSounds[0];
        swordAudio.Play();
        ApplyForce();
        yield return animationDone;
        playerScript.enabled = true;
        armatureComponent.animation.Play("WalkAnimation", -1);
        WalkToPlayer();
        attacking = false;
    }

    IEnumerator SwordRain()
    {
        armatureComponent.animation.Play("FireFistAnimation", -1);
        WalkToPlayer();

        yield return fireWaitTime;

        for (int i = 0; i < 15; i++)
        {
            swordDropLoc.Add(new Vector2(Random.Range(-31, -4), Random.Range(-6, 10)));
        }

        for (int i = 0; i < 15; i++)
        {
            objectPool.SpawnFromPool("FallingSword", swordDropLoc[i], Quaternion.Euler(0f, 0f, 126.746f));
            yield return new WaitForSeconds(.3f);
        }

        attacking = false;
        armatureComponent.animation.Play("WalkAnimation", -1);

    }
    // For dash attack, make a physics2d.overlapbox/overlapboxall, get player thats inside box deal damage 3 times using coroutine

    IEnumerator Death()
    {
        canWalk = false;
        armatureComponent.animation.Reset();
        armatureComponent.animation.Play("DeathAnimation", 1);
        deathAudio.Play();
        yield return animationDone;
        armatureComponent.animation.GotoAndPlayByProgress("DeathAnimation", .7f, 1);
        GetComponent<EnemyHealth>().canDie = true;
    }

    void DealDamage()
    {
        if (dashDamage.canDashDmg)
        {
            playerScript.health -= 2;
        }
    }

    void ApplyForce()
    {
        if (forceTrigger.canApplyForce)
        {
            playerScript.enabled = false;
            if (facingOriginal)
            {
                playerRb.AddForce(Vector2.right * knockBackForce);
            }
            else
            {
                playerRb.AddForce(Vector2.left * knockBackForce);
            }
        }
    }
}
