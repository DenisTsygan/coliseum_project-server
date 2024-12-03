public class User
{
    private User(Guid id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }

    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public ICollection<RoleEntity> Roles { get; set; } = [];// one user can have many roles

}
