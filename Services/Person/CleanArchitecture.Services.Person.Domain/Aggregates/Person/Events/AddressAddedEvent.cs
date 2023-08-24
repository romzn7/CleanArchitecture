using CleanArchitecture.Services.Person.Domain.Aggregates.Person.ValueObjects;
using MediatR;
namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;
public record AddressAddedEvent(long PersonId, Address address, int UserId) : INotification { }
