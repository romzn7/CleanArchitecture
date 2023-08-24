using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Services.Person.API.Routing;

public class PersonRouteAttribute : RouteAttribute
{
    private const string _routePrefix = "api/person";

    /// <summary>
    /// Creates a route, prefixed by the api/master-settings
    /// </summary>
    /// <param name="template"></param>
    public PersonRouteAttribute(string template) :
            base($"{_routePrefix}{template}")
    {
    }
}
