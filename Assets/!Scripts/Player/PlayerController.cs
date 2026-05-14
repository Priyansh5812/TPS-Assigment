using UnityEngine;
[RequireComponent(typeof(CharacterController), typeof(PlayerVitalityModule))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerStatData playerData;
    [SerializeField] Transform bodyTransform;
    CharacterController cc;
    IDamageHandler vitality; 
    Camera cam;
    Vector3 inputDirection;
    Vector3 intendedDirection;
    Vector3 targetVelocity;
    Vector3 currentVelocity;
    Vector3 lastVelocity;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        vitality = GetComponent<PlayerVitalityModule>();
    }

    private void OnEnable()
    {
        EventManager.GetPlayerTransform.AddListener(GetPlayerTransform);
        EventManager.GetPlayerVitality.AddListener(GetPlayerVitality);
    }
    void Start()
    {
        vitality.InitializeDamageData(playerData);
    }

    void Update()
    {
        PollInputs();
        GetIntendedDirection();
        CalculateVelocity();

        //--------------

        BodyRotationPass();
    }
    #region Locomotion
    void PollInputs()
    {
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.z = Input.GetAxisRaw("Vertical");
    }

    void GetIntendedDirection()
    {
        intendedDirection = cam.transform.TransformDirection(inputDirection);
        intendedDirection = Vector3.ProjectOnPlane(intendedDirection, Vector3.up);
    }

    void CalculateVelocity()
    {
        targetVelocity = intendedDirection.normalized * playerData.MaxSpeed;

        if (targetVelocity.sqrMagnitude > 0)
        {
            currentVelocity = lastVelocity + (GetAccelaration() * Time.deltaTime);
        }
        else 
        {
            currentVelocity *= playerData.Deacclaration;
        }

        currentVelocity = Vector3.ClampMagnitude(currentVelocity, playerData.MaxSpeed);

        lastVelocity = currentVelocity;
    }
    #endregion

    #region BodyRotation
    void BodyRotationPass()
    { 
        Vector3 forwardDirection = cam.transform.forward;
        forwardDirection = Vector3.ProjectOnPlane(forwardDirection, Vector3.up);
        bodyTransform.localRotation = Quaternion.LookRotation(forwardDirection);
    }
    #endregion

    void LateUpdate()
    {
        cc.Move(currentVelocity * Time.deltaTime);  
    }

    void OnDisable()
    {
        EventManager.GetPlayerTransform.RemoveListener(GetPlayerTransform);
        EventManager.GetPlayerVitality.RemoveListener(GetPlayerVitality);
    }


    #region HELPERS
    Vector3 GetAccelaration()
    {
        Vector3 accDir = targetVelocity - currentVelocity;
        return accDir.normalized * playerData.Accelaration;
    }
    #endregion


    #region Others

    Transform GetPlayerTransform() => this.gameObject.transform;
    IDamageHandler GetPlayerVitality() => this.vitality;
    #endregion
}
