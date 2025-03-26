using Meta.WitAi;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform[] cover;
    [SerializeField]
    private GameObject enemySpawnPointPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float enemySpawnTime = 1;
    [SerializeField]
    private float enemySpawnLatency = 1;

    private MemoryPool spawnPointMemoryPool;
    private MemoryPool enemyMemoryPool;

    private int numberOfEnemiesSpawnedAtOnce = 1;
    private Vector2Int mapSize = new Vector2Int(100, 100);
    private Wall farthestWall;

    private bool[] isOccupied;
    private Transform targetTransform;

    private void Start()
    {
        isOccupied = new bool[4] { false, false, false, false };
        farthestWall = WallDetector.farthestWall;
    }

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool = new MemoryPool(enemyPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        int maximumNumber = 50;

        while (true)
        {
            for (int i = 0; i < numberOfEnemiesSpawnedAtOnce; i++)
            {
                GameObject item = spawnPointMemoryPool.ActivatePoolItem();

                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.49f, mapSize.x * 0.49f), 1, Random.Range(-mapSize.y * 0.49f, mapSize.y * 0.49f));

                StartCoroutine("SpawnEnemy", item);
            }
            currentNumber++;

            if (currentNumber >= maximumNumber)
            {
                currentNumber = 0;
                numberOfEnemiesSpawnedAtOnce++;
            }

            yield return new WaitForSeconds(enemySpawnTime);
        }
    }

    private IEnumerator SpawnEnemy(GameObject point)
    {
        yield return new WaitForSeconds(enemySpawnLatency);
        GameObject item = enemyMemoryPool.ActivatePoolItem();
        item.transform.position = point.transform.position;
        for (int i = 0; i < isOccupied.Length; i++)
        {
            targetTransform = target;
            if (isOccupied[i] == false)
            {
                isOccupied[i] = true;
                targetTransform = cover[i];
                break;
            }
            else
            {
                continue;
            }
        }
        if (targetTransform != target)
        {
            item.GetComponent<CoverFSM>().Setup(target, targetTransform, this);
        }
        else
        {
            item.GetComponent<EnemyFSM>().Setup(target, this);
        }
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }
    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
    }
}
