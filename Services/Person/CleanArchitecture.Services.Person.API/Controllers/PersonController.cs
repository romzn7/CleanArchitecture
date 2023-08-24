using CleanArchitecture.Services.Person.Application.Queries.Person;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Services.Person.API.Controllers;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = ApiGroupings.PersonApiGroupingsName)]
public class PersonController : PersonControllerBase
{
    private readonly ILogger<PersonController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private const string _swaggerOperationTag = "People";
    public PersonController(ILogger<PersonController> logger,
       IMapper mapper,
        IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("people")]
    [SwaggerOperation(
         Summary = "Creates a person",
         Description = "Creates a person",
         OperationId = "person.person.create",
         Tags = new[] { _swaggerOperationTag })]
    [SwaggerResponse(StatusCodes.Status201Created, "Creates a person", type: typeof(CreatePersonResponseDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Application failed to process the request")]
    //[Authorize(Policy = SecurityPolicies.UserRoleIsAdmin)]
    public async Task<IActionResult> CreatePerson([FromBody, SwaggerRequestBody("Create person parameters", Required = true)] CreatePersonDTO createPerson,
                                                  CancellationToken cancellationToken)
    {
        try
        {
            var command = _mapper.Map<CreatePerson.Command>(createPerson);
            var response = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<CreatePersonResponseDTO>(response));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreatePerson {@createPerson}", createPerson);
            throw;
        }
    }



    [HttpPost("peoples")]
    [SwaggerOperation(
      Summary = "Retrieves all people",
      Description = "Retrieves all people",
      OperationId = "person.person.searchpeople",
      Tags = new[] { _swaggerOperationTag })]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of people", type: typeof(PeopleResultDTO))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Application failed to process the request")]
    public async Task<ActionResult<PeopleResultDTO>> GetAdCategories([FromBody, SwaggerParameter("Search params", Required = true)] PeopleSearchDTO peopleSearchDto,
                                                                CancellationToken cancellationToken)
    {
        try
        {
            var command = _mapper.Map<GetPeople.Query>(peopleSearchDto);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(_mapper.Map<PeopleResultDTO>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAdCategories {@peopleSearchDto}", peopleSearchDto);
            throw;
        }
    }
}
