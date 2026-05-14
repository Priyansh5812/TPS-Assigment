using UnityEngine;


public interface IDamageHandler
{   
    public float Health
    {
        get;
    }

    public float DamageInflict
    {
        get;
    }

    public void InitializeDamageData(ScriptableObject obj);
    public void ReceiveDamage(float inflictAmt);
    public void InflictDamage(IDamageHandler other);
    public void OnEntityKilled();
}
