public class UserEntity
{
    public UserEntity()
    {
    }

    private UserEntity(Guid id, string userName, string passwordHash, string email)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
    }

    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public ICollection<RoleEntity> Roles { get; set; } = [];// one user can have many roles

    public static UserEntity Create(Guid id, string userName, string passwordHash, string email)
    {
        return new UserEntity(id, userName, passwordHash, email);
    }

}
