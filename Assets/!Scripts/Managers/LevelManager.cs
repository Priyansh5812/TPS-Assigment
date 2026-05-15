using System.Threading.Tasks;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    (PlayerVitalityModule, Pose) playerModule;
    (EnemyVitalityModule , Pose)[] enemyVitalityModulesRegistry;
    bool isGameOver = true;
    int totalEnemies;
    private void OnEnable()
    {
        EventManager.OnPlayerKilled.AddListener(HandlePlayerDeath);
        EventManager.OnEnemyKilled.AddListener(HandleEnemyKilled);
        EventManager.OnGameRestarted.AddListener(HandleGameRestart);
        EventManager.IsGameOver.AddListener(IsGameOver);
    }

    void Start()
    {
        InitializePlayerModule();
        InitializeEnemyModules();
    }

    public void InitiateGame()
    {
        playerModule.Item1.gameObject.SetActive(true);

        foreach (var i in enemyVitalityModulesRegistry)
        {
            i.Item1.gameObject.SetActive(true);
        }

        isGameOver = false;
    }

    void InitializePlayerModule()
    {
        playerModule.Item1 = GameObject.FindAnyObjectByType<PlayerVitalityModule>(FindObjectsInactive.Include);
        playerModule.Item2 = new Pose(playerModule.Item1.transform.position, playerModule.Item1.transform.rotation);
    }

    void InitializeEnemyModules()
    {   
        var modules = GetComponentsInChildren<EnemyVitalityModule>(true);
        enemyVitalityModulesRegistry ??= new (EnemyVitalityModule, Pose)[modules.Length];
        int c = 0;
        foreach (var i in modules)
        {
            i.InitEnemyIndex(c);
            enemyVitalityModulesRegistry[c].Item1 = i;
            enemyVitalityModulesRegistry[c].Item2 = new Pose(i.transform.localPosition, i.transform.localRotation);
            c++;
        }
        totalEnemies = modules.Length;
    }

    bool IsGameOver() => isGameOver;


    void HandleEnemyKilled(int index)
    {
        if (enemyVitalityModulesRegistry.Length <= index)
            return;
        
        ResetEnemy(enemyVitalityModulesRegistry[index].Item1 , enemyVitalityModulesRegistry[index].Item2);
        totalEnemies--;

        if (totalEnemies == 0)
        { 
            EventManager.OnGameOver.Invoke(GameOverType.WIN , 0);
            isGameOver = true;
        }
    }

    void HandlePlayerDeath()
    {
        ResetPlayer();
        EventManager.OnGameOver.Invoke(GameOverType.LOST, totalEnemies);
        isGameOver = true;
    }

    async void HandleGameRestart()
    {
        foreach (var i in enemyVitalityModulesRegistry)
        {
            ResetEnemy(i.Item1, i.Item2);
        }

        ResetPlayer();

        totalEnemies = enemyVitalityModulesRegistry.Length;
        
        await Task.Yield(); // let the changes take effect

        InitiateGame();
    }

    void ResetEnemy(EnemyVitalityModule module , Pose pose)
    {
        module.gameObject.SetActive(false);
        module.transform.SetLocalPositionAndRotation(pose.Position, pose.Rotation);
    }


    void ResetPlayer()
    {
        playerModule.Item1.gameObject.SetActive(false);
        playerModule.Item1.transform.SetPositionAndRotation(playerModule.Item2.Position, playerModule.Item2.Rotation);
    }


    private void OnDisable()
    {
        EventManager.OnEnemyKilled.RemoveListener(HandleEnemyKilled);
        EventManager.OnGameRestarted.RemoveListener(HandleGameRestart);
        EventManager.OnPlayerKilled.RemoveListener(HandlePlayerDeath);
        EventManager.IsGameOver.RemoveListener(IsGameOver);
    }
}
