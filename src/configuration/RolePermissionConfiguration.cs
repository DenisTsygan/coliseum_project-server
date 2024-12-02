public class RolePermissionConfiguration
{
    private readonly AuthorizationOptions _authorizationOptions;
    public RolePermissionConfiguration(AuthorizationOptions authorizationOptions)
    {
        _authorizationOptions = authorizationOptions;
        this.Configure();
    }

    public void Configure()//TODO add to DB
    {
        var items = ParseRolePermissions();
        Console.WriteLine("Configure Configure");
        foreach (var item in items)
        {
            Console.WriteLine("ParseRolePermissions");

            Console.WriteLine(item.ToString());
            Console.WriteLine(item.RoleId);
            Console.WriteLine(item.PermissionId);
        }
    }
    public RolePermissionEntity[] ParseRolePermissions()
    {
        return _authorizationOptions.RolePermissions
            .SelectMany(rp => rp.Permissions.Select(p => new RolePermissionEntity
            {
                RoleId = (int)Enum.Parse<Role>(rp.Role),
                PermissionId = (int)Enum.Parse<Permission>(p)
            })).ToArray();
    }
}