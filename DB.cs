public class TestDb
{
    public static void GenerateData(IConfiguration configuration)//TODO add in DB when start
    {
        var authOptions = configuration.GetSection(nameof(AuthorizationOptions)).Get<AuthorizationOptions>();
        Console.WriteLine("123123");

        var roles = Enum.GetValues<Role>()
            .Select(r => new RoleEntity
            {
                Id = (int)r,
                Name = r.ToString()
            });
        var permissions = Enum.GetValues<Permission>()
            .Select(p => new PermissionEntity
            {
                Id = (int)p,
                Name = p.ToString()
            });
        foreach (var item in roles)
        {
            Console.WriteLine("GenerateData roles");

            Console.WriteLine(item.ToString());
            Console.WriteLine(item.Id);
            Console.WriteLine(item.Name);
            Console.WriteLine(item.Permissions);
        }
        foreach (var item in permissions)
        {
            Console.WriteLine("GenerateData permissions");

            Console.WriteLine(item.ToString());
            Console.WriteLine(item.Id);
            Console.WriteLine(item.Name);
            Console.WriteLine(item.Roles);
        }
        // and this generate table Role and Permission 
        var config = new RolePermissionConfiguration(authOptions);//table roleid permissionid
    }
}