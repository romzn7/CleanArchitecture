

using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Events;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.ValueObjects;
using System;

namespace CleanArchitecture.Services.Person.Domain.Tests.UnitTests;

public class PersonTests
{
    private readonly Fixture _fixture = new Fixture();
    private Aggregates.Person.Entities.Person _Person;
    private Gender _gender;
    private string _name;
    private int _age;
    private string _email;
    private int _userId;
    private IEnumerable<Address> _Addresses;


    [SetUp]
    public void Setup()
    {
        _name = _fixture.Create<string>();
        _email = _fixture.Create<string>();
        _age = Random.Shared.Next(1, 100);
        _userId = Random.Shared.Next(1, 100);

        _Person = _fixture.Create<Aggregates.Person.Entities.Person>();
        _Addresses = _fixture.CreateMany<Address>();
        _gender = _fixture.Create<Gender>();
    }


    private Aggregates.Person.Entities.Person _CreateInstance() => new Aggregates.Person.Entities.Person(_name, _email, _age, _userId);

    [TestCase(0)]
    [TestCase(-1)]
    public void initialize_userId_should_fail(int multiplyer)
    {
        _userId *= multiplyer;
        TestDelegate td = () => _CreateInstance();

        var assertion = Assert.Throws<ArgumentException>(td);
        assertion.ParamName.Should().Be("addedBy");
    }

    [TestCase("")]
    [TestCase(null)]
    public void initialize_name_should_fail(string name)
    {
        _name = name;
        TestDelegate td = () => _CreateInstance();


        var assertion = name == null ? Assert.Throws<ArgumentNullException>(td)! :
            Assert.Throws<ArgumentException>(td)!;


        assertion.ParamName.Should().Be("name");
    }

    [TestCase("")]
    [TestCase(null)]
    public void initialize_email_should_fail(string email)
    {
        _email = email;
        TestDelegate td = () => _CreateInstance();


        var assertion = email == null ? Assert.Throws<ArgumentNullException>(td)! :
            Assert.Throws<ArgumentException>(td)!;


        assertion.ParamName.Should().Be("email");
    }


    [TestCase(0)]
    [TestCase(-1)]
    public void initialize_age_should_fail(int multiplyer)
    {
        _age *= multiplyer;
        TestDelegate td = () => _CreateInstance();

        var assertion = Assert.Throws<ArgumentException>(td);
        assertion.ParamName.Should().Be("age");
    }

    [Test]
    public void init_should_succeed()
    {
        _Person = _CreateInstance();

        _Person.Name.Should().Be(_name);
        _Person.Email.Should().Be(_email);
        _Person.Age.Should().Be(_age);
        _Person.AddedBy.Should().Be(_userId);
        _Person.IsActive.Should().Be(true);

        _Person.DomainEvents.Should().NotBeNull();
        _Person.DomainEvents.Should().NotBeEmpty();
        _Person.DomainEvents.Should().Contain(d => d.GetType() == typeof(PersonAddedEvent));
    }

    [TestCase("")]
    [TestCase(null)]
    public void UpdatePerson_Name_should_fail(string name)
    {
        // Arrange
        _Person = _CreateInstance();
        _name = name;
        // Act
        // Assert
        TestDelegate sut = () => _Person.UpdatePerson(_name, _email, _age, _userId);
        var assertion = name == null ? Assert.Throws<ArgumentNullException>(sut)! :
              Assert.Throws<ArgumentException>(sut)!;
        assertion.ParamName.Should().Be("name");
    }

