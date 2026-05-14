using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShooterModule : MonoBehaviour
{
    Animator animator;
    Camera cam;
    Vector3 lookAtPoint;
    Vector2 screenMid;
    RaycastHit[] hitInfo = new RaycastHit[1];
    const float maxRaycastDistance = 5000f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        screenMid = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    void Update()
    {
        CalculateLookAtPoint();
        PerformLookAt();
    }

    void CalculateLookAtPoint()
    {
        Ray ray = cam.ScreenPointToRay(screenMid);

        if (Physics.RaycastNonAlloc(ray, hitInfo, maxRaycastDistance) > 0)
        {
            lookAtPoint = hitInfo[0].point;
        }
        else 
        {
            lookAtPoint = ray.GetPoint(maxRaycastDistance);
        }
    }

    void PerformLookAt()
    {
        this.transform.LookAt(lookAtPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lookAtPoint, 0.5f);
    }


}
