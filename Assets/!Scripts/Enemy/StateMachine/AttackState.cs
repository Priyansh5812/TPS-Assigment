using System;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : IEnemyState
{
    readonly EnemyController enemyStateDriver;
    readonly Transform playerTransform;
    readonly NavMeshAgent agent;
    readonly Animator animator;
    readonly int hash_attack;
    readonly int hash_static;
    float duration;
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

        animator.speed = 1 / duration;
        animator.CrossFade(hash_attack, 0.25f);
        onCompleted?.Invoke();
    }

    public void OnUpdate()
    {
        agent.SetDestination(playerTransform.position);
        agent.transform.LookAt(playerTransform);
    }

    public void OnExit(Action onCompleted = null)
    {
        animator.speed = 1;
        animator.CrossFade(hash_static, 0.25f);
        Debug.Log("Exited Attack State");
        onCompleted?.Invoke();
    }

    public void OnCheckTransition()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            enemyStateDriver.InitiateStateChange(typeof(FollowState));
        }
    }
}
