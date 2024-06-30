using System.Security.Claims;

namespace DemoSocial.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserProfileIdByClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("UserProfileId", context);
    }

    public static Guid GetIdentityIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("IdentityId", context);
    }

    private static Guid GetGuidClaimValue(string key, HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        return Guid.Parse(input: identity?.FindFirst(key)?.Value);
    }
}
