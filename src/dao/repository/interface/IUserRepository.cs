using Microsoft.AspNetCore.Components.Forms;

public interface IUserRepository
{
    Task Add(User user);

    Task<User> GetByEmail(string email);

    Task<HashSet<Permission>> GetUserPermissions(Guid userId);

    Task<List<User>> GetList();

}