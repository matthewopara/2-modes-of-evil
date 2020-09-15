using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummonEnemiesEntry : MonoBehaviour
{
    ObjectPool objectPool;
    //public GameObject[] gameEnemies;
    private SpriteRenderer closedRoomRenderer;
    private PolygonCollider2D closedRoomCollider;
    public string[] gameEnemies;
    //private List<GameObject> sceneEnemies = new List<GameObject>();
    private List<string> sceneEnemies = new List<string>();
    private int enemiesPerWave = 5;
    private Vector3 buffer = new Vector3(.5f, .5f, 0f);
    private float roomEdgeX = 4.5f;
    private float roomEdgeY = 3.23f;

    private bool secondWaveSpawned = false;
    private List<GameObject> wave = new List<GameObject>();
    private int waveCount;

    private Scene currentScene;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = ObjectPool.Instance;

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        ChooseEnemies();

        closedRoomRenderer = GetComponentInChildren<SpriteRenderer>();
        closedRoomCollider = GetComponentInChildren<PolygonCollider2D>();

        closedRoomRenderer.enabled = true;
        closedRoomCollider.enabled = true;
        // Instantiate 5 enemies
        for (int i = 0; i < enemiesPerWave; i++)
        {
            //GameObject spawnedEnemy = (GameObject)Instantiate(sceneEnemies[Random.Range(0, sceneEnemies.Count)], gameObject.transform.parent.parent.transform.position + new Vector3(Random.Range(-roomEdgeX, roomEdgeX), Random.Range(-roomEdgeY, roomEdgeY), 0), Quaternion.identity);
            GameObject spawnedEnemy = objectPool.SpawnFromPool(sceneEnemies[Random.Range(0, sceneEnemies.Count)], gameObject.transform.parent.parent.transform.position + new Vector3(Random.Range(-roomEdgeX, roomEdgeX), Random.Range(-roomEdgeY, roomEdgeY), 0), Quaternion.identity);
            wave.Add(spawnedEnemy);
        }
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
        if (wave.Count <= 1 && !secondWaveSpawned)
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
