public interface IDroneStorage : IShipModule
{
    public bool TryTopUp();

    public bool TryEmpty();
}

