using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Transform player;
    GameObject[] spawnPoints;
    List<GameObject> inRangePoints;

    List<GameObject> allEnemies;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxAmountOfEnemies = 10;
    [SerializeField] float maxSpawnRange = 25;
    [SerializeField] float timeBetweenSpawns;
    float timer;

    [SerializeField] float maxEnemyLiveRange = 40;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        inRangePoints = new List<GameObject>();
        allEnemies = new List<GameObject>();

        UpdateNearPlayerList();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawns && allEnemies.Count <= maxAmountOfEnemies && inRangePoints.Count > 0)
        {
            timer = 0;
            SpawnEnemy();
        }
    }

    private void LateUpdate()
    {
        UpdateNearPlayerList();
    }

    void UpdateNearPlayerList()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (Vector3.Distance(spawnPoint.transform.position, player.position) <= maxSpawnRange)
            {
                if (!inRangePoints.Contains(spawnPoint))
                {
                    inRangePoints.Add(spawnPoint);
                }
            }
            else
            {
                if (inRangePoints.Contains(spawnPoint))
                {
                    inRangePoints.Remove(spawnPoint);
                }
            }
        }
    }

    void SpawnEnemy()
    {
        allEnemies.Add(Instantiate(enemyPrefab, inRangePoints[Random.Range(0, inRangePoints.Count-1)].transform.position, Quaternion.identity));
    }

    public void RemoveFromList(GameObject instance)
    {
        if (allEnemies.Contains(instance))
        {
            allEnemies.Remove(instance);
            Destroy(instance);
        }
    }
}
