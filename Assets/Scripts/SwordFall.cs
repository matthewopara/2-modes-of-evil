using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFall : MonoBehaviour
{
    ObjectPool objectPool;
    public Rigidbody2D swordTrailRb;
    public Transform swordTrailT;
    public SpriteRenderer swordTrailRenderer;
    public SpriteRenderer dirtRenderer;
    public SpriteRenderer swordRenderer;
    //public SpriteRenderer trailRenderer;
    public Collider2D circleCollider;
    public float swordSpeed;
    private WaitForSeconds pause1;
    private WaitForSeconds pause2;
    private GameObject swordSplash;
    private ParticleSystem splashParticles;
    public SpriteRenderer swordTarget;
    public Animator swordTargetAnim;
    public Animator swordAnim;
    public Animator dirtAnim;
    private int damage = 2;

    public float spawnDelay;
    private bool spawned = false;
    private float timer = 5f;
    private bool startTimer = false;

    private Vector3 trailPos;

    public AudioSource swordSFX;

    void OnEnable()
    {
        StartCoroutine("Spawn");
        trailPos = swordTrailT.localPosition;
    }

    private void Start()
    {
        objectPool = ObjectPool.Instance;
        pause1 = new WaitForSeconds(3f);
        pause2 = new WaitForSeconds(.2f);
    }

    private void Update()
    {
        if (spawned && swordTrailT != null && swordTrailT.position.y <= transform.position.y)
        {
            swordTrailRenderer.enabled = false;
            swordTrailRenderer.gameObject.GetComponent<EchoEffect>().canEcho = false;
            swordTrailRb.velocity = new Vector3(0f, 0f, 0f);
            swordTrailT.localPosition = trailPos;
            swordSplash = objectPool.SpawnFromPool("SwordSplash", dirtRenderer.gameObject.transform.position, Quaternion.identity);
            splashParticles = swordSplash.GetComponent<ParticleSystem>();
            splashParticles.Play();
            StartCoroutine("DeactivateParticles");
            swordRenderer.enabled = true;
            dirtRenderer.enabled = true;
            swordTarget.enabled = false;
            StartCoroutine("DealDamage");
        }


        if (startTimer)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            swordAnim.SetTrigger("FadeOut");
            dirtAnim.SetTrigger("FadeOut");
            startTimer = false;
            timer = 5f;
        }
    }

    IEnumerator DeactivateParticles()
    {
        yield return pause1;
        splashParticles.Clear();
        swordSplash.SetActive(false);
    }

    IEnumerator DealDamage()
    {
        circleCollider.enabled = true;
        yield return pause2;
        circleCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().health -= damage;
        }
    }

    IEnumerator Spawn()
    {
        swordTargetAnim.SetTrigger("Spawn");
        yield return new WaitForSeconds(spawnDelay);
        swordSFX.Play();
        swordTrailRb.velocity = new Vector3(0f, -swordSpeed, 0f);
        spawned = true;
        startTimer = true;
    }

    private void Disable()
    {
        spawned = false;
        swordRenderer.enabled = false;
        dirtRenderer.enabled = false;
        swordTarget.enabled = true;
        swordTrailRenderer.enabled = true;
        swordTrailRenderer.gameObject.GetComponent<EchoEffect>().canEcho = true;
        //dirtAnim.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
