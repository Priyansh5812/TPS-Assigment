using UnityEngine;


[RequireComponent(typeof(Animator))]
// Handles aiming shooting timing and hit detection for the player weapon
public class ShooterModule : MonoBehaviour
{
    [SerializeField] PlayerStatData data;
    [SerializeField] Transform shootPoint;
    IDamageHandler playerVitality;
    Animator animator;
    Camera cam;
    Vector3 lookAtPoint;
    Vector2 screenMid;
    // Reusable hit buffer for aim checks
    RaycastHit[] hitInfos = new RaycastHit[5];
    RaycastHit closestHit;
    const float maxRaycastDistance = 5000f;
    float lastFiringTime;
    LayerMask enemyLayer;

    // View helper that handles firing animation and impact particles
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
        // Finds the closest point under the screen center so the weapon can aim correctly
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
        // Rotates the shooter toward the current aim point
        this.transform.LookAt(lookAtPoint);
    }
    #endregion

    #region ShootLogic
    void CheckForShoot()
    {   
        // Fires only when the mouse is pressed and the fire rate delay has passed
        if (Input.GetMouseButtonDown(0) && HasExceededShootTimeout())
        {
            lastFiringTime = Time.unscaledTime;
            PerformShootAction();
        }
    }

    void PerformShootAction()
    {
        // Plays the visual shot feedback and applies damage if the aim lands on an enemy
        view.PlayShootAnimation();
        view.PlayParticleAt(closestHit.point, closestHit.normal);
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
        // Checks whether the current hit collider belongs to the enemy layer
        return collider != null ? collider.gameObject.layer == enemyLayer: false;
    }

    bool HasExceededShootTimeout()
    {
        // Compares the fire rate delay against the time since the last shot
        float reqDelay = 1 / data.fireRate;
        return reqDelay <= Time.unscaledTime - lastFiringTime;
    }

    #endregion
}

