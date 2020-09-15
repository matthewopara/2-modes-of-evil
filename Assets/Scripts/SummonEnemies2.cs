using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummonEnemies2 : MonoBehaviour
{
    ObjectPool objectPool;
    //public GameObject[] gameEnemies;
    //private List<GameObject> sceneEnemies = new List<GameObject>();
    private SpriteRenderer closedRoomRenderer;
    private PolygonCollider2D closedRoomCollider;
    public string[] gameEnemies;
    private List<string> sceneEnemies = new List<string>();
    private int enemiesPerWave = 5;
    private Vector3 buffer = new Vector3(.5f, .5f, 0f);
    private float roomEdgeX = 4.5f;
    private float roomEdgeY = 3.23f;
    public bool isEntryRoom;

    private bool firstWaveSpawned = false;
    private bool secondWaveSpawned = false;
    private List<GameObject> wave = new List<GameObject>();
    private int waveCount;

    private bool canSummon;

    private Scene currentScene;
    private string sceneName;

    private void Start()
    {
        objectPool = ObjectPool.Instance;

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        ChooseEnemies();

        closedRoomRenderer = GetComponentInChildren<SpriteRenderer>();
        closedRoomCollider = GetComponentInChildren<PolygonCollider2D>();
    }

    private void Update()
    {
        // Remove enemy from the wave list when it dies
        for (int i = wave.Count - 1; i >= 0; i--)
        {
            if (!wave[i].activeSelf)
            {
                wave.RemoveAt(i);
            }
        }

        // If number of enemies left alive is 1 or less, instantiate 5 more enemies, then don't summon anymore
        if (wave.Count <= 1 && firstWaveSpawned && !secondWaveSpawned)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                //Instantiate(sceneEnemies[Random.Range(0, sceneEnemies.Count)], gameObject.transform.parent.parent.transform.position + new Vector3(Random.Range(-roomEdgeX, roomEdgeX), Random.Range(-roomEdgeY, roomEdgeY), 0), Quaternion.identity);
                GameObject spawnedEnemy = objectPool.SpawnFromPool(sceneEnemies[Random.Range(0, sceneEnemies.Count)], gameObject.transform.parent.parent.transform.position + new Vector3(Random.Range(-roomEdgeX, roomEdgeX), Random.Range(-roomEdgeY, roomEdgeY), 0), Quaternion.identity);
                wave.Add(spawnedEnemy);
            }

            secondWaveSpawned = true;
        }

        if (wave.Count == 0 && secondWaveSpawned)
        {
            closedRoomRenderer.enabled = false;
            closedRoomCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckCondition(GetComponent<MoveCamera>().direction, GameObject.FindGameObjectWithTag("Player").transform);
        if (canSummon)
        {
            if (collision.gameObject.CompareTag("Player") && !firstWaveSpawned)
            {
                closedRoomRenderer.enabled = true;
                closedRoomCollider.enabled = true;
                // if first wave has not spawned, spawn first wave and set first wave spawned to true
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    //GameObject spawnedEnemy = (GameObject)Instantiate(sceneEnemies[Random.Range(0, sceneEnemies.Count)], gameObject.transform.parent.parent.transform.position + new Vector3(Random.Range(-roomEdgeX, roomEdgeX), Random.Range(-roomEdgeY, roomEdgeY), 0), Quaternion.identity);
                    GameObject spawnedEnemy = objectPool.SpawnFromPool(sceneEnemies[Random.Range(0, sceneEnemies.Count)], gameObject.transform.parent.parent.transform.position + new Vector3(Random.Range(-roomEdgeX, roomEdgeX), Random.Range(-roomEdgeY, roomEdgeY), 0), Quaternion.identity);
                    wave.Add(spawnedEnemy);
                }
                Debug.Log("Running");

                SummonEnemies2[] summonEnemies = transform.parent.GetComponentsInChildren<SummonEnemies2>();
                foreach (SummonEnemies2 c in summonEnemies)
                {
                    c.firstWaveSpawned = true;

                    if (c.gameObject.name != name)
                    {
                        c.secondWaveSpawned = true;
                    }
                }
            }
        }
    }

    void CheckCondition(int direction, Transform player)
    {
        if (direction == 1 && player.position.y < transform.position.y)
        {
            canSummon = true;
        }
        if (direction == 2 && player.position.y > transform.position.y)
        {
            canSummon = true;
        }
        if (direction == 3 && player.position.x > transform.position.x)
        {
            canSummon = true;
        }
        if (direction == 4 && player.position.x < transform.position.x)
        {
            canSummon = true;
        }
    }

    void ChooseEnemies()
    {
        if (sceneName == "Scene1")
        {
            foreach (string gameEnemy in gameEnemies)
            {
                if (gameEnemy == "LostSoul" || gameEnemy == "Skeleton")
                {
                    sceneEnemies.Add(gameEnemy);
                }
            }
        }

        if (sceneName == "Scene2")
        {
            foreach (string gameEnemy in gameEnemies)
            {
                if (gameEnemy == "LostSoul" || gameEnemy == "Spider")
                {
                    sceneEnemies.Add(gameEnemy);
                }
            }
        }
    }
}
