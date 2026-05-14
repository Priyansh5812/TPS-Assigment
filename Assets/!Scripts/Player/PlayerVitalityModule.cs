using UnityEngine;

public class PlayerVitalityModule : VitalityModule
{
    public override void InitializeDamageData(ScriptableObject obj)
    {
        var playerData = obj as PlayerStatData;
        Health = playerData.health;
        DamageInflict = playerData.damageInflict;
    }

    public override void OnEntityKilled()
    {
        
    }

}
