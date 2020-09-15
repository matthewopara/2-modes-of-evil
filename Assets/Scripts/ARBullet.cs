using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARBullet : MonoBehaviour
{
    ObjectPool objectPool;
    public float speed;
    public float lifeTime;
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
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Jealousy")) && Time.time > 4)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyHealth>().health -= 1;
            }
            //Instantiate particle effect
            //Destroy(gameObject);
            objectPool.SpawnFromPool("ARSplash", transform.position, transform.rotation);
            gun.playImpact = true;
            CancelInvoke();
            gameObject.SetActive(false);
        }

        if (collision.gameObject.layer == 8 && Time.time > 4)
        {
            //Instantiate particle effect
            //Destroy(gameObject);
            objectPool.SpawnFromPool("ARSplash", transform.position, transform.rotation);
            gun.playImpact = true;
            CancelInvoke();
            gameObject.SetActive(false);
        }
    }

    void DisableProjectile()
    {
        gameObject.SetActive(false);
    }
}
