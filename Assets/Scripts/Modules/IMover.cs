public interface IMover : IShipModule
{
    public bool TryMoveTo(IHavePosition target);
}

