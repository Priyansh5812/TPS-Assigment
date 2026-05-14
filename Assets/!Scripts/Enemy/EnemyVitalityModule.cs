using UnityEngine;

public class EnemyVitalityModule : VitalityModule
{
    public override void InitializeDamageData(ScriptableObject obj)
    {
        var enemyData = obj as EnemyData;
        Health = enemyData.health;
        DamageInflict = enemyData.damageInflict;
    }

    public override void OnEntityKilled()
    {

    }
}
