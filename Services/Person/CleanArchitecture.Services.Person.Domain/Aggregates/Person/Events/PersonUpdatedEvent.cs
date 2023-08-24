using MediatR;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;
public record PersonUpdatedEvent(long PersonGuid, int UserId) : INotification { }
