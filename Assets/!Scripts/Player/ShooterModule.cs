using UnityEngine;


[RequireComponent(typeof(Animator))]
public class ShooterModule : MonoBehaviour
{
    [SerializeField] PlayerStatData data;
    [SerializeField] Transform shootPoint;
    IDamageHandler playerVitality;
    Animator animator;
    Camera cam;
    Vector3 lookAtPoint;
    Vector2 screenMid;
    RaycastHit[] hitInfos = new RaycastHit[5];
    RaycastHit closestHit;
    const float maxRaycastDistance = 5000f;
    float lastFiringTime;
    LayerMask enemyLayer;
    //-------

    ShooterModuleView view;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
        screenMid = new Vector2(Screen.width / 2, Screen.height / 2);
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Start()
    {
        playerVitality = EventManager.GetPlayerVitality.Invoke();
        view ??= new(animator);
    }

    void Update()
    {
        CalculateLookAtPoint();
        PerformLookAt();
        CheckForShoot();
    }
    #region Aim
    void CalculateLookAtPoint()
    {
        Ray ray = cam.ScreenPointToRay(screenMid);

        float minDistance = maxRaycastDistance;
        int c = Physics.RaycastNonAlloc(ray, hitInfos, maxRaycastDistance);
        closestHit = default;
        if (c > 0)
        {
            for(int i = 0; i < c; i++)
            {
                if (minDistance > hitInfos[i].distance)
                { 
                    lookAtPoint = hitInfos[i].point;
                    minDistance = hitInfos[i].distance;
                    closestHit = hitInfos[i];
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
    #endregion

    #region ShootLogic
    void CheckForShoot()
    {   
        if (Input.GetMouseButtonDown(0) && HasExceededShootTimeout())
        {
            lastFiringTime = Time.unscaledTime;
            PerformShootAction();
        }
    }

    void PerformShootAction()
    {
        view.PlayShootAnimation();
        Collider collider = closestHit.collider;
        Debug.Log(collider == null ? "None" : collider.gameObject.name);
        if (IsEnemyInLineOfAim(collider))
        {   
            IDamageHandler handler = collider.GetComponent<IDamageHandler>();
            playerVitality.InflictDamage(handler);
        }

    }

    bool IsEnemyInLineOfAim(Collider collider)
    {
        return collider != null ? collider.gameObject.layer == enemyLayer: false;
    }

    bool HasExceededShootTimeout()
    {
        float reqDelay = 1 / data.fireRate;
        return reqDelay <= Time.unscaledTime - lastFiringTime;
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(lookAtPoint, 0.5f);
    }


}

