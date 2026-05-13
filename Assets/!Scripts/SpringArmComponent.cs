using UnityEngine;


public class SpringArmComponent : MonoBehaviour
{
    Camera mainCamera;
    [Header("Position")]
    [SerializeField] float m_raycastDistance = 7.0f;
    [SerializeField] Vector3 originOffset = Vector3.up;
    [SerializeField] Vector3 direction = Vector3.up - Vector3.forward;
    [SerializeField] LayerMask layersToIgnore;
    [Header("Rotation")]
    [SerializeField] Vector3 EulerOffset;
    [Header("Input")]
    [SerializeField] float XSens;
    [SerializeField] float YSens;
    [SerializeField] float globalSensMultiplier = 5f;
    [SerializeField, Range(0f, 90f)] float minMaxXAngle;
    [SerializeField] bool lazyUpdate = true;
    [SerializeField] float lazyUpdationSpeed = 10.0f;
    float xInput, yInput;
    Vector3 origin;
    Vector3 rayDirection;
    Vector3 finalPosition;
    Quaternion finalRotation;
    float currentXRotation = 0f;
    RaycastHit[] hitsInfo = new RaycastHit[1];
    bool isActive = true;
    private void OnEnable()
    {
        SetGameStartState();
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        mainCamera ??= Camera.main;
    }

    private void Update()
    {
        AddInput();
        ComputeRayParams();
        ComputeRotation();
        ComputePosition();
        ApplyPose();
        Debug.DrawLine(origin, finalPosition , Color.purple);
    }

    void AddInput()
    {
        xInput = isActive ? Input.GetAxis("Mouse X") : 0;
        yInput = isActive ? Input.GetAxis("Mouse Y") : 0;
        Vector3 rotation = this.transform.localRotation.eulerAngles;
        float xStep = -yInput * XSens * globalSensMultiplier * Time.deltaTime;
        float yStep = xInput * YSens * globalSensMultiplier * Time.deltaTime;
        currentXRotation += xStep;
        currentXRotation = Mathf.Clamp(currentXRotation, -minMaxXAngle, minMaxXAngle);

        float currentYRotation = transform.localEulerAngles.y + yStep;

        transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
    }


    void ComputeRayParams()
    {
        origin = this.transform.position;
        origin += (this.transform.forward * originOffset.z) + (this.transform.up * originOffset.y) + (this.transform.right * originOffset.x);
        rayDirection = this.transform.rotation * direction;
    }

    void ComputePosition()
    {
        float camDistance = m_raycastDistance;
        Vector3 normDir = rayDirection.normalized;
        LayerMask mask = ~layersToIgnore;
        
        if (Physics.SphereCastNonAlloc(origin, 0.25f, normDir, hitsInfo, m_raycastDistance, mask) > 0)
        {
            RaycastHit hitInfo = hitsInfo[0];
            camDistance = hitInfo.distance;
        }
        finalPosition = origin + (normDir * camDistance);
    }

    void ComputeRotation()
    {
        finalRotation = Quaternion.LookRotation(-rayDirection, this.transform.up);
        finalRotation *= Quaternion.Euler(EulerOffset);
    }

    void ApplyPose()
    {
        mainCamera.transform.position = lazyUpdate ? Vector3.Lerp(mainCamera.transform.position, finalPosition, lazyUpdationSpeed * Time.deltaTime) : finalPosition;
        mainCamera.transform.rotation = lazyUpdate ? Quaternion.Slerp(mainCamera.transform.rotation, finalRotation, lazyUpdationSpeed * Time.deltaTime) : finalRotation;
    }

    public (Vector3, Quaternion) GetComputedPose()
    {
        ComputePosition();
        ComputeRotation();
        return (finalPosition, finalRotation);
    }




    void SetGameStartState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isActive = true;
    }
    void SetGameEndState()
    {
        Cursor.lockState = CursorLockMode.None;
        isActive = false;
    }




}
