using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CleanArchitecture.Services.Shared.Security;

[Authorize(AuthenticationSchemes = "Cookies,Bearer")]
public abstract class AuthorizedControllerBase : ControllerBase
{
    #region User information

    protected int? TraderId
    {
        get
        {
            int.TryParse(HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == IdentityModel.JwtClaimTypes.Subject)?.Value, out int traderId);
            return traderId;
        }
    }

    protected int? RoleId
    {
        get
        {
            int.TryParse(HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == IdentityModel.JwtClaimTypes.Role)?.Value, out int roleId);
            return roleId;
        }
    }

    protected IEnumerable<string> UserEntityCodes => (HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.Entities)?.Value ?? "").Replace(" ", "").Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>();
    protected IEnumerable<int> UserEntityIds => (HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityIds)?.Value ?? "")
        .Replace(" ", "")
        .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => { int.TryParse(x, out int id); return id; })
        .Where(x => x > 0) ?? Enumerable.Empty<int>();

    #endregion
}
