using UnityEngine;

public abstract class VitalityModule : MonoBehaviour, IDamageHandler
{
    public float Health
    {
        get; protected set;
    }

    public float DamageInflict
    {
        get; protected set;
    }

    public virtual void InflictDamage(IDamageHandler other)
    {
        other.ReceiveDamage(DamageInflict);
    }

    public abstract void InitializeDamageData(ScriptableObject obj);

    public abstract void OnEntityKilled();

    public virtual void ReceiveDamage(float inflictAmt)
    {
        Health -= inflictAmt;
        if (Health <= 0)
        {
            OnEntityKilled();
        }
    }
}
