public interface IEnemyState
{   
    public void OnEnter(System.Action onCompleted = null);
    public void OnUpdate();
    public void OnExit(System.Action onCompleted = null);
    public void OnCheckTransition();
}
