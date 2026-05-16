using UnityEngine;

// Handles enemy health values and death reporting
public class EnemyVitalityModule : VitalityModule
{   
    EnemyController controller;
    EnemyData enemyData;

    private void OnEnable()
    {
        if(controller == null)
            controller = GetComponent<EnemyController>();   
    }

    // Data Init from Scriptable Object
    public override void InitializeDamageData(ScriptableObject obj)
    {
        enemyData = obj as EnemyData;
        Health = enemyData.health;
        DamageInflict = enemyData.damageInflict;
    }

    public override void OnEntityKilled()
    {   
        // Notifies the level flow that this enemy has been killed
        EventManager.OnEnemyKilled.Invoke(controller , enemyData.scoreOnDeath);
    }
}
