using System;
using UnityEngine;
using UnityEngine.AI;

// Keeps the enemy moving toward the player until attack range is reached
public class FollowState : IEnemyState
{
    // State driver used to request transitions
    readonly EnemyController enemyStateDriver;

    // Navigation agent used to move the enemy
    readonly NavMeshAgent agent;

    // Player transform used as the movement target
    readonly Transform playerTransform;

    // Stores the references needed while this follow state runs
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
        // Continuously updates the path toward the player position
        agent.SetDestination(playerTransform.position);
    }

    public void OnExit(Action onCompleted = null)
    {
        Debug.Log("Exited Follow State");
        onCompleted?.Invoke();
    }

    public void OnCheckTransition()
    {
        // Switches to the attack state once the enemy is close enough to stop moving
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            enemyStateDriver.InitiateStateChange(typeof(AttackState));
        }
    }
}
