public class AuthorizationOptions
{
    public RolePermissions[] RolePermissions { get; set; } = [];
    public AdminEntity? Admin { get; set; }
}

public class RolePermissions
{
    public string Role { get; set; } = string.Empty;

    public string[] Permissions { get; set; } = [];
}

public class AdminEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}