public class Retrieving : DroneStateBase
{
    private IMover Mover;
    private IBase Target;

    public Retrieving(IStateMachine<IDroneState> droneStateMachine, IShipModuleProvider moduleProvider) : base(droneStateMachine, moduleProvider)
    {
        Mover = ModuleProvider.GetModule<IMover>();
    }

    public override void Enter()
    {
        base.Enter();

        Target = ModuleProvider.GetModule<IBaseLocator>().GetSuitableBase();
    }

    public override void Check()
    {
        base.Check();

        // TODO Use real physics instead
        if (!Mover.TryMoveTo(Target))
            DroneStateMachine.ChangeState<Mining>();
    }
}

