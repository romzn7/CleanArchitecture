using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Services.GymGenius.API.Routing;

public class GymGeniusRouteAttribute : RouteAttribute
{
    private const string _routePrefix = "api/gymgenius";

    /// <summary>
    /// Creates a route, prefixed by the api/master-GymGenius
    /// </summary>
    /// <param name="template"></param>
    public GymGeniusRouteAttribute(string template) :
            base($"{_routePrefix}{template}")
    {
    }
}
