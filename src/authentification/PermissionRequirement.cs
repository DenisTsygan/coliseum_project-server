using Microsoft.AspNetCore.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permission[] Permissions { get; set; } = [];

    public PermissionRequirement(Permission[] permissions)
    {
        Permissions = permissions;
    }
}