using System;
using System.Collections.Generic;

public class DroneStateMachine : IStateMachine<IDroneState>
{
    private readonly Dictionary<Type, IDroneState> AllStates = new();

    private IDroneState CurrentState;

    public DroneStateMachine()
    {
    }

    public DroneStateMachine(IShipModuleProvider moduleProvider)
    {
        // TODO Use DI instead
        AllStates[typeof(Advancing)] = new Advancing(this, moduleProvider);
        AllStates[typeof(Mining)] = new Mining(this, moduleProvider);
        AllStates[typeof(Retrieving)] = new Retrieving(this, moduleProvider);
        AllStates[typeof(Unloading)] = new Unloading(this, moduleProvider);
        AllStates[typeof(Idle)] = new Idle(this, moduleProvider);
    }

    public void ChangeState<T>() where T : IDroneState
    {
        CurrentState?.Exit();

        CurrentState = AllStates[typeof(T)];

        CurrentState?.Enter();
    }

    public void Check() => CurrentState?.Check();
}

