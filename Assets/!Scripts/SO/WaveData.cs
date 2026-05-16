using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [Min(0)] public int weakEnemyCount = 1;
    [Min(0)] public int tankEnemyCount = 1;
    [Min(0)] public int maxActiveEnemies = 1;
}
