using System;
using Zenject;

public class GloballGameState : IInitializable
{
    public State CurrentState{ get; private set; }
    public event Action<State> OnStateChangeEvent;

    public void Initialize()
    {
        CurrentState = State.PlaneSelection;
    }

    public void ChangeCurrentState(State newState)
    {
        CurrentState = newState;
        OnStateChangeEvent?.Invoke(CurrentState);
    }
}

public enum State
{
    None = 0,
    PlaneSelection = 1,
    Gameplay = 2
}