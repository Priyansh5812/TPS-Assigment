using UnityEngine;

public struct Pose
{   
    public Pose(Vector3 position , Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public Vector3 Position;
    public Quaternion Rotation;
}
