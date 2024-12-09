public interface IUserRepository
{
    Task<User> Add(UserEntity user, int roleId);

    Task DeleteById(Guid userId);

    Task<UserEntity> GetByEmail(string email);

    Task<UserEntity> GetById(Guid id);

    Task<HashSet<Permission>> GetUserPermissions(Guid userId);

    Task<List<User>> GetList();

    Task<Dictionary<Guid, User>> GetListByIds(HashSet<Guid> guids);

}