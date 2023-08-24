using MediatR;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;
public record PersonDeletedEvent(long PersonGuid, int UserId) : INotification { }
