public class Mining : DroneStateBase
{
    private IDroneStorage Storage;

    public Mining(IStateMachine<IDroneState> droneStateMachine, IShipModuleProvider moduleProvider) : base(droneStateMachine, moduleProvider)
    {
        Storage = ModuleProvider.GetModule<IDroneStorage>();
    }

    public override void Check()
    {
        base.Check();

        if (!Storage.TryTopUp())
            DroneStateMachine.ChangeState<Retrieving>();
    }
}

