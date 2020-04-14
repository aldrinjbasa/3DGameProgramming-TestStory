using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MobSpawner : MonoBehaviour
{

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public Transform enemyToSpawn;
        public int maxEnemyCount;
        public float spawnRate;

    }

    public enum SpawnState
    {
        SPAWNING, WAITING, COUNTING
    }

    public Wave[] enemyWaves;
    private int nextWave;
    public float timeTilNextWave = 5f;
    public float countTilNextWave;
    private SpawnState state = SpawnState.COUNTING;
    private float searchCountdown = 1f;
    public Transform[] spawnPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        countTilNextWave = timeTilNextWave;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if(EnemyAliveCheck() == false)
            {
                WaveCompleted();   
            }
            else
            {
                return;
            }
        }
        if(countTilNextWave <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(enemyWaves[nextWave]));
            }
        }
        else
        {
            countTilNextWave -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        for(int i = 0; i < _wave.maxEnemyCount; i++)
        {
            SpawnEnemies(_wave.enemyToSpawn);
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemies(Transform _enemy)
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, spawn.position, Quaternion.identity);
    }

    bool EnemyAliveCheck()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {

                return false;
            }
        }
        return true;
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        countTilNextWave = timeTilNextWave;
        if (nextWave + 1 > enemyWaves.Length - 1)
        {
            Debug.Log("Game completed");
        }
        else
        {
            nextWave++;
        }
    }
}
