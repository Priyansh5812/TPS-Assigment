using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles the overall round flow for starting resetting and ending the level
public class LevelManager : MonoBehaviour
{
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] EnemyData weakEnemyData;
    [SerializeField] EnemyData tankEnemyData;
    [SerializeField] WaveData[] waves;
    (PlayerController,Pose) playerData;
    SpawnPoint[] spawnPoints;
    Queue<EnemyController> inactiveEnemyControllers = new();
    HashSet<EnemyController> activeEnemyControllers = new();
    List<EnemyType> enemyList = new List<EnemyType>(10);
    const int enemyStockCacheCount = 6;
    int maxActiveEnemies;
    bool isStocked = false;
    int currWaveIndex = 0;
    int remainingEnemies;
    int score = 0;
    int enemyListIndex = 0;
    bool isPlayerDead = false;
    Coroutine waveRoutine;
    private void OnEnable()
    {
        EventManager.OnPrepareGame.AddListener(PrepareGame);
        EventManager.OnGameRestarted.AddListener(PrepareGame);
        EventManager.OnWaveStarted.AddListener(StartCurrentWave);
        EventManager.OnEnemyKilled.AddListener(HandleEnemyKilled);
        EventManager.OnPlayerKilled.AddListener(HandlePlayerKilled);
    }

    void Start()
    {
        StockPlayer();
        StartCoroutine(StockEntities());
        spawnPoints = this.GetComponentsInChildren<SpawnPoint>();
    }

    void StockPlayer()
    {   
        playerData.Item1 = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include);
        playerData.Item2 = new Pose(playerData.Item1.transform.position, playerData.Item1.transform.rotation);
    }

    IEnumerator StockEntities()
    {
        AsyncInstantiateOperation<EnemyController> op = InstantiateAsync(enemyPrefab, enemyStockCacheCount, this.transform);

        yield return op;

        var enemyControllers = op.Result;

        foreach (var i in enemyControllers)
        {
            i.gameObject.SetActive(false); 
            this.inactiveEnemyControllers.Enqueue(i);
        }

        isStocked = true;
    }


    public void PrepareGame()
    {
        ResetPlayer(true);
        ClearEnemyControllers();
        PrepareEnemyList();
        isPlayerDead = false;
        EventManager.OnPreWaveStarted.Invoke(currWaveIndex);
    }


    void StartCurrentWave()
    {
        remainingEnemies = waves[currWaveIndex].weakEnemyCount + waves[currWaveIndex].tankEnemyCount;
        maxActiveEnemies = waves[currWaveIndex].maxActiveEnemies;

        EventManager.OnUpdateEnemyCount.Invoke(remainingEnemies);
        
        waveRoutine = StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (!isStocked) // Wait if enemies are not stocked yet
        {
            yield return null;
        }

        while (remainingEnemies > 0)
        {
            if (activeEnemyControllers.Count < Mathf.Min(remainingEnemies, maxActiveEnemies))
            {
                DispatchEnemy();
            }
            yield return null;
        }

        currWaveIndex++;
        if (currWaveIndex < waves.Length)
        {
            EventManager.OnPrepareGame.Invoke();
        }
        else 
        {
            EventManager.OnGameOver.Invoke(GameOverType.WIN , score);
            ResetStats();
            ResetPlayer(false);
        }
        waveRoutine = null;
    }

    void HandleEnemyKilled(EnemyController e , int scoreInr)
    {
        remainingEnemies--;
        score += scoreInr;
        EventManager.OnUpdateEnemyCount.Invoke(remainingEnemies);

        if (activeEnemyControllers.Contains(e))
            activeEnemyControllers.Remove(e);   

        e.gameObject.SetActive(false);
        inactiveEnemyControllers.Enqueue(e);

    }

    void HandlePlayerKilled()
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);
        isPlayerDead = true;
        ResetStats();
        ResetPlayer(false);
        EventManager.OnGameOver.Invoke(GameOverType.LOST , score);
    }

    void ResetStats()
    {
        score = remainingEnemies = currWaveIndex = 0;
    }

    void ClearEnemyControllers()
    {   
        if (activeEnemyControllers.Count == 0)
            return;

        foreach (var e in activeEnemyControllers)
        {
            e.gameObject.SetActive(false);
            this.inactiveEnemyControllers.Enqueue(e);
        }

        activeEnemyControllers.Clear();
    }

    void ResetPlayer(bool setPlayerActive)
    {
        playerData.Item1.transform.SetPositionAndRotation(playerData.Item2.Position, playerData.Item2.Rotation);
        playerData.Item1.gameObject.SetActive(setPlayerActive);
    }


    void DispatchEnemy()
    {
        var e = inactiveEnemyControllers.Dequeue();
        var sp = GetRandomSpawnPoint();
        Debug.Log(enemyListIndex);
        e.SetEnemyData(enemyList[enemyListIndex++] == EnemyType.Weak ? weakEnemyData : tankEnemyData);
        sp.SetTransform(e.transform);
        e.gameObject.SetActive(true);
        activeEnemyControllers.Add(e);
    }

    void PrepareEnemyList()
    {
        enemyListIndex = 0;
        enemyList.Clear();
        int c = 0;
        while (c < waves[currWaveIndex].weakEnemyCount)
        {
            enemyList.Add(EnemyType.Weak);
            c++;
        }

        c = 0;
        while (c < waves[currWaveIndex].tankEnemyCount)
        {
            enemyList.Add(EnemyType.Tank);
            c++;
        }


        for (c = 0 ; c < enemyList.Count; c++)
        {
            int randIndex = Random.Range(0, c + 1);

            var t = enemyList[c];
            enemyList[c] = enemyList[randIndex];
            enemyList[randIndex] = t;
        }
    }

    SpawnPoint GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
    

    private void OnDisable()
    {
        EventManager.OnPrepareGame.RemoveListener(PrepareGame);
        EventManager.OnGameRestarted.RemoveListener(PrepareGame);
        EventManager.OnWaveStarted.RemoveListener(StartCurrentWave);
        EventManager.OnEnemyKilled.RemoveListener(HandleEnemyKilled);
        EventManager.OnPlayerKilled.RemoveListener(HandlePlayerKilled);
    }
}


