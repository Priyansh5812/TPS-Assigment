using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scriptable Objects/PlayerMovementData")]
public class PlayerStatData : ScriptableObject
{
    [Header("Locomotion-Based")]
    [Min(1f)]public float MaxSpeed;
    [Min(1f)]public float Accelaration;
    [Range(0f , 0.99f)]public float Deacclaration;

    [Header("Shoot Module Based")]
    public float health;
    public float damageInflict;
    [Min(0.25f)]public float fireRate;
}
