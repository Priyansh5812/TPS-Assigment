using UnityEngine;


// Handles player health values and keeps the health view updated
public class PlayerVitalityModule : VitalityModule
{
    // Player stat data is cached here after setup
    PlayerStatData playerData;

    // Data Init from Scriptable Object
    public override void InitializeDamageData(ScriptableObject obj)
    {
        playerData = obj as PlayerStatData;
        Health = playerData.health;
        DamageInflict = playerData.damageInflict;
    }


    public override void ReceiveDamage(float inflictAmt)
    {
        // Applies damage first and then refreshes the health display
        base.ReceiveDamage(inflictAmt);
        EventManager.OnPlayerDamaged.Invoke(Health / playerData.health);
    }


    public override void OnEntityKilled()
    {
        // Notifies the game flow that the player has been killed
        EventManager.OnPlayerKilled.Invoke();   
    }

}
