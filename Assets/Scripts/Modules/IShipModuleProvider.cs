public interface IShipModuleProvider
{
    public T GetModule<T>() where T : IShipModule;
}

