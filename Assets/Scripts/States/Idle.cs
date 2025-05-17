public class Idle : DroneStateBase
{
    public Idle(IStateMachine<IDroneState> droneStateMachine, IShipModuleProvider moduleProvider) : base(droneStateMachine, moduleProvider)
    {
    }
}

