using System;
using UnityEngine;
using UnityEngine.AI;

// Keeps the enemy facing the player and playing the attack behavior at close range
public class AttackState : IEnemyState
{
    // State driver used to request transitions
    readonly EnemyController enemyStateDriver;

    // Player transform used for facing and distance checks
    readonly Transform playerTransform;

    // Navigation agent used to track movement and range
    readonly NavMeshAgent agent;

    // Animator used to play the attack and idle states
    readonly Animator animator;

    // Cached hash for the attack animation state
    readonly int hash_attack;

    // Cached hash for the static animation state
    readonly int hash_static;

    // Attack animation duration used to normalize playback speed
    float duration;

    // Stores the references and animation data needed while this attack state runs
    public AttackState(EnemyController enemyStateDriver, Transform playerTransform, NavMeshAgent agent , Animator animator , float animationDuration)
    {
        this.enemyStateDriver = enemyStateDriver;
        this.playerTransform = playerTransform;
        this.agent = agent;
        this.animator = animator;
        hash_attack = Animator.StringToHash("Attack");
        hash_static = Animator.StringToHash("Static");
        duration = animationDuration;
    }

    public void OnEnter(Action onCompleted = null)
    {
        Debug.Log("Entered Attack State");

        // Adjusts the animator speed and blends into the attack animation
        animator.speed = 1 / duration;
        animator.CrossFade(hash_attack, 0.25f);
        onCompleted?.Invoke();
    }

    public void OnUpdate()
    {
        // Keeps the enemy close to the player and facing the player while attacking
        agent.SetDestination(playerTransform.position);
        agent.transform.LookAt(playerTransform);
    }

    public void OnExit(Action onCompleted = null)
    {
        // Restores normal animator speed and blends back to the static pose
        animator.speed = 1;
        animator.CrossFade(hash_static, 0.25f);
        Debug.Log("Exited Attack State");
        onCompleted?.Invoke();
    }

    public void OnCheckTransition()
    {
        // Switches back to follow when the player moves outside the stopping range
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            enemyStateDriver.InitiateStateChange(typeof(FollowState));
        }
    }
}
