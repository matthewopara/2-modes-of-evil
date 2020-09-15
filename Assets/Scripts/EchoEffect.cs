using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    ObjectPool objectPool;
    private float timeBtwSpawns;
    public float starTimeBtwSpawns;
    public bool canEcho = true;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = ObjectPool.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (canEcho)
        {
            if (timeBtwSpawns <= 0)
            {
                objectPool.SpawnFromPool("Echo", transform.position, transform.rotation);
                timeBtwSpawns = starTimeBtwSpawns;
            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
