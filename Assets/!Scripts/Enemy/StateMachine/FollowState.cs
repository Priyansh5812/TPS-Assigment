using System;
using UnityEngine;
using UnityEngine.AI;

public class FollowState : IEnemyState
{
    readonly EnemyController enemyStateDriver;
    readonly NavMeshAgent agent;
    readonly Transform playerTransform;
    public FollowState(EnemyController enemyStateDriver , NavMeshAgent agent, Transform playerTransform)
    {
        this.enemyStateDriver = enemyStateDriver;
        this.agent = agent;
        this.playerTransform = playerTransform;
    }

    public void OnEnter(Action onCompleted = null)
    {
        Debug.Log("Entered Follow State");
        onCompleted?.Invoke();
    }

    public void OnUpdate()
    {
        agent.SetDestination(playerTransform.position);
    }

    public void OnExit(Action onCompleted = null)
    {
        Debug.Log("Exited Follow State");
        onCompleted?.Invoke();
    }

    public void OnCheckTransition()
    {
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            enemyStateDriver.InitiateStateChange(typeof(AttackState));
        }
    }
}
