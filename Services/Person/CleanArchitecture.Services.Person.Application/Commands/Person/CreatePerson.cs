using CleanArchitecture.Services.Person.Application.Common.Repositories;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;
using CleanArchitecture.Services.Shared.DomainDesign.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CleanArchitecture.Services.Person.Application.Commands.Person;
public static class CreatePerson
{
    #region Command
    public record Command : IRequest<Response>
    {
        [StripHtml]
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public int GenderId { get; set; }
        public IEnumerable<AddressEntry> Addresses { get; set; }
    }

    public record AddressEntry
    {
        public string City { get; set; }
        public string Location { get; set; }
        public int WardNo { get; set; }
    }
    #endregion

    #region Validation
    public class CreatePersonValidator : AbstractValidator<Command>
    {
        private readonly IReadOnlyPersonRepository _readOnlyPersonRepository;
        public CreatePersonValidator(IReadOnlyPersonRepository readOnlyPersonRepository
            )
        {
            this._readOnlyPersonRepository=readOnlyPersonRepository;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required.").WithName("Name");


            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18)
                .WithMessage("{PropertyName} must be greater than 18.").WithName("Age");

            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName} is required.").WithName("Email")
                .MustAsync(async (command, _, cancellationToken) => !await _IsExist(command.Name, cancellationToken))
                .WithMessage((name) =>
                {
                    return $"{name.Name} already exists.";
                });

            RuleFor(x => x.GenderId)
                .GreaterThanOrEqualTo(1)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is required.").WithName("Gender");


            RuleForEach(x => x.Addresses)
                .NotEmpty()
               .ChildRules(address =>
               {
                   address.RuleFor(x => x.Location)
                        .NotEmpty()
                        .NotNull()
                        .WithMessage("{PropertyName} is required.").WithName("Location");

                   address.RuleFor(x => x.City)
                        .NotEmpty()
                        .NotNull()
                        .WithMessage("{PropertyName} is required.").WithName("City");

                   address.RuleFor(x => x.WardNo)
                        .NotEmpty()
                        .NotNull()
                        .GreaterThanOrEqualTo(1)
                        .WithMessage("{PropertyName} is required.").WithName("WardNo");


               });
        }

        private async Task<bool> _IsExist(string email, CancellationToken cancellationToken)
            => await _readOnlyPersonRepository.IsExist(email, cancellationToken);

    }
    #endregion

    #region Handler
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;

        public Handler(ILogger<Handler> logger,
            IMapper mapper,
            IPersonRepository personRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _personRepository=personRepository;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.Aggregates.Person.Entities.Person person = new(request.Name, request.Email, request.Age, 1);

                Gender gender = Enumeration.FromValue<Gender>(request.GenderId);

                person.SetGender(gender);

                foreach (var address in request.Addresses)
                {
                    person.SetAddress(new Domain.Aggregates.Person.ValueObjects.Address(address.City, address.Location, address.WardNo), 1);
                }

                var response = await _personRepository
                                    .CreateAsync(person, cancellationToken);

                await _personRepository
                    .UnitOfWork
                    .SaveEntitiesAsync(cancellationToken);

                return _mapper.Map<Response>(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{@request}", request);
                throw;
            }
        }
    }
    #endregion

    #region Response
    public record Response() : IMapFrom<Domain.Aggregates.Person.Entities.Person>
    {
        public Guid PersonGUID { get; init; }
        public string Name { get; init; }

        public void Mapping(Profile profile) => profile.CreateMap<Domain.Aggregates.Person.Entities.Person, Response>()
                .ForMember(d => d.PersonGUID, d => d.MapFrom(a => a.PersonGuid))
                .ForMember(d => d.Name, d => d.MapFrom(a => a.Name));
    }
    #endregion
}
