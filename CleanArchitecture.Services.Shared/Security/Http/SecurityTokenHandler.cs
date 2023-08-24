using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CleanArchitecture.Services.Shared.Security.Http;

public class SecurityTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<SecurityTokenHandler> _logger;

    public SecurityTokenHandler(IHttpContextAccessor contextAccessor, ILogger<SecurityTokenHandler> logger)
    {
        this._contextAccessor = contextAccessor;
        this._logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = await _GetAccessToken(_contextAccessor.HttpContext);

            if (!string.IsNullOrEmpty(accessToken))
                request.SetBearerToken(accessToken);
            else
                _logger.LogWarning("No access token found from the current context");

            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    #region Helpers

    private async Task<string> _GetAccessToken(HttpContext httpContext)
    {
        var accessToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        if (string.IsNullOrEmpty(accessToken))
        {
            //try to retrieve from bearer
            var access = await _contextAccessor.HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (access?.Succeeded ?? false)
                accessToken = access.Ticket.Properties.GetTokenValue("access_token");
            else
            {
                //try to retrieve from cookie
                access = await _contextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (access?.Succeeded ?? false && (access?.Ticket?.Properties?.Items?.ContainsKey("access_token") ?? false))
                    accessToken = access.Ticket.Properties.Items["access_token"];
            }
        }

        return accessToken;
    }

    #endregion
}
