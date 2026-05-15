using System;
using UnityEngine;

// Handles enemy health values and death reporting
public class EnemyVitalityModule : VitalityModule
{
    // Stores the registry index for this enemy
    int index;

    // Assigns the index used when this enemy is reported as dead
    public void InitEnemyIndex(int indx) => index = indx;

    // Data Init from Scriptable Object
    public override void InitializeDamageData(ScriptableObject obj)
    {
        var enemyData = obj as EnemyData;
        Health = enemyData.health;
        DamageInflict = enemyData.damageInflict;
    }

    public override void OnEntityKilled()
    {
        // Notifies the level flow that this enemy has been killed
        EventManager.OnEnemyKilled.Invoke(index);
    }
}
