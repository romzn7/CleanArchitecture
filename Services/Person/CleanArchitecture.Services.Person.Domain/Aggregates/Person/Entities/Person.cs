using Ardalis.GuardClauses;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.ValueObjects;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;

namespace CleanArchitecture.Services.Person.Domain.Aggregates.Person.Entities;

public class Person : AuditableEntity, IAggregateRoot
{
    public Guid PersonGuid { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public int Age { get; private set; }
    public Gender Gender { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Address> _address = new();
    public IEnumerable<Address> Address => _address;

    public Person(string name, string email, int age, int addedBy)
    {
        PersonGuid = Guard.Against.Default(Guid.NewGuid());
        Name = Guard.Against.NullOrEmpty(name);
        Email= Guard.Against.NullOrEmpty(email);
        Age = Guard.Against.NegativeOrZero(age);
        AddedBy = Guard.Against.NegativeOrZero(addedBy);
        IsActive = true;

        _AddPersonAddedEvent();

    }

    public void DeletePerson(int userId)
    {
        this.IsActive = false;

        if (!IsTransient())
        {
            this.UpdatedBy = Guard.Against.NegativeOrZero(userId);
            _AddPersonDeletedEvent();
        }
    }

    public void UpdatePerson(string name, string email, int age, int updatedBy)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Email= Guard.Against.NullOrEmpty(email);
        Age = Guard.Against.NegativeOrZero(age);
        UpdatedBy = Guard.Against.NegativeOrZero(updatedBy);

        _AddPersonUpdatedEvent();
    }

    public void SetGender(Gender gender)
    {
        Guard.Against.Null(gender);
        Gender = gender;
    }

    public void SetAddress(Address address, int userId)
    {
        Guard.Against.Null(address);

        _AddressExist(address);

        _address.Add(address);

        if (!IsTransient())
        {
            this.UpdatedBy = Guard.Against.NegativeOrZero(userId);
            _AddPersonUpdatedEvent();
        }
    }

    public void ClearAddress(int userId)
    {
        _address.Clear();

        if (!IsTransient())
        {
            this.UpdatedBy = Guard.Against.NegativeOrZero(userId);
            _AddAddressClearedEvent();
            _AddPersonUpdatedEvent();
        }
    }

    public void RemoveAddress(Address address, int userId)
    {
        Guard.Against.Null(address);

        _AddressExist(address);

        var addressConfig = _address.Single(x => x.WardNo.Equals(address.WardNo) && x.Location.Equals(address.Location) && x.City.Equals(address.City));
        _address.RemoveAll(x => x.WardNo.Equals(address.WardNo) && x.Location.Equals(address.Location) && x.City.Equals(address.City));

        if (!IsTransient())
        {
            this.UpdatedBy = Guard.Against.NegativeOrZero(userId);
            _AddAddressAddedEvent(address);
            _AddPersonUpdatedEvent();
        }
    }
    private void _AddressExist(Address address) => Guard.Against.InvalidInput<Address>(address, nameof(address),
    x => !_address.Any(x => x.WardNo.Equals(address.WardNo) && x.Location.Equals(address.Location) && x.City.Equals(address.City)),
    $"{address.City},{address.Location}-{address.WardNo} is already added in the Address");


    #region Events
    private void _AddPersonAddedEvent()
        => AddDomainEvent(new PersonAddedEvent(this.Id, this.PersonGuid, this.Name, this.AddedBy));

    private void _AddPersonUpdatedEvent()
        => AddDomainEvent(new PersonUpdatedEvent(this.Id, this.UpdatedBy!.Value));

    private void _AddPersonDeletedEvent()
        => AddDomainEvent(new PersonDeletedEvent(this.Id, this.UpdatedBy!.Value));


    private void _AddAddressClearedEvent()
        => AddDomainEvent(new AddressClearedEvent(this.Id, this.UpdatedBy!.Value));

    private void _AddAddressAddedEvent(Address address)
        => AddDomainEvent(new AddressAddedEvent(this.Id, address, this.UpdatedBy!.Value));

    private void _AddAddressRemovedEvent(Address address)
        => AddDomainEvent(new AddressRemovedEvent(this.Id, address, this.UpdatedBy!.Value));
    #endregion

}
