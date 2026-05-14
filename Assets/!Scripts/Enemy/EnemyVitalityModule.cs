using System;
using UnityEngine;

public class EnemyVitalityModule : VitalityModule
{
    int index;

    public void InitEnemyIndex(int indx) => index = indx;

    public override void InitializeDamageData(ScriptableObject obj)
    {
        var enemyData = obj as EnemyData;
        Health = enemyData.health;
        DamageInflict = enemyData.damageInflict;
    }

    public override void OnEntityKilled()
    {
        EventManager.OnEnemyKilled.Invoke(index);
    }
}
