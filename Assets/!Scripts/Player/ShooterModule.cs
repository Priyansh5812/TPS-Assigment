using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShooterModule : MonoBehaviour
{
    Animator animator;
    Camera cam;
    Vector3 lookAtPoint;
    Vector2 screenMid;
    RaycastHit[] hitInfos = new RaycastHit[5];
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

        float minDistance = maxRaycastDistance;
        int c = Physics.RaycastNonAlloc(ray, hitInfos, maxRaycastDistance);
        if (c > 0)
        {
            for(int i = 0; i < c; i++)
            {
                if (minDistance > hitInfos[i].distance)
                { 
                    lookAtPoint = hitInfos[i].point;
                    minDistance = hitInfos[i].distance;
                }
            }
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
