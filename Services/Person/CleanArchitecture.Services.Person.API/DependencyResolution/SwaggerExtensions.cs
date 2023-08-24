using CleanArchitecture.Services.Person.API.Routing;
using CleanArchitecture.Services.Shared.Models.Middlewares;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Services.Person.API.DependencyResolution;

public static class SwaggerExtensions
{
    public static SwaggerGenOptions AddPersonSwaggerGen(this SwaggerGenOptions options)
    {
        options.EnableAnnotations();

        options.SwaggerDoc(ApiGroupings.PersonApiGroupingsName, new OpenApiInfo
        {
            Version = "v1",
            Title = "Person",
            Description = "Person Web API"
        });

        return options;
    }

    public static List<SwaggerEndpointDefinition> UsePersonSwaggerEndpoints(this List<SwaggerEndpointDefinition> swaggerEndpointDefinitions)
    {
        swaggerEndpointDefinitions.Add(new SwaggerEndpointDefinition($"/swagger/{ApiGroupings.PersonApiGroupingsName}/swagger.json", "Person"));

        return swaggerEndpointDefinitions;
    }
}