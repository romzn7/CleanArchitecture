using CleanArchitecture.Services.GymGenius.API.Routing;
using CleanArchitecture.Services.Shared.Models.Middlewares;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanArchitecture.Services.GymGenius.API.DependencyResolution;

public static class SwaggerExtensions
{
    public static SwaggerGenOptions AddGymGeniusSwaggerGen(this SwaggerGenOptions options)
    {
        options.EnableAnnotations();

        options.SwaggerDoc(ApiGroupings.GymGeniusApiGroupingsName, new OpenApiInfo
        {
            Version = "v1",
            Title = "GymGenius",
            Description = "GymGenius Web API"
        });

        return options;
    }

    public static List<SwaggerEndpointDefinition> UseGymGeniusSwaggerEndpoints(this List<SwaggerEndpointDefinition> swaggerEndpointDefinitions)
    {
        swaggerEndpointDefinitions.Add(new SwaggerEndpointDefinition($"/swagger/{ApiGroupings.GymGeniusApiGroupingsName}/swagger.json", "GymGenius"));

        return swaggerEndpointDefinitions;
    }
}