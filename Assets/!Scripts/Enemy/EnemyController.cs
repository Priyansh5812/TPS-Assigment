using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
 
    // Holds one reusable instance per enemy state type.
    Dictionary<System.Type, IEnemyState> stateReg;

    // Tracks the currently active state for this enemy.
    IEnemyState currentEnemyState;

    // Prevents overlapping transitions while a state change is in progress.
    bool isChangingState = false;

    //Components
    NavMeshAgent agent;
    MeshRenderer meshRenderer;
    Transform playerTransform;
    Animator animator;
    IDamageHandler vitality;
    IDamageHandler playerVitality;
    LayerMask playerMask;
    RaycastHit[] hitInfos = new RaycastHit[1];

    void OnEnable()
    {   
        InitializeComponents();
        ApplyEnemyData();
        InitializeStateReg();
        InitiateStateChange(typeof(FollowState));
    }

    void InitializeComponents()
    {   
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if(meshRenderer == null)
            meshRenderer = GetComponentInChildren<MeshRenderer>();

        if(playerTransform == null)
            playerTransform = EventManager.GetPlayerTransform.Invoke();

        if (playerVitality == null)
            playerVitality = EventManager.GetPlayerVitality.Invoke();

        if (vitality == null)
            vitality = GetComponent<EnemyVitalityModule>();

        playerMask = 1 << LayerMask.NameToLayer("Player");
    }

    void ApplyEnemyData()
    {
        agent.speed = enemyData.movementSpeed;
        agent.acceleration = enemyData.accelaration;
        meshRenderer.material = enemyData.meshMaterial;
        vitality.InitializeDamageData(enemyData);
    }
    


    void InitializeStateReg()
    {
        stateReg = DictionaryPool<Type, IEnemyState>.Get();

        // Register each available state here so it can be looked up by type.
        stateReg[typeof(FollowState)] = new FollowState(this , agent, playerTransform);
        stateReg[typeof(AttackState)] = new AttackState(this, playerTransform, agent, animator, enemyData.animationDuration);
    }

    void Update()
    {

        if (isChangingState)
            return;

        currentEnemyState?.OnUpdate();
        currentEnemyState?.OnCheckTransition();
    }


    public void InitiateStateChange(Type type, Action OnExit = null, Action OnNewStateEnter = null)
    {
        // Let the current state clean itself up before swapping references.
        currentEnemyState?.OnExit(OnExit);

        if (!stateReg.ContainsKey(type))
        {
            Debug.LogError($"{type.Name} does not exist as a state. Check state Registery");
            return;
        }

        isChangingState = true;
        currentEnemyState = stateReg[type];

        // Enter the new state and clear the transition lock once it finishes.
        currentEnemyState.OnEnter(OnNewStateEnterCompletion);
        void OnNewStateEnterCompletion()
        {

            isChangingState = false;
            OnNewStateEnter?.Invoke();
        }
    }

    public void CheckToDamagePlayer()
    {
        Ray ray = new Ray(this.transform.position , this.transform.forward);
        if (Physics.RaycastNonAlloc(ray, hitInfos, enemyData.attackRange, playerMask) > 0)
        {
            Debug.Log("Damage inflicted to Player");
            vitality.InflictDamage(playerVitality);
        }
    }

    void DeInitReferences()
    {
        if (stateReg == null)
            return;

        DictionaryPool<Type, IEnemyState>.Release(stateReg);
        stateReg = null;
        currentEnemyState = null;
    }



    void OnDisable()
    {
        DeInitReferences();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
#endif
}
