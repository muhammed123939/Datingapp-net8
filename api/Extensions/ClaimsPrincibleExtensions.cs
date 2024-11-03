using System.Security.Claims;
namespace api.Extensions;

public static class ClaimsPrincibleExtensions
{
    

    public static string GetUsername (this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier) 
       ?? throw new Exception ("cannot get username from token");
        return username ; 
    }

}
