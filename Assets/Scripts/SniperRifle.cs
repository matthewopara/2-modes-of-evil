using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : MonoBehaviour
{
    //public GameObject bullet;
    ObjectPool objectPool;
    public GameObject shotPoint;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private GunController gunController;

    private void Start()
    {
        objectPool = ObjectPool.Instance;
        gunController = gameObject.GetComponentInParent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && GetComponent<SpriteRenderer>().enabled)
            {
                //Instantiate(bullet, shotPoint.transform.position, shotPoint.transform.rotation);
                objectPool.SpawnFromPool("SRBullet", shotPoint.transform.position, shotPoint.transform.rotation);
                gunController.playAudio = true;
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
