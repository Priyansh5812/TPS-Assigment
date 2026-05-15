using UnityEngine;

// Shared health and damage behavior for anything that can take damage
public abstract class VitalityModule : MonoBehaviour, IDamageHandler
{
    // Current health value 
    public float Health
    {
        get; protected set;
    }

    // Damage dealt to another target
    public float DamageInflict
    {
        get; protected set;
    }

    public virtual void InflictDamage(IDamageHandler other)
    {
        // Sends this damage value to the other damage handler
        other.ReceiveDamage(DamageInflict);
    }

    // Initialization
    public abstract void InitializeDamageData(ScriptableObject obj);

    // Lets each subclass decide what happens when health reaches zero
    public abstract void OnEntityKilled();

    public virtual void ReceiveDamage(float inflictAmt)
    {
        // Reduces health and triggers the kill flow when no health remains
        Health -= inflictAmt;
        if (Health <= 0)
        {
            OnEntityKilled();
        }
    }
}
