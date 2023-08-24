using CleanArchitecture.Services.Person.Application.Common.Repositories;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;
using Humanizer;
using Microsoft.AspNetCore.Http.Features;

namespace CleanArchitecture.Services.Person.Application.Queries.Person;
public static class GetPeople
{
    #region Query
    public record Query : IRequest<Response>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
    #endregion

    #region Validation
    public class GetPeopleValidator : AbstractValidator<Query>
    {
        public GetPeopleValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Invalid {PropertyName} (> 0)")
                .WithName("page number");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Invalid {PropertyName} (> 0)")
                .WithName("page size");

            When(x => !string.IsNullOrEmpty(x.SearchTerm), () =>
            {
                RuleFor(x => x.SearchTerm)
                .MinimumLength(3)
                .WithMessage("Please provide at least 3 letters to initiate a search.");
            });
        }
    }
    #endregion

    #region Handler
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IReadOnlyPersonRepository _readOnlyPersonRepository;

        public Handler(ILogger<Handler> logger,
            IMapper mapper, IReadOnlyPersonRepository readOnlyPersonRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _readOnlyPersonRepository=readOnlyPersonRepository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _readOnlyPersonRepository.GetAll(request.PageNumber, request.PageSize, request.SearchTerm, cancellationToken);

                var people = _mapper.Map<IEnumerable<PeopleResult>>(result.Results);

                return new Response(people, result.TotalRecords);

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
    public record Response(IEnumerable<PeopleResult> Results, int TotalRecords);

    public record AddressResult : IMapFrom<Domain.Aggregates.Person.ValueObjects.Address>
    {
        public string City { get; set; }
        public string Location { get; set; }
        public int WardNo { get; set; }
    }
    public record PeopleResult : IMapFrom<Domain.Aggregates.Person.Entities.Person>
    {
        public Guid PersonGuid { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public int Age { get; init; }
        public string Gender { get; init; }

        public IEnumerable<AddressResult> Addresses { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Aggregates.Person.Entities.Person, PeopleResult>()
                .ForMember(x => x.Addresses, x => x.MapFrom(g => g.Address))
                .ForMember(x => x.Gender, x => x.MapFrom(g => g.Gender.Name));
        }
    }
    #endregion
}
