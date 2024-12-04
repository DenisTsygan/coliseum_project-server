public interface IUserRepository
{
    Task Add(UserEntity user);

    Task<UserEntity> GetByEmail(string email);

    Task<UserEntity> GetById(Guid id);

    Task<HashSet<Permission>> GetUserPermissions(Guid userId);

    Task<List<User>> GetList();

}