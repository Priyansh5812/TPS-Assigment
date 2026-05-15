using UnityEngine;
using UnityEngine.UI;

// Handles player health values and keeps the health view updated
public class PlayerVitalityModule : VitalityModule
{
    // This image material shows the current health amount
    [SerializeField] Image healthFillup;

    // Player stat data is cached here after setup
    PlayerStatData playerData;

    private void Start()
    {
        // Creates a separate material instance for this health bar
        healthFillup.material = new Material(healthFillup.material);
    }


    // Data Init from Scriptable Object
    public override void InitializeDamageData(ScriptableObject obj)
    {
        playerData = obj as PlayerStatData;
        Health = playerData.health;
        DamageInflict = playerData.damageInflict;
        UpdateHealthView();
    }

    void UpdateHealthView()
    {
        // Refreshes the bar fill based on the remaining health ratio
        healthFillup.material.SetFloat("_Factor", Health / playerData.health);
    }

    public override void ReceiveDamage(float inflictAmt)
    {
        // Applies damage first and then refreshes the health display
        Debug.Log(inflictAmt);
        base.ReceiveDamage(inflictAmt);
        UpdateHealthView();
    }


    public override void OnEntityKilled()
    {
        // Notifies the game flow that the player has been killed
        EventManager.OnPlayerKilled.Invoke();   
    }

}
