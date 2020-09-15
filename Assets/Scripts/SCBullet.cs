using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCBullet : MonoBehaviour
{
    ObjectPool objectPool;
    public float speed;
    public float lifeTime;
    private Collider2D[] hitColliders;
    public float dmgRadius;
    private GunController gun;
    // Start is called before the first frame update
    void OnEnable()
    {
        objectPool = ObjectPool.Instance;
        Invoke("DisableProjectile", lifeTime);
        gun = GameObject.Find("Guns").GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.layer == 8 || collision.gameObject.CompareTag("Jealousy")) && Time.time > 4)
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, dmgRadius);

            foreach (Collider2D col in hitColliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    col.gameObject.GetComponent<EnemyHealth>().health -= 3;
                }
            }

            objectPool.SpawnFromPool("Explosion", transform.position, Quaternion.identity);
            gun.playImpact = true;
            CancelInvoke();
            gameObject.SetActive(false);
        }
    }

    void DisableProjectile()
    {
        objectPool.SpawnFromPool("Explosion", transform.position, Quaternion.identity);
        gun.playImpact = true;
        gameObject.SetActive(false);
    }
}
