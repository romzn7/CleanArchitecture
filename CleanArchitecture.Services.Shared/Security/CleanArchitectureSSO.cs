using CleanArchitecture.Services.Shared.Models.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CleanArchitecture.Services.Shared.Security;

public static class CleanArchitectureSSO
{

    //Must be consistent accross applications that uses cookie auth
    private const string _APP_NAME = "CleanArchitecture";
    private const string _APP_SHARED_COOKIE_NAME = "__CleanArchitecture";
    private const string _COOKIE_PURPOSE = "Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware";
    private static string[] _COOKIE_SUB_PURPOSE = { "Cookies.Application", "v2" };


    public static IServiceCollection AddCleanArchitectureSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var ssoSettings = configuration.GetSection(nameof(SSOSettings)).Get<SSOSettings>();

        services.ConfigureApplicationSecurity(new CleanArchitectureSSOSettings
        {
            Authority = ssoSettings.Authority,
            CommonRingPath = ssoSettings.SecurityProviderPath,
            Domain = ssoSettings.CookieDomain,
            ClientId = ssoSettings.ClientId,
            ClientSecret= ssoSettings.ClientSecret,
            RedirectUri = ssoSettings.RedirectUri
        }, ssoSettings.ApiResourceName);

        return services;
    }

    private static IServiceCollection ConfigureApplicationSecurity(this IServiceCollection services, CleanArchitectureSSOSettings ssoSettings, string apiResourceName)
    {
        if (string.IsNullOrEmpty(ssoSettings?.CommonRingPath))
            throw new ArgumentNullException(nameof(ssoSettings.CommonRingPath));

        //Use shared cookie auth for all skynet apps
        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(ssoSettings.CommonRingPath))
            .SetApplicationName(_APP_NAME);

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = _APP_SHARED_COOKIE_NAME;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Domain = string.IsNullOrEmpty(ssoSettings.Domain) ? options.Cookie.Domain : ssoSettings.Domain;
            options.Cookie.Name = _APP_SHARED_COOKIE_NAME;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.Path = "/";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
            options.SlidingExpiration = true;

            options.CookieManager = new ChunkingCookieManager();
            options.TicketDataFormat = new SecureDataFormat<AuthenticationTicket>(new TicketSerializer(),
                DataProtectionProvider.Create(new DirectoryInfo(ssoSettings.CommonRingPath),
                        (builder) => { builder.SetApplicationName(_APP_NAME); })
                    .CreateProtector(_COOKIE_PURPOSE, _COOKIE_SUB_PURPOSE));
        })
        .AddOpenIdConnect(options =>
        {
            options.Authority = ssoSettings.Authority;
            options.ClientId = ssoSettings.ClientId;
            options.ClientSecret = ssoSettings.ClientSecret;
            options.UseTokenLifetime = false;
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("imagegalleryapi.fullaccess");
            options.RequireHttpsMetadata = true;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.UsePkce = true;
            options.SaveTokens= true;
            options.GetClaimsFromUserInfoEndpoint= true;
            options.CallbackPath = "/signin-oidc";
        })
        .AddJwtBearer(options =>
        {
            options.Authority = ssoSettings.Authority;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters.ValidAudiences = new List<string>() { apiResourceName };
            options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            options.TokenValidationParameters.ValidIssuer = ssoSettings.Authority;
        });

        //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        //    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        //    {
        //        options.SignInScheme=CookieAuthenticationDefaults.AuthenticationScheme;
        //        options.Authority = "https://localhost:5001/";
        //        options.ClientId="imagegalleryclient";
        //        options.ClientSecret="apisecret";
        //        options.ResponseType="code";
        //        //options.Scope.Add("openid");
        //        //options.Scope.Add("profile");
        //        options.CallbackPath = new PathString("/signin-oidc");
        //        options.SaveTokens= true;
        //    })
        //.AddJwtBearer(options =>
        //{
        //    options.Authority = ssoSettings.Authority;
        //    options.RequireHttpsMetadata = true;
        //    options.TokenValidationParameters.ValidAudiences = new List<string>() { apiResourceName };
        //    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        //    options.TokenValidationParameters.ValidIssuer = ssoSettings.Authority;
        //});

        System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        return services;
    }
}
