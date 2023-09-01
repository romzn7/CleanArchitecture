using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using CleanArchitecture.Services.Shared.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;

namespace CleanArchitecture.Services.Tests.Shared.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection InjectMockedTransient<T>(this IServiceCollection services, AutoMocker mocker)
        where T : class
    {
        services.AddTransient<T>(_ => mocker.GetMock<T>().Object);
        return services;
    }

    public static IServiceCollection InjectMockedScoped<T>(this IServiceCollection services, AutoMocker mocker)
        where T : class
    {
        services.AddScoped<T>(_ => mocker.GetMock<T>().Object);
        return services;
    }

    public static IServiceCollection InjectMockedRepository<T, TAggregateRoot>(this IServiceCollection services, AutoMocker mocker)
        where T : class, IRepository<TAggregateRoot>
        where TAggregateRoot : IQueryableEntity
    {
        services.AddScoped<T>(_ => mocker.GetMock<T>().Object);
        mocker.GetMock<T>()
            .SetupGet(x => x.UnitOfWork)
            .Returns(mocker.GetMock<IUnitOfWork>().Object);

        return services;
    }

    public static IServiceCollection InjectMockedReadOnlyRepository<T, TAggregateRoot>(this IServiceCollection services, AutoMocker mocker)
        where T : class, IReadOnlyRepository<TAggregateRoot>
        where TAggregateRoot : IQueryableEntity
    {
        services.AddScoped<T>(_ => mocker.GetMock<T>().Object);

        return services;
    }

    public static IServiceCollection InjectMockedSingleton<T>(this IServiceCollection services, AutoMocker mocker)
        where T : class
    {
        services.AddSingleton<T>(_ => mocker.GetMock<T>().Object);
        return services;
    }
}