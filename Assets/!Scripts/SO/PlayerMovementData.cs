using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scriptable Objects/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [Min(1f)]public float MaxSpeed;
    [Min(1f)]public float Accelaration;
    [Range(0f , 0.99f)]public float Deacclaration;
}
