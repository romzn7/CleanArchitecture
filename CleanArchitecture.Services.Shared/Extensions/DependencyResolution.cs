namespace SkyNet.Services.Shared.Extensions
{
    public interface IRegisterableService { }
    public interface ITransientService : IRegisterableService { }
    public interface IScopedService : IRegisterableService { }
    public interface ISingletonService : IRegisterableService { }
}