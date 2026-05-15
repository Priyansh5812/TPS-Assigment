using System.Threading.Tasks;
using UnityEngine;

// Handles the overall round flow for starting resetting and ending the level
public class LevelManager : MonoBehaviour
{
    // Stores the player vitality module together with the original spawn pose
    (PlayerVitalityModule, Pose) playerModule;

    // Stores every enemy vitality module together with its original spawn pose
    (EnemyVitalityModule , Pose)[] enemyVitalityModulesRegistry;

    // Tracks whether the level is currently in a game over state
    bool isGameOver = true;

    // Keeps count of how many enemies are still active in the round
    int totalEnemies;
    private void OnEnable()
    {
        EventManager.OnPlayerKilled.AddListener(HandlePlayerDeath);
        EventManager.OnEnemyKilled.AddListener(HandleEnemyKilled);
        EventManager.OnGameRestarted.AddListener(HandleGameRestart);
    }

    void Start()
    {
        InitializePlayerModule();
        InitializeEnemyModules();
    }

    public void InitiateGame()
    {
        // Reactivates the player and all enemies before the round begins
        playerModule.Item1.gameObject.SetActive(true);

        foreach (var i in enemyVitalityModulesRegistry)
        {
            i.Item1.gameObject.SetActive(true);
        }

        isGameOver = false;
    }

    void InitializePlayerModule()
    {
        // Finds the player module and remembers the original player spawn pose (One time execution here)
        playerModule.Item1 = GameObject.FindAnyObjectByType<PlayerVitalityModule>(FindObjectsInactive.Include);
        playerModule.Item2 = new Pose(playerModule.Item1.transform.position, playerModule.Item1.transform.rotation);
    }

    void InitializeEnemyModules()
    {   
        // Finds every enemy module then stores each one with its spawn pose and index
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


    void HandleEnemyKilled(int index)
    {
        // Resets the defeated enemy and ends the round when no enemies remain
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
        // Resets the player and reports the loss with the remaining enemy count
        ResetPlayer();
        EventManager.OnGameOver.Invoke(GameOverType.LOST, totalEnemies);
        isGameOver = true;
    }

    async void HandleGameRestart()
    {
        // Resets every tracked character back to the saved start state before restarting
        foreach (var i in enemyVitalityModulesRegistry)
        {
            ResetEnemy(i.Item1, i.Item2);
        }

        ResetPlayer();

        totalEnemies = enemyVitalityModulesRegistry.Length;
        
        await Task.Yield(); // Waits one frame so the reset state settles before the next round starts

        InitiateGame();
    }

    void ResetEnemy(EnemyVitalityModule module , Pose pose)
    {
        // Deactivates the enemy and places it back at its saved spawn pose
        module.gameObject.SetActive(false);
        module.transform.SetLocalPositionAndRotation(pose.Position, pose.Rotation);
    }


    void ResetPlayer()
    {
        // Deactivates the player and places the player back at the saved spawn pose
        playerModule.Item1.gameObject.SetActive(false);
        playerModule.Item1.transform.SetPositionAndRotation(playerModule.Item2.Position, playerModule.Item2.Rotation);
    }


    private void OnDisable()
    {
        EventManager.OnEnemyKilled.RemoveListener(HandleEnemyKilled);
        EventManager.OnGameRestarted.RemoveListener(HandleGameRestart);
        EventManager.OnPlayerKilled.RemoveListener(HandlePlayerDeath);
    }
}
