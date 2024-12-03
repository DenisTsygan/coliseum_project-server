using Microsoft.AspNetCore.Components.Forms;

public interface IUserRepository
{
    Task Add(UserEntity user);

    Task<UserEntity> GetByEmail(string email);

    Task<HashSet<Permission>> GetUserPermissions(Guid userId);

    Task<List<User>> GetList();

}