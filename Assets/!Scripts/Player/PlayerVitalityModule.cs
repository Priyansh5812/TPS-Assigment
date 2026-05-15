using UnityEngine;
using UnityEngine.UI;
public class PlayerVitalityModule : VitalityModule
{
    [SerializeField] Image healthFillup;
    PlayerStatData playerData;

    private void Start()
    {
        healthFillup.material = new Material(healthFillup.material);
    }

    public override void InitializeDamageData(ScriptableObject obj)
    {
        playerData = obj as PlayerStatData;
        Health = playerData.health;
        DamageInflict = playerData.damageInflict;
        UpdateHealthView();
    }

    void UpdateHealthView()
    {
        healthFillup.material.SetFloat("_Factor", Health / playerData.health);
    }

    public override void ReceiveDamage(float inflictAmt)
    {
        base.ReceiveDamage(inflictAmt);
        UpdateHealthView();
    }


    public override void OnEntityKilled()
    {
        EventManager.OnPlayerKilled.Invoke();   
    }

}
