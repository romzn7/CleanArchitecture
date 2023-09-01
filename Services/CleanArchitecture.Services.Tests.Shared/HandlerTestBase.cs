using AutoFixture;
using CleanArchitecture.Services.Shared.Application.Behaviours;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using CleanArchitecture.Services.Shared.Infrastructure.Persistence;
using CleanArchitecture.Services.Tests.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq.AutoMock;

namespace CleanArchitecture.Services.Tests.Shared;

public abstract class HandlerTestBase
{
    protected AutoMocker Mocker = new AutoMocker();
    protected Fixture Fixture = new Fixture();
    protected IMediator Mediator;
    protected IServiceCollection Services;
    protected ServiceProvider Provider;

    [SetUp]
    public virtual void Setup()
    {
        Mocker = new AutoMocker();
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        Services = new ServiceCollection();

        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AsyncValidatorBehavior<,>));
        Services.AddMemoryCache();

        this.SetupServices();
        Provider = Services.BuildServiceProvider();

        SetupHandler();
    }

    public virtual void SetupHandler()
    { }

    public virtual void SetupServices()
    {
    }
    protected void InjectMockedRepository<TRepository, TAggregate>()
        where TRepository : class, IRepository<TAggregate>
        where TAggregate : IQueryableEntity
        => Services.InjectMockedRepository<TRepository, TAggregate>(Mocker);

    protected void InjectMockedReadOnlyRepository<TRepository, TAggregate>()
        where TRepository : class, IReadOnlyRepository<TAggregate>
        where TAggregate : IQueryableEntity
        => Services.InjectMockedReadOnlyRepository<TRepository, TAggregate>(Mocker);

    protected void InjectMockedTransient<T>()
       where T : class
       => Services.InjectMockedTransient<T>(Mocker);

    protected void InjectMockedScoped<T>()
       where T : class
       => Services.InjectMockedScoped<T>(Mocker);

    protected void InjectMockedSingleton<T>()
       where T : class
       => Services.InjectMockedSingleton<T>(Mocker);

    //protected virtual void InjectCurrentUserHelper(int? userId = null)
    //{
    //    InjectMockedSingleton<ICurrentUserHelper>();
    //    userId = !userId.HasValue ? Random.Shared.Next(1, 1000) : userId.Value;

    //    Fixture.Register<CurrentTraderInfo>(() => new CurrentTraderInfo(userId.Value, Random.Shared.Next(1, 6), new string[] { "AU" }, new int[] { 1 }));

    //    Mocker.GetMock<ICurrentUserHelper>()
    //        .Setup(x => x.GetCurrentTrader())
    //        .ReturnsAsync(() => Fixture.Create<CurrentTraderInfo>());
    //}
}

public abstract class HandlerTestBase<THandler, TRequest> : HandlerTestBase
    where THandler : AsyncRequestHandler<TRequest>
    where TRequest : IRequest
{
    protected THandler Handler;
    protected TRequest Command;

    [SetUp]
    public new virtual void Setup()
    {
        Mocker = new AutoMocker();
        var validatorLogger = Mocker.GetMock<ILogger<AsyncValidatorBehavior<TRequest, Unit>>>();

        Services = new ServiceCollection();

        Services.AddValidatorsFromAssemblyContaining<TRequest>(lifetime: ServiceLifetime.Scoped);
        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AsyncValidatorBehavior<,>));

        Services.AddMediatR(typeof(THandler));
        Services.AddScoped<ILogger<AsyncValidatorBehavior<TRequest, Unit>>>(f => validatorLogger.Object);
        Services.AddTransient<ILogger<THandler>>(_ => Mocker.GetMock<ILogger<THandler>>().Object);
        Services.AddMemoryCache();

        this.SetupServices();


        Services.AddTransient<ILogger<THandler>>(_ =>
            Mocker.GetMock<ILogger<THandler>>().Object
        );

        Services.AddAutoMapper(typeof(THandler).Assembly);

        Provider = Services.BuildServiceProvider();
        Mediator = Provider.GetRequiredService<IMediator>();

        SetupHandler();
    }

    public new virtual void SetupHandler()
    { }

    public new virtual void SetupServices()
    {
    }


    protected virtual TRequest CreateCommand()
        => default(TRequest);
}

public abstract class HandlerTestBase<THandlder, TRequest, TResponse> : HandlerTestBase
    where THandlder : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected THandlder Handler;
    protected TRequest CommandQuery;
    protected TResponse Response;

    [SetUp]
    public new virtual void Setup()
    {
        var validatorLogger = Mocker.GetMock<ILogger<AsyncValidatorBehavior<TRequest, TResponse>>>();

        Services = new ServiceCollection();

        Services.AddValidatorsFromAssemblyContaining<TRequest>(ServiceLifetime.Scoped);
        Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AsyncValidatorBehavior<,>));
        Services.AddMemoryCache();
        Services.AddMediatR(typeof(THandlder));
        Services.AddScoped<ILogger<AsyncValidatorBehavior<TRequest, TResponse>>>(f => validatorLogger.Object);
        Services.AddTransient<ILogger<THandlder>>(_ => Mocker.GetMock<ILogger<THandlder>>().Object);
        Services.AddAutoMapper(typeof(THandlder).Assembly);
        this.SetupServices();
        Provider = Services.BuildServiceProvider();
        Mediator = Provider.GetRequiredService<IMediator>();

        SetupHandler();
    }

    public new virtual void SetupHandler()
    { }

    public new virtual void SetupServices()
    {
    }

    protected virtual TRequest CreateCommandQuery()
        => default(TRequest);
}
