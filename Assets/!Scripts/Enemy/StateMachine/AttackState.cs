using System;

public class AttackState : IEnemyState
{
    private readonly EnemyController enemyStateDriver;

    public AttackState(EnemyController enemyStateDriver)
    {
        this.enemyStateDriver = enemyStateDriver;
    }

    public void OnEnter(Action onCompleted = null)
    {
        onCompleted?.Invoke();
    }

    public void OnUpdate()
    {
    }

    public void OnExit(Action onCompleted = null)
    {
        onCompleted?.Invoke();
    }

    public void OnCheckTransition()
    {
    }
}
