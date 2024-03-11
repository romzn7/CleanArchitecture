using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Services.Shared.Middlewares;

public class SwaggerOAuthMiddleware
{
    private readonly RequestDelegate next;
    public SwaggerOAuthMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (IsSwaggerUI(context.Request.Path))
        {
            // if user is not authenticated
            if (context.User.Identity is not null && !context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync("OpenIdConnect");
                return;
            }
        }
        await next.Invoke(context);
    }
    public bool IsSwaggerUI(PathString pathString)
    {
        return pathString.StartsWithSegments("/swagger") && !pathString.ToString().EndsWith("swagger.json");
    }
}
