using UnityEngine;

public class LevelManager : MonoBehaviour
{
    (EnemyVitalityModule , Pose)[] enemyVitalityModulesRegistry;
    int totalEnemies;
    private void OnEnable()
    {
        EventManager.OnEnemyKilled.AddListener(HandleEnemyKilled);
    }

    private void Start()
    {
        InitializeEnemyModules();
    }

    void InitializeEnemyModules()
    {   
        var modules = GetComponentsInChildren<EnemyVitalityModule>();
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
        if (enemyVitalityModulesRegistry.Length <= index)
            return;
        
        ResetEnemy(enemyVitalityModulesRegistry[index].Item1 , enemyVitalityModulesRegistry[index].Item2);
        totalEnemies--;
        EventManager.OnGameOver.Invoke();
    }

    
    void ResetEnemy(EnemyVitalityModule module , Pose pose)
    {
        module.gameObject.SetActive(false);
        module.transform.SetLocalPositionAndRotation(pose.Position, pose.Rotation);
    }


    private void OnDisable()
    {
        EventManager.OnEnemyKilled.RemoveListener(HandleEnemyKilled);
    }
}

public struct Pose
{   
    public Pose(Vector3 position , Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public Vector3 Position;
    public Quaternion Rotation;
}