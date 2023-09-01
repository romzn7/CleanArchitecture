using AutoFixture;
using AutoFixture.Kernel;
using CleanArchitecture.Services.Person.Application.Common.Repositories;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace CleanArchitecture.Services.Persion.Application.Tests.Commands.Person
{
    internal class CreatePersonTests : HandlerTestBase<CreatePerson.Handler, CreatePerson.Command, CreatePerson.Response>
    {

        private string _name;
        private string _email;
        private int _age;
        private int _userId;
        private int _genderId;
        private IEnumerable<CreatePerson.AddressEntry> _addresses;
        public override void SetupServices()
        {
            base.SetupServices();
            //InjectCurrentUserHelper();
            InjectMockedReadOnlyRepository<IReadOnlyPersonRepository, Services.Person.Domain.Aggregates.Person.Entities.Person>();
            InjectMockedRepository<IPersonRepository, Services.Person.Domain.Aggregates.Person.Entities.Person>();
            InjectMockedScoped<IUnitOfWork>();

            _name = Fixture.Create<string>();
            _email = _GenerateRandomEmailAddress();
            _age = Random.Shared.Next(18, 100);
            _userId = Fixture.Create<int>();
            _genderId = Random.Shared.Next(1, 3);
            _addresses = Fixture.CreateMany<CreatePerson.AddressEntry>();

            Mocker.GetMock<IReadOnlyPersonRepository>()
             .Setup(x => x.IsExist(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

            Mocker.GetMock<IPersonRepository>()
                .Setup(x => x.CreateAsync(It.IsAny<Services.Person.Domain.Aggregates.Person.Entities.Person>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Fixture.Create<Services.Person.Domain.Aggregates.Person.Entities.Person>());
        }
        [TestCase(null)]
        [TestCase("")]
        public void Validation_Should_faild_when_name_null_or_empty(string name)
        {
            //arrange
            CommandQuery = new CreatePerson.Command
            {
                Name = name,
                Email = _email,
                Age = _age,
                GenderId = _genderId,
                Addresses=_addresses
            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CommandQuery.Name));
        }

        [Test]
        public void Validation_Should_faild_when_name_already_exist()
        {
            Mocker.GetMock<IReadOnlyPersonRepository>()
               .Setup(x => x.IsExist(It.IsAny<string>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            //arrange
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = _age,
                GenderId = _genderId,
                Addresses=_addresses

            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CommandQuery.Email));

            Mocker.GetMock<IReadOnlyPersonRepository>()
               .Verify(x => x.IsExist(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validation_Should_faild_when_age_is_zero_or_negative(int age)
        {
            //arrange
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = age,
                GenderId = _genderId,
                Addresses=_addresses

            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CommandQuery.Age));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validation_Should_faild_when_genderid_is_zero_or_negative(int genderId)
        {
            //arrange
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = _age,
                GenderId = genderId,
                Addresses=_addresses

            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.PropertyName == nameof(CommandQuery.GenderId));
        }

        
        [TestCase(null)]
        [TestCase("")]
        public void Validation_Should_faild_when_city_null_or_empty(string city)
        {
            //arrange
            var address = _addresses.ToList();
            foreach (var item in address)
            {
                item.City = city;
            }
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = _age,
                GenderId = _genderId,
                Addresses=address
            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.ErrorMessage.Contains("City"));
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validation_Should_faild_when_location_null_or_empty(string location)
        {
            //arrange
            var address = _addresses.ToList();
            foreach (var item in address)
            {
                item.Location = location;
            }
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = _age,
                GenderId = _genderId,
                Addresses=address
            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.ErrorMessage.Contains("Location"));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validation_Should_faild_when_wardno_null_or_empty(int wardNo)
        {
            //arrange
            var address = _addresses.ToList();
            foreach (var item in address)
            {
                item.WardNo = wardNo;
            }
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = _age,
                GenderId = _genderId,
                Addresses=address
            };
            // Act
            AsyncTestDelegate sut = () => Mediator.Send(CommandQuery);
            var assertion = Assert.ThrowsAsync<ValidationException>(sut)!;
            assertion.Errors.Should().Contain(x => x.ErrorMessage.Contains("WardNo"));
        }

        [Test]
        public async Task handler_should_succeed()
        {
            CommandQuery = new CreatePerson.Command
            {
                Name = _name,
                Email = _email,
                Age = _age,
                GenderId = _genderId,
                Addresses = _addresses
            };

            var response = await Mediator.Send(CommandQuery, new CancellationToken());
            response.Should().NotBeNull();

            Mocker.GetMock<IReadOnlyPersonRepository>()
              .Verify(x => x.IsExist(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            Mocker.GetMock<IPersonRepository>()
                .Verify(x => x.CreateAsync(It.IsAny<Services.Person.Domain.Aggregates.Person.Entities.Person>(), It.IsAny<CancellationToken>()), Times.Once);

            Mocker.GetMock<IPersonRepository>()
                .Verify(x => x.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private string _GenerateRandomEmailAddress()
        {
            var randomString = Path.GetRandomFileName().Replace(".", "");
            var email = $"{randomString}@example.com";
            return email;
        }
    }
}
