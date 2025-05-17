public abstract class DroneStateBase : IDroneState
{
    protected readonly IStateMachine<IDroneState> DroneStateMachine;
    protected readonly IShipModuleProvider ModuleProvider;

    public DroneStateBase(IStateMachine<IDroneState> droneStateMachine, IShipModuleProvider moduleProvider)
    {
        DroneStateMachine = droneStateMachine;
        ModuleProvider = moduleProvider;
    }

    public virtual void Enter()
    {
    }

    public virtual void Check()
    {
    }

    public virtual void Exit()
    {
    }

    // Under construction ->
    // -> Under repairing -> 
    // -> Under fueling -> 
    // -> Under disassembling

    // Finding ore -> Moving to ore -> Mining -> Moving to foundry -> Unloading -> Finding ore
}
