public class User
{
    private User(Guid id, string userName, string passwordHash, string email)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;

        Roles.Add(new RoleEntity
        {
            Id = (int)Enum.Parse<Role>(Role.Accountant.ToString()),
            Name = Role.Accountant.ToString(),
            Permissions = [new PermissionEntity{
                Id=(int)Enum.Parse<Permission>(Permission.ADD_ACCOUNTANT.ToString()),
                Name=Permission.ADD_ACCOUNTANT.ToString()
            }]
        });
    }

    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public ICollection<RoleEntity> Roles { get; set; } = [];

    public static User Create(Guid id, string userName, string passwordHash, string email)
    {
        return new User(id, userName, passwordHash, email);
    }

}
