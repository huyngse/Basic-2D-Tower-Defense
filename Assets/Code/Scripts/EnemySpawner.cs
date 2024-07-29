using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject[] EnemyPrefabs;

    [Header("Attributes")]
    [SerializeField]
    private int baseEnemies = 8;

    [SerializeField]
    private float enemiesPerSecond = 0.5f;

    [SerializeField]
    private float maxEnemiesPerSecond = 10f;

    [SerializeField]
    private float timeBetweenWave = 5f;

    [SerializeField]
    private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new();
    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning;
    private float baseEnemiesPerSecond;

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWave);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        enemiesPerSecond = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(
            baseEnemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor),
            0,
            maxEnemiesPerSecond
        );
    }

    private void SpawnEnemies()
    {
        int index = Random.Range(0, EnemyPrefabs.Length);
        GameObject prefabToSpawn = EnemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        baseEnemiesPerSecond = enemiesPerSecond;
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning)
            return;
        timeSinceLastSpawn += Time.deltaTime;
        if (enemiesLeftToSpawn > 0)
        {
            if (timeSinceLastSpawn >= 1f / enemiesPerSecond)
            {
                SpawnEnemies();
                enemiesLeftToSpawn--;
                enemiesAlive++;
                timeSinceLastSpawn = 0f;
            }
        }
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }
}
