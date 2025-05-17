public interface IStateMachine<TState>
{
    public void ChangeState<T>() where T : TState;
}

