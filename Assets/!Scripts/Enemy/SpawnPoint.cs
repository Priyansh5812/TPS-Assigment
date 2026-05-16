using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void SetTransform(Transform transform)
    {
        transform.position = this.transform.position;
        transform.rotation = this.transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 1);
    }
}
