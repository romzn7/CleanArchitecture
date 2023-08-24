using MediatR;

namespace CleanArchitecture.Services.Shared.Application.Events;

public interface IAsyncMediator : IMediator { }

public class AsyncMediator : Mediator, IAsyncMediator
{
    public AsyncMediator(ServiceFactory serviceFactory) : base(serviceFactory) { }
    protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken) => _ParallelNoWait(allHandlers, notification, cancellationToken);

    private Task _ParallelNoWait(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
    {
        foreach (var handler in handlers)
        {
            Task.Run(() => handler(notification, cancellationToken));
        }

        return Task.CompletedTask;
    }
}
