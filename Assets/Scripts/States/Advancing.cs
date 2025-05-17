public class Advancing : DroneStateBase
{
    private IMover Mover;
    private Asteroid Target;

    public Advancing(IStateMachine<IDroneState> droneStateMachine, IShipModuleProvider moduleProvider) : base(droneStateMachine, moduleProvider)
    {
        Mover = ModuleProvider.GetModule<IMover>();
    }

    public override void Enter()
    {
        base.Enter();

        var asteroidLocator = ModuleProvider.GetModule<IAsteroidLocator>();
        Target = asteroidLocator.GetMostSuitableAsteroid();
    }

    public override void Check()
    {
        base.Check();

        // TODO Use real physics instead
        if (!Mover.TryMoveTo(Target))
            DroneStateMachine.ChangeState<Mining>();
    }
}

