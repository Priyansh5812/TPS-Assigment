using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public Material meshMaterial;
    public float movementSpeed;
    public float accelaration;
    public float health;
    public float damageInflict;
    public float attackRange = 2.0f;
    public float animationDuration;
}
