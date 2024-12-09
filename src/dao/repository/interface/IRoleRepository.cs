public interface IRoleRepository
{
    Task<List<RoleEntity>> GetList();
}