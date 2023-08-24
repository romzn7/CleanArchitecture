using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Services.Person.API.Models;

public class CreatePersonDTOFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context) => schema.Example = new OpenApiObject
    {
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.Name))] = new OpenApiString("Person 1"),
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.Email))] = new OpenApiString("person1@yopmail.com"),
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.Age))] = new OpenApiInteger(18),
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.GenderId))] = new OpenApiInteger(1),
    };
}


public class PeopleSearchDTOFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context) => schema.Example = new OpenApiObject
    {
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(PeopleSearchDTO.PageSize))] = new OpenApiInteger(20),
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(PeopleSearchDTO.PageNumber))] = new OpenApiInteger(1),
        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(PeopleSearchDTO.SearchTerm))] = new OpenApiString("")
    };
}