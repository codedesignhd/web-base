using Microsoft.AspNetCore.Authorization;

namespace CodeDesign.WebAPI.Core.Authorization;


public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles) : base()
    {
        Roles = string.Join(",", roles);
    }
}
