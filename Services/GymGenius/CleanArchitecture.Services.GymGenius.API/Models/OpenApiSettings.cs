namespace CleanArchitecture.Services.Person.API.Models;

//public class CreatePersonDTOFilter : ISchemaFilter
//{
//    public void Apply(OpenApiSchema schema, SchemaFilterContext context) => schema.Example = new OpenApiObject
//    {
//        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.Name))] = new OpenApiString("Person 1"),
//        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.Email))] = new OpenApiString("person1@yopmail.com"),
//        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.Age))] = new OpenApiInteger(18),
//        [System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(CreatePersonDTO.GenderId))] = new OpenApiInteger(1),
//    };
//}