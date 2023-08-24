using MediatR;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;
public record PersonAddedEvent(long PersonId, Guid PersonGuid, string name, int UserId) : INotification { }
