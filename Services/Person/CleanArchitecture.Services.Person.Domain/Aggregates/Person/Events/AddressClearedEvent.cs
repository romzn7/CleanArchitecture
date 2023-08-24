using MediatR;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;

public record AddressClearedEvent(long PersonId, int UserId) : INotification { }