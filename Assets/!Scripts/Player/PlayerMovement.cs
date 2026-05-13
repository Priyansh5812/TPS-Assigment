using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovementData playerData;
    CharacterController cc;
    Camera cam;

    Vector3 inputDirection;
    Vector3 intendedDirection;
    Vector3 targetVelocity;
    Vector3 currentVelocity;
    Vector3 lastVelocity;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        PollInputs();
        GetIntendedDirection();
        CalculateVelocity();
    }

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

    void LateUpdate()
    {
        cc.Move(currentVelocity * Time.deltaTime);  
        
    }


    #region HELPERS
    Vector3 GetAccelaration()
    {
        Vector3 accDir = targetVelocity - currentVelocity;
        return accDir.normalized * playerData.Accelaration;
    }
    #endregion


}