    [TestCase("")]
    [TestCase(null)]
    public void UpdatePerson_Email_should_fail(string email)
    {
        // Arrange
        _Person = _CreateInstance();
        _email = email;
        // Act
        // Assert
        TestDelegate sut = () => _Person.UpdatePerson(_name, _email, _age, _userId);
        var assertion = email == null ? Assert.Throws<ArgumentNullException>(sut)! :
              Assert.Throws<ArgumentException>(sut)!;
        assertion.ParamName.Should().Be("email");
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void UpdatePerson_Age_should_fail(int age)
    {
        // Arrange
        _Person = _CreateInstance();
        _age = age;
        // Act
        // Assert
        TestDelegate sut = () => _Person.UpdatePerson(_name, _email, _age, _userId);
        var assertion = age == null ? Assert.Throws<ArgumentNullException>(sut)! :
              Assert.Throws<ArgumentException>(sut)!;
        assertion.ParamName.Should().Be("age");
    }


    [TestCase(0)]
    [TestCase(-1)]
    public void UpdatePerson_UserId_should_fail(int userId)
    {
        // Arrange
        _Person = _CreateInstance();
        _userId = userId;
        // Act
        // Assert
        TestDelegate sut = () => _Person.UpdatePerson(_name, _email, _age, _userId);
        var assertion = userId == null ? Assert.Throws<ArgumentNullException>(sut)! :
              Assert.Throws<ArgumentException>(sut)!;
        assertion.ParamName.Should().Be("updatedBy");
    }

    [Test]
    public void UpdatePerson_should_succeed()
    {
        // Arrange
        _Person = _CreateInstance();

        // Act
        // Assert
        _Person.UpdatePerson(_name, _email, _age, _userId);

        _Person.Name.Should().Be(_name);
        _Person.Email.Should().Be(_email);
        _Person.Age.Should().Be(_age);
        _Person.UpdatedBy.Should().Be(_userId);
        _Person.DomainEvents.Should().Contain(d => d.GetType() == typeof(PersonUpdatedEvent));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Delete_UserId_should_fail(int userId)
    {
        // Arrange
        _userId = userId;

        // Act
        // Assert
        TestDelegate sut = () => _Person.DeletePerson(_userId);
        var assertion = userId == null ? Assert.Throws<ArgumentNullException>(sut)! :
              Assert.Throws<ArgumentException>(sut)!;
        assertion.ParamName.Should().Be("userId");
    }

    [Test]
    public void Delete_should_succeed()
    {
        // Arrange
        // Act
        // Assert
        _Person.DeletePerson(_userId);

        _Person.UpdatedBy.Should().Be(_userId);
        _Person.IsActive.Should().Be(false);
        _Person.DomainEvents.Should().Contain(d => d.GetType() == typeof(PersonDeletedEvent));
    }

    [TestCase(null)]
    public void SetGender_should_fail(Gender gender)
    {
        // Arrange
        _Person = _CreateInstance();
        _gender = gender;
        // Assert
        TestDelegate sut = () => _Person.SetGender(_gender);
        var assertion = gender == null ? Assert.Throws<ArgumentNullException>(sut)! :
              Assert.Throws<ArgumentException>(sut)!;
        assertion.ParamName.Should().Be("gender");
    }

    [Test]
    public void SetGender_should_succeed()
    {
        // Arrange
        _Person = _CreateInstance();

        // Act
        // Assert
        _Person.SetGender(_gender);

        _Person.Gender.Should().Be(_gender);
    }

    [Test]
    public void clear_address_should_succeed()
    {
        // Arrange
        _Person = _CreateInstance();
        _SetAddresses();
        _ClearAddresses();
        // Act

        // Assert

        _Person.DomainEvents.Should().NotBeNull();
        _Person.Address.Count().Should().Be(0);
        _Person.DomainEvents.Should().Contain(d => d.GetType() == typeof(AddressClearedEvent));

    }

    [Test]
    public void set_address_should_succeed()
    {
        // Arrange
        _Person = _CreateInstance();

        _SetAddresses();

        _Person.Address.Count().Should().Be(_Addresses.Count());
        _Person.DomainEvents.Should().Contain(d => d.GetType() == typeof(AddressAddedEvent));
    }


    [Test]
    public void remove_address_should_succeed()
    {
        // Arrange
        _Person = _CreateInstance();
        _SetAddresses();

        // Act
        _RemoveAddress(_Person.Address.First());
        // Assert

        _Person.DomainEvents.Should().NotBeNull();
        _Person.Address.Count().Should().Be(_Addresses.Count() - 1);
        _Person.DomainEvents.Should().Contain(d => d.GetType() == typeof(AddressRemovedEvent));

    }

    private void _RemoveAddress(Address address)
    {
        _Person.RemoveAddress(address, _userId);
    }

    private void _ClearAddresses()
    {
        _Person.ClearAddress(_userId);
    }

    private void _SetAddresses()
    {
        foreach (var address in _Addresses)
        {
            _Person.SetAddress(address, _userId);
        }
    }
}
