public class Unloading : DroneStateBase
{
    private IDroneStorage Storage;

    public Unloading(IStateMachine<IDroneState> droneStateMachine, IShipModuleProvider moduleProvider) : base(droneStateMachine, moduleProvider)
    {
        Storage = ModuleProvider.GetModule<IDroneStorage>();
    }

    public override void Check()
    {
        base.Check();

        if (!Storage.TryEmpty())
            DroneStateMachine.ChangeState<Retrieving>();
    }
}

