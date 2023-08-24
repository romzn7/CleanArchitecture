using CleanArchitecture.Services.Person.Application.Queries.Person;
using CleanArchitecture.Services.Person.Domain.Aggregates.Person.Enumerations;
using Humanizer;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection.Metadata.Ecma335;
using static CleanArchitecture.Services.Person.Application.Queries.Person.GetPeople;

namespace CleanArchitecture.Services.Person.API.Models;
public class DTOs
{
}

#region Person
//[SwaggerSchemaFilter(typeof(CreatePersonDTOFilter))]
public class CreatePersonDTO : IMapTo<CreatePerson.Command>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public int GenderId { get; set; }

    public IEnumerable<AddressDTO> Addresses { get; set; }

    public void Mapping(Profile profile) => profile.CreateMap<CreatePersonDTO, CreatePerson.Command>()
                                    .ForMember(x => x.Addresses, x => x.MapFrom(d => d.Addresses));
}

public record AddressDTO : IMapTo<CreatePerson.AddressEntry>
{
    public string City { get; set; }
    public string Location { get; set; }
    public int WardNo { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPeople.AddressResult, AddressDTO>();
        profile.CreateMap<AddressDTO, CreatePerson.AddressEntry>();
    }

}

public record CreatePersonResponseDTO : IMapFrom<CreatePerson.Response>
{
    public Guid PersonGUID { get; init; }
    public string PersonName { get; init; }
    public void Mapping(Profile profile) => profile.CreateMap<CreatePerson.Response, CreatePersonResponseDTO>()
            .ForMember(x => x.PersonName, x => x.MapFrom(d => d.Name));
}

public record PeopleResultDTO : IMapFrom<GetPeople.Response>
{
    public IEnumerable<PeopleListDTO> Results { get; init; }
    public int TotalRecords { get; init; }
    public void Mapping(Profile profile) => profile.CreateMap<GetPeople.Response, PeopleResultDTO>()
            .ForMember(x => x.Results, x => x.MapFrom(d => d.Results))
            .ForMember(x => x.TotalRecords, x => x.MapFrom(d => d.TotalRecords));
}

public record PeopleListDTO : IMapFrom<GetPeople.PeopleResult>
{
    public Guid PersonGuid { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public int Age { get; init; }
    public string Gender { get; init; }

    public IEnumerable<AddressDTO> Addresses { get; set; }
}

[SwaggerSchemaFilter(typeof(PeopleSearchDTOFilter))]
public record PeopleSearchDTO : IMapTo<GetPeople.Query>
{
    public string? SearchTerm { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
#endregion
