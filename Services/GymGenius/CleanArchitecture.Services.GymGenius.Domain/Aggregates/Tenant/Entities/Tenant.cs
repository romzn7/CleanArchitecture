using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Enumerations;
using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Events;
using CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.ValueObjects;
using System.Runtime.CompilerServices;

namespace CleanArchitecture.Services.GymGenius.Domain.Aggregates.Tenant.Entities;

public class Tenant : AuditableEntity, IAggregateRoot
{
    public Guid TenantGUID { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDisable { get; private set; }
    public string LogoURL { get; private set; }

    public string Email { get; private set; }

    public string Number { get; private set; }

    public Address Address { get; private set; }

    public SocialMediaLinks SocialMediaLinks { get; private set; }
    public OperatingHours OperatingHours { get; private set; }

    public IEnumerable<FacilityType> FacilityTypes => _facilityTypes;
    public PaymentConfiguration PaymentConfiguration { get; private set; }
    public SecurityConfiguration SecurityConfiguration { get; private set; }


    private readonly List<FacilityType> _facilityTypes = new();

    public Tenant(string name, string description, Address address, OperatingHours operatingHours, SecurityConfiguration securityConfiguration)
    {
        TenantGUID = Guard.Against.Default(Guid.NewGuid());
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.NullOrEmpty(description);
        IsActive = true;
        IsDisable=false;
        Address = Guard.Against.Null(address);
        OperatingHours = Guard.Against.Null(operatingHours);
        SecurityConfiguration = Guard.Against.Null(securityConfiguration);

        _AddTenantAddedEvent();
    }

    public void UpdateTenant(string name, string description)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.NullOrEmpty(description);

        _AddTenantUpdatedEvent();
    }

    public void Delete()
    {
        IsActive = true;

        _AddTenantDeletedEvent();
    }

    public void Disable()
    {
        IsDisable = true;

        _AddTenantDisabledEvent();
    }

    public void Enable()
    {
        IsDisable = false;

        _AddTenantEnabledEvent();
    }

    public void AddFacilityType(FacilityType facilityType) => _facilityTypes.Add(Guard.Against.Null(facilityType));
    public void RemoveFacilityTypes(IEnumerable<FacilityType> facilityTypes) => _facilityTypes.RemoveAll(x => facilityTypes.Any(t => t.Name.Equals(x.Name)));

    public void AddPaymentConfiguration(AcceptedPaymentMethod paymentMethod)
    {
        Guard.Against.Null(paymentMethod);
        if (!PaymentConfiguration.AcceptedPaymentMethods.Contains(paymentMethod))
        {
            PaymentConfiguration.AcceptedPaymentMethods.Add(paymentMethod);
        }
        else
        {
            throw new InvalidOperationException($"Payment method '{paymentMethod}' is already added.");
        }
    }

    public void RemovePaymentConfiguration(AcceptedPaymentMethod paymentMethod)
    {
        Guard.Against.Null(paymentMethod);

        if (PaymentConfiguration.AcceptedPaymentMethods.Contains(paymentMethod))
        {
            PaymentConfiguration.AcceptedPaymentMethods.Remove(paymentMethod);
        }
        else
        {
            throw new InvalidOperationException($"Payment method '{paymentMethod}' is not found.");
        }
    }

    public void ClearPaymentConfiguration(AcceptedPaymentMethod paymentMethod)
    {
        PaymentConfiguration.AcceptedPaymentMethods.Clear();
    }

    #region Domain Events
    private void _AddTenantAddedEvent()
        => AddDomainEvent(new TenantAddedEvent(this.Id, this.TenantGUID, this.Name, this.AddedBy));

    private void _AddTenantDeletedEvent()
        => AddDomainEvent(new TenantDeletedEvent(this.Id, this.Name, this.AddedBy));

    private void _AddTenantUpdatedEvent()
        => AddDomainEvent(new TenantUpdatedEvent(this.Id, this.AddedBy));

    private void _AddTenantDisabledEvent()
        => AddDomainEvent(new TenantDisabledEvent(this.Id, this.AddedBy));

    private void _AddTenantEnabledEvent()
        => AddDomainEvent(new TenantEnabledEvent(this.Id, this.AddedBy));
    #endregion
}
